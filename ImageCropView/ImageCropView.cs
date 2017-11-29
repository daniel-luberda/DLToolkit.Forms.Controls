using System;
using Xamarin.Forms;
using FFImageLoading.Forms;
using FFImageLoading.Work;
using System.Collections.Generic;
using ImageSource = Xamarin.Forms.ImageSource;
using FFImageLoading.Transformations;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using FFImageLoading;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.IO;
using System.Reactive.Disposables;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DLToolkit.Forms.Controls
{
    [Helpers.ImageCropView.Preserve(AllMembers = true)]
    public class ImageCropView : ContentView
    {
        /// <summary>
        /// Used to avoid linking issues
        /// eg. when using only XAML
        /// </summary>
        public static void Init()
        {
#pragma warning disable 0219
            var dummy1 = typeof(ImageCropView);
#pragma warning restore 0219
        }

        readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        readonly Grid _root;
        readonly CustomCachedImage _image;
        readonly CropTransformation _crop;

        View _frame;

        const double MIN_SCALE = 1;

        CompositeDisposable _observables = new CompositeDisposable();
        IObservable<PinchGestureUpdatedEventArgs> _pinchObservable;
        IObservable<PanUpdatedEventArgs> _panObservable;

        public ImageCropView()
        {
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;

            _crop = new CropTransformation();

            _image = new CustomCachedImage()
            {
                LoadingDelay = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                InputTransparent = true,
                Aspect = Aspect.Fill,
                Transformations = new List<ITransformation>() { _crop },
            };

            _root = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                InputTransparent = true,
                Children = {
                    _image
                }
            };

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += PinchGesture_PinchUpdated; ;

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += PanGesture_PanUpdated;

            GestureRecognizers.Clear();
            GestureRecognizers.Add(pinchGesture);
            GestureRecognizers.Add(panGesture);

            _pinchObservable = Observable.FromEventPattern<PinchGestureUpdatedEventArgs>(pinchGesture, "PinchUpdated")
                                         .Select(v => v.EventArgs).Where(v => v.Status == GestureStatus.Running);

            _panObservable = Observable.FromEventPattern<PanUpdatedEventArgs>(panGesture, "PanUpdated")
                                       .Select(v => v.EventArgs).Where(v => v.StatusType == GestureStatus.Running);

            HandlePreviewTransformations(null, PreviewTransformations);
            Content = _root;
            ResetCrop();

            SetDelay();
        }

        public static readonly BindableProperty TransformationsProperty = BindableProperty.Create(nameof(Transformations), typeof(IList<ITransformation>), typeof(ImageCropView), new List<ITransformation>());

        public IList<ITransformation> Transformations
        {
            get { return (IList<ITransformation>)GetValue(TransformationsProperty); }
            set { SetValue(TransformationsProperty, value); }
        }


        public static readonly BindableProperty PreviewTransformationsProperty = BindableProperty.Create(nameof(PreviewTransformations), typeof(IList<ITransformation>), typeof(ImageCropView), new ObservableCollection<ITransformation>(), propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((ImageCropView)bindable).HandlePreviewTransformations(oldValue, newValue);
        });

        public IList<ITransformation> PreviewTransformations
        {
            get { return (IList<ITransformation>)GetValue(PreviewTransformationsProperty); }
            set { SetValue(PreviewTransformationsProperty, value); }
        }


        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(ImageCropView), default(ImageSource));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }


        public static readonly BindableProperty FrameViewProperty = BindableProperty.Create(nameof(FrameView), typeof(View), typeof(ImageCropView), default(View));

        public View FrameView
        {
            get { return (View)GetValue(FrameViewProperty); }
            set { SetValue(FrameViewProperty, value); }
        }


        public static readonly BindableProperty FramePaddingProperty = BindableProperty.Create(nameof(FramePadding), typeof(double), typeof(ImageCropView), default(double));

        public double FramePadding
        {
            get { return (double)GetValue(FramePaddingProperty); }
            set { SetValue(FramePaddingProperty, value); }
        }


        public static readonly BindableProperty PanSpeedProperty = BindableProperty.Create(nameof(PanSpeed), typeof(double), typeof(ImageCropView), 1d);

        public double PanSpeed
        {
            get { return (double)GetValue(PanSpeedProperty); }
            set { SetValue(PanSpeedProperty, value); }
        }


        public static readonly BindableProperty DelayProperty = BindableProperty.Create(nameof(Delay), typeof(int), typeof(ImageCropView), 100);

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }


        public static readonly BindableProperty PreviewMaxResolutionProperty = BindableProperty.Create(nameof(PreviewMaxResolution), typeof(int), typeof(ImageCropView), default(int));

        public int PreviewMaxResolution
        {
            get { return (int)GetValue(PreviewMaxResolutionProperty); }
            set { SetValue(PreviewMaxResolutionProperty, value); }
        }


        public static readonly BindableProperty RefinedMaxResolutionProperty = BindableProperty.Create(nameof(RefinedMaxResolution), typeof(int), typeof(ImageCropView), default(int));

        public int RefinedMaxResolution
        {
            get { return (int)GetValue(RefinedMaxResolutionProperty); }
            set { SetValue(RefinedMaxResolutionProperty, value); }
        }



        public static readonly BindableProperty MaxZoomProperty = BindableProperty.Create(nameof(MaxZoom), typeof(double), typeof(ImageCropView), 4d);

        public double MaxZoom
        {
            get { return (double)GetValue(MaxZoomProperty); }
            set { SetValue(MaxZoomProperty, value); }
        }


        void SetDelay()
        {
            _observables.Clear();

            var merged = _pinchObservable.Merge<object>(_panObservable);

            var fast = merged
                    .Sample(TimeSpan.FromMilliseconds(Delay))
                    .SubscribeOn(Scheduler.Default)
                    .Subscribe(v =>
                    {
                        _image.LoadImage();
                    });

            var refined = merged
                    .Throttle(TimeSpan.FromMilliseconds(Delay))
                    .Delay(TimeSpan.FromMilliseconds(Delay))
                    .SubscribeOn(Scheduler.Default)
                    .Subscribe(v =>
                    {
                        _image.LoadRefinedImage();
                    });


            _observables.Add(fast);
            _observables.Add(refined);
        }

        void PinchGesture_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Running:
                    double current =  (e.Scale - 1) / 2 * PanSpeed;
                    _crop.ZoomFactor = Clamp(_crop.ZoomFactor + current, MIN_SCALE, MaxZoom);
                    break;
            }
        }

        void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    var xOffset = e.TotalX / _crop.ZoomFactor / 100 * -1 * PanSpeed / _crop.CropWidthRatio;
                    var yOffset = e.TotalY / _crop.ZoomFactor / 100 * -1 * PanSpeed / _crop.CropHeightRatio;

                    _crop.XOffset = Clamp(_crop.XOffset + xOffset, -Width / 2 / _crop.ZoomFactor, Width / 2 / _crop.ZoomFactor);
                    _crop.YOffset = Clamp(_crop.YOffset + yOffset, -Height / 2 / _crop.ZoomFactor, Height / 2 / _crop.ZoomFactor);
                    break;
            }
        }

        void ResetCrop()
        {
            _crop.ZoomFactor = 1d;
            _crop.XOffset = 0d;
            _crop.YOffset = 0d;
        }

        internal void HandlePreviewTransformations(object oldValue, object newValue)
        {
            var oldNotify = oldValue as INotifyCollectionChanged;
            if (oldNotify != null)
                oldNotify.CollectionChanged -= NotifyCollectionChanged;

            var newNotify = newValue as INotifyCollectionChanged;
            if (newNotify != null)
                newNotify.CollectionChanged += NotifyCollectionChanged;
        }

        void NotifyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetPreviewTransformations();
        }

        void SetPreviewTransformations()
        {
            if (PreviewTransformations == null || PreviewTransformations.Count == 0)
            {
                _image.Transformations = new List<ITransformation>() { _crop };
            }
            else
            {
                var list = PreviewTransformations.ToList();
                list.Insert(0, _crop);
                _image.Transformations = list;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == PreviewTransformationsProperty.PropertyName)
            {
                ResetCrop();
                SetPreviewTransformations();
            }
            else if (propertyName == SourceProperty.PropertyName)
            {
                ResetCrop();
                _image.SetSource(Source);
            }
            else if (propertyName == FrameViewProperty.PropertyName)
            {
                if (_frame != null)
                {
                    _root.Children.Remove(_frame);
                }

                _frame = FrameView;

                if (FrameView != null)
                {
                    FrameView.InputTransparent = true;
                    FrameView.HorizontalOptions = LayoutOptions.FillAndExpand;
                    FrameView.VerticalOptions = LayoutOptions.FillAndExpand;
                    _root.Children.Add(FrameView);
                }
            }
            else if (propertyName == DelayProperty.PropertyName)
            {
                SetDelay();
            }
            else if (propertyName == PreviewMaxResolutionProperty.PropertyName)
            {
                _image.PreviewResolution = PreviewMaxResolution;
            }
            else if (propertyName == RefinedMaxResolutionProperty.PropertyName)
            {
                _image.RefinedResolution = RefinedMaxResolution;
            }
        }

        double _width = -1;
        double _height = -1;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > 0 && height > 0 && (Math.Abs(_width - width) > double.Epsilon || Math.Abs(_height - height) > double.Epsilon) )
            {
                _width = width;
                _height = height;
                ResetCrop();
                _crop.CropWidthRatio = width;
                _crop.CropHeightRatio = height;
                _image.LoadRefinedImage();
            }
        }

        public Task<Stream> GetImageAsJpegAsync(int quality = 90, int maxWidth = 0, int maxHeight = 0)
        {
            TaskParameter task = null;

            switch (_image.SourceType)
            {
                case FFImageLoading.Work.ImageSource.Url:
                    task = ImageService.Instance.LoadUrl(_image.Path);
                    break;
                case FFImageLoading.Work.ImageSource.Filepath:
                    task = ImageService.Instance.LoadFile(_image.Path);
                    break;
                case FFImageLoading.Work.ImageSource.ApplicationBundle:
                    task = ImageService.Instance.LoadFileFromApplicationBundle(_image.Path);
                    break;
                case FFImageLoading.Work.ImageSource.CompiledResource:
                    task = ImageService.Instance.LoadCompiledResource(_image.Path);
                    break;
                case FFImageLoading.Work.ImageSource.EmbeddedResource:
                    task = ImageService.Instance.LoadEmbeddedResource(_image.Path);
                    break;
                case FFImageLoading.Work.ImageSource.Stream:
                    task = ImageService.Instance.LoadStream(_image.Stream);
                    break;
            }

            var applied = (1 + (2 * (FramePadding / _crop.CropHeightRatio)));

            var transformations = (Transformations?.ToList() ?? new List<ITransformation>());
            transformations.Insert(0, new CropTransformation()
            {
                XOffset = _crop.XOffset,
                YOffset = _crop.YOffset,
                CropHeightRatio = _crop.CropHeightRatio,
                CropWidthRatio = _crop.CropWidthRatio,
                ZoomFactor = _crop.ZoomFactor * applied,
            });

            return task
                .Transform(transformations)
                .DownSample(maxWidth, maxHeight)
                .AsJPGStreamAsync(quality);
        }

        T Clamp<T>(T value, T minimum, T maximum) where T : IComparable
        {
            if (value.CompareTo(minimum) < 0)
                return minimum;
            if (value.CompareTo(maximum) > 0)
                return maximum;
            
            return value;
        }

        double MinMoreThan(double value, double than)
        {
            if (value.CompareTo(0) < 0)
                return Math.Min(value, -than);
            if (value.CompareTo(0) > 0)
                return Math.Max(value, than);
            
            return value;
        }

        class CustomCachedImage : CachedImage
        {
            string _cacheKey;
            string _refinedCacheKey;
            bool _isRefined;
            Guid _imageGuid;
            ImageSource _source;
            ImageSource _refinedSource;

            public Func<CancellationToken, Task<Stream>> Stream { get; private set; }
            public string Path { get; private set; }
            public FFImageLoading.Work.ImageSource SourceType { get; private set; }

            public int PreviewResolution { get; set; } = 200;
            public int RefinedResolution { get; set; } = 1280;

            protected override void SetupOnBeforeImageLoading(TaskParameter imageLoader)
            {
                Stream = imageLoader.Stream;
                Path = imageLoader.Path;
                SourceType = imageLoader.Source;

                imageLoader.CacheKey(_isRefined ? _refinedCacheKey : _cacheKey);

                base.SetupOnBeforeImageLoading(imageLoader);
            }

            public async void SetSource(ImageSource source)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(_cacheKey))
                        await ImageService.Instance.InvalidateCacheEntryAsync(_cacheKey, FFImageLoading.Cache.CacheType.Memory, true);

                    if (!string.IsNullOrWhiteSpace(_refinedCacheKey))
                        await ImageService.Instance.InvalidateCacheEntryAsync(_cacheKey, FFImageLoading.Cache.CacheType.Memory, true);

                    _imageGuid = Guid.NewGuid();
                    _cacheKey = _imageGuid.ToString();
                    _refinedCacheKey = $"{_imageGuid.ToString()}-Refined";
                    TaskParameter task = null;
                    TaskParameter taskRefined = null;

                    var fileSource = source as FileImageSource;
                    if (fileSource != null)
                    {
                        task = ImageService.Instance.LoadFile(fileSource.File);
                        taskRefined = ImageService.Instance.LoadFile(fileSource.File);
                    }

                    var urlSource = source as UriImageSource;
                    if (urlSource != null)
                    {
                        task = ImageService.Instance.LoadUrl(urlSource.Uri?.OriginalString);
                        taskRefined = ImageService.Instance.LoadUrl(urlSource.Uri?.OriginalString);
                    }

                    var streamSource = source as StreamImageSource;
                    if (streamSource != null)
                    {
                        task = ImageService.Instance.LoadStream(streamSource.Stream);
                        taskRefined = ImageService.Instance.LoadStream(streamSource.Stream);
                    }

                    using (var stream = await task.DownSample(PreviewResolution, PreviewResolution).AsJPGStreamAsync(90))
                    {
                        byte[] bytes = StreamToByteArray(stream);
                        _source = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }

                    using (var streamRefined = await taskRefined.DownSample(RefinedResolution, RefinedResolution).AsJPGStreamAsync(90))
                    {
                        byte[] bytes = StreamToByteArray(streamRefined);
                        _refinedSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }

                    LoadRefinedImage();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }

            byte[] StreamToByteArray(Stream stream)
            {
                var ms = stream as MemoryStream;

                if (ms == null)
                {
                    using (var ms1 = new MemoryStream())
                    {
                        stream.CopyTo(ms1);
                        return ms1.ToArray();
                    }
                }

                return ms.ToArray();
            }

            public void LoadImage()
            {
                _isRefined = false;

                if (Source == _source)
                    ReloadImage();
                else
                    Source = _source;
            }

            public void LoadRefinedImage()
            {
                _isRefined = true;

                if (Source == _refinedSource)
                    ReloadImage();
                else
                    Source = _refinedSource;
            }
        }

        //class CustomFrame : Frame
        //{
        //    public CustomFrame()
        //    {
        //        HasShadow = false;
        //        Margin = 50d;
        //        OutlineColor = Color.White;
        //        BackgroundColor = Color.Transparent;
        //        InputTransparent = true;
        //        HorizontalOptions = LayoutOptions.FillAndExpand;
        //        VerticalOptions = LayoutOptions.FillAndExpand;
        //    }
        //}
    }
}
