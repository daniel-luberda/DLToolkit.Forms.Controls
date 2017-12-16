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
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

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

        readonly Grid _root;
        readonly CustomCachedImage _image;
        readonly CropTransformation _crop;
        readonly PinchGestureRecognizer _pinchGesture;
        readonly PanGestureRecognizer _panGesture;

        View _frame;
        const double MIN_SCALE = 1;
        IntervalThrottle _intervalThrottle;

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
                FadeAnimationEnabled = false,
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

            _intervalThrottle = new IntervalThrottle(Delay,
                () => _image.LoadImage(), () => _image.LoadImage(), () => _image.LoadRefinedImage());

            _pinchGesture = new PinchGestureRecognizer();
            _pinchGesture.PinchUpdated += PinchGesture_PinchUpdated; ;

            _panGesture = new PanGestureRecognizer();
            _panGesture.PanUpdated += PanGesture_PanUpdated;

            GestureRecognizers.Clear();
            GestureRecognizers.Add(_pinchGesture);
            GestureRecognizers.Add(_panGesture);

            HandlePreviewTransformations(null, PreviewTransformations);
            Content = _root;
            ResetCrop();
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

        public static readonly BindableProperty PanSpeedProperty = BindableProperty.Create(nameof(PanSpeed), typeof(double), typeof(ImageCropView), 1d);

        public double PanSpeed
        {
            get { return (double)GetValue(PanSpeedProperty); }
            set { SetValue(PanSpeedProperty, value); }
        }


        public static readonly BindableProperty ZoomSpeedProperty = BindableProperty.Create(nameof(ZoomSpeed), typeof(double), typeof(ImageCropView), 1d);

        public double ZoomSpeed
        {
            get { return (double)GetValue(ZoomSpeedProperty); }
            set { SetValue(ZoomSpeedProperty, value); }
        }


        public static readonly BindableProperty ManualZoomProperty = BindableProperty.Create(nameof(ManualZoom), typeof(double), typeof(ImageCropView), 1d);

        public double ManualZoom
        {
            get { return (double)GetValue(ManualZoomProperty); }
            set { SetValue(ManualZoomProperty, value); }
        }


        public static readonly BindableProperty ManualOffsetXProperty = BindableProperty.Create(nameof(ManualOffsetX), typeof(double), typeof(ImageCropView), default(double));

        public double ManualOffsetX
        {
            get { return (double)GetValue(ManualOffsetXProperty); }
            set { SetValue(ManualOffsetXProperty, value); }
        }


        public static readonly BindableProperty ManualOffsetYProperty = BindableProperty.Create(nameof(ManualOffsetY), typeof(double), typeof(ImageCropView), default(double));

        public double ManualOffsetY
        {
            get { return (double)GetValue(ManualOffsetYProperty); }
            set { SetValue(ManualOffsetYProperty, value); }
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


        public static readonly BindableProperty TouchGesturesEnabledProperty = BindableProperty.Create(nameof(TouchGesturesEnabled), typeof(bool), typeof(ImageCropView), true);

        public bool TouchGesturesEnabled
        {
            get { return (bool)GetValue(TouchGesturesEnabledProperty); }
            set { SetValue(TouchGesturesEnabledProperty, value); }
        }


        public static readonly BindableProperty ImageRotationProperty = BindableProperty.Create(nameof(ImageRotation), typeof(int), typeof(ImageCropView), default(int));

        public int ImageRotation
        {
            get { return (int)GetValue(ImageRotationProperty); }
            set { SetValue(ImageRotationProperty, value); }
        }

        void PinchGesture_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Running:
                    double current =  (e.Scale - 1) / 2 * ZoomSpeed;
                    _crop.ZoomFactor = Clamp(_crop.ZoomFactor + current, MIN_SCALE, MaxZoom);

                    _intervalThrottle.Handle();
                    break;
            }
        }

        void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    var xOffset = e.TotalX / _crop.ZoomFactor / 50 * -1 * PanSpeed / _crop.CropWidthRatio;
                    var yOffset = e.TotalY / _crop.ZoomFactor / 50 * -1 * PanSpeed / _crop.CropHeightRatio;
                    _crop.XOffset = Clamp(_crop.XOffset + xOffset, -Width / 2 / _crop.ZoomFactor, Width / 2 / _crop.ZoomFactor);
                    _crop.YOffset = Clamp(_crop.YOffset + yOffset, -Height / 2 / _crop.ZoomFactor, Height / 2 / _crop.ZoomFactor);

                    _intervalThrottle.Handle();
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
            var currentTransformations = PreviewTransformations?.ToList() ?? new List<ITransformation>();
            currentTransformations.Insert(0, _crop);
            _image.Transformations = currentTransformations;
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
                _intervalThrottle.Delay = Delay;
            }
            else if (propertyName == PreviewMaxResolutionProperty.PropertyName)
            {
                _image.PreviewResolution = PreviewMaxResolution;
            }
            else if (propertyName == RefinedMaxResolutionProperty.PropertyName)
            {
                _image.RefinedResolution = RefinedMaxResolution;
            }
            else if (propertyName == TouchGesturesEnabledProperty.PropertyName)
            {
                GestureRecognizers.Clear();

                if (TouchGesturesEnabled)
                {
                    GestureRecognizers.Add(_pinchGesture);
                    GestureRecognizers.Add(_panGesture);
                }
            }
            else if (propertyName == ManualZoomProperty.PropertyName)
            {
                _crop.ZoomFactor = Clamp(ManualZoom, MIN_SCALE, MaxZoom);
                _intervalThrottle.Handle();
            }
            else if (propertyName == ManualOffsetXProperty.PropertyName)
            {
                _crop.XOffset = Clamp(ManualOffsetX / _crop.CropWidthRatio, -Width / 2 / _crop.ZoomFactor, Width / 2 / _crop.ZoomFactor);
                _intervalThrottle.Handle();
            }
            else if (propertyName == ManualOffsetYProperty.PropertyName)
            {
                _crop.YOffset = Clamp(ManualOffsetY / _crop.CropHeightRatio, -Height / 2 / _crop.ZoomFactor, Height / 2 / _crop.ZoomFactor);
                _intervalThrottle.Handle();
            }
            else if (propertyName == ImageRotationProperty.PropertyName)
            {
                ResetCrop();
                _image.ImageRotation = ImageRotation;
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

        /// <summary>
        /// Gets the image as JPEG stream.
        /// </summary>
        /// <returns>The image as JPEG async.</returns>
        /// <param name="quality">Quality.</param>
        /// <param name="maxWidth">Max width.</param>
        /// <param name="maxHeight">Max height.</param>
        /// <param name="framePadding">Frame padding.</param>
        public Task<Stream> GetImageAsJpegAsync(int quality = 90, int maxWidth = 0, int maxHeight = 0, double framePadding = 0d)
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

            var applied = (1 + (2 * (framePadding / _crop.CropHeightRatio)));

            var transformations = (Transformations?.ToList() ?? new List<ITransformation>());
            transformations.Insert(0, new CropTransformation()
            {
                XOffset = _crop.XOffset,
                YOffset = _crop.YOffset,
                CropHeightRatio = _crop.CropHeightRatio,
                CropWidthRatio = _crop.CropWidthRatio,
                ZoomFactor = _crop.ZoomFactor * applied,
            });

            if (ImageRotation != 0)
                transformations.Insert(0, new RotateTransformation(Math.Abs(ImageRotation), ImageRotation < 0) { Resize = true });

            return task
                .WithCache(FFImageLoading.Cache.CacheType.Disk)
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
            readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

            string _cacheKey;
            string _refinedCacheKey;
            bool _isRefined;
            ImageSource _source;
            ImageSource _refinedSource;
            ImageSource _originalSource;

            public Func<CancellationToken, Task<Stream>> Stream { get; private set; }
            public string Path { get; private set; }
            public FFImageLoading.Work.ImageSource SourceType { get; private set; }

            int _previewResolution = 200;
            public int PreviewResolution { get { return _previewResolution; } set { _previewResolution = value; SetSource(_originalSource); } }

            int _refinedResolution = 1024;
            public int RefinedResolution { get { return _refinedResolution; } set { _refinedResolution = value; SetSource(_originalSource);} }

            int _rotation = 0;
            public int ImageRotation { get { return _rotation; } set { _rotation = value; SetSource(_originalSource); } }

            protected override void SetupOnBeforeImageLoading(TaskParameter imageLoader)
            {
                imageLoader.CacheKey(_isRefined ? _refinedCacheKey : _cacheKey);

                base.SetupOnBeforeImageLoading(imageLoader);
            }

            public async void SetSource(ImageSource source)
            {
                try
                {
                    await _lock.WaitAsync();

                    if (!string.IsNullOrWhiteSpace(_cacheKey))
                        await ImageService.Instance.InvalidateCacheEntryAsync(_cacheKey, FFImageLoading.Cache.CacheType.Memory, true);

                    if (!string.IsNullOrWhiteSpace(_refinedCacheKey))
                        await ImageService.Instance.InvalidateCacheEntryAsync(_cacheKey, FFImageLoading.Cache.CacheType.Memory, true);
                    
                    if (source == null)
                    {
                        _cacheKey = null;
                        _refinedCacheKey = null;
                        _source = null;
                        _refinedSource = null;
                        _originalSource = null;
                        Source = null;   
                        return;
                    }

                    var imageGuid = Guid.NewGuid();
                    _cacheKey = imageGuid.ToString();
                    _refinedCacheKey = $"{imageGuid.ToString()}-Refined";
                    _originalSource = source;
                    TaskParameter task = null;
                    TaskParameter taskRefined = null;

                    var fileSource = source as FileImageSource;
                    if (fileSource != null)
                    {
                        task = ImageService.Instance.LoadFile(fileSource.File);
                        taskRefined = ImageService.Instance.LoadFile(fileSource.File);
                        Stream = null;
                        Path = fileSource.File;
                        SourceType = FFImageLoading.Work.ImageSource.Filepath;
                    }

                    var urlSource = source as UriImageSource;
                    if (urlSource != null)
                    {
                        task = ImageService.Instance.LoadUrl(urlSource.Uri?.OriginalString);
                        taskRefined = ImageService.Instance.LoadUrl(urlSource.Uri?.OriginalString);
                        Stream = null;
                        Path = urlSource.Uri?.OriginalString;
                        SourceType = FFImageLoading.Work.ImageSource.Url;
                    }

                    var streamSource = source as StreamImageSource;
                    if (streamSource != null)
                    {
                        task = ImageService.Instance.LoadStream(streamSource.Stream);
                        taskRefined = ImageService.Instance.LoadStream(streamSource.Stream);
                        Stream = streamSource.Stream;
                        Path = null;
                        SourceType = FFImageLoading.Work.ImageSource.Stream;
                    }

                    if (ImageRotation != 0)
                    {
                        var rotateTransformation = new RotateTransformation(Math.Abs(ImageRotation), ImageRotation < 0) { Resize = true };

                        task.Transform(rotateTransformation);
                        taskRefined.Transform(rotateTransformation);
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
                finally
                {
                    _lock.Release();
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
