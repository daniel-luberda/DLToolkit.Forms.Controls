using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;
using Foundation;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using DLToolkit.Forms.Controls;

[assembly: ExportRenderer(typeof(RecyclerView), typeof(RecyclerViewRenderer))]
namespace DLToolkit.Forms.Controls
{
    public class RecyclerViewRenderer : ViewRenderer<RecyclerView, UICollectionView>
    {
        public new static void Init()
        {
            var ignore1 = typeof(RecyclerView);
            var ignore2 = typeof(RecyclerViewRenderer);
        }

        RecyclerCollectionView _control;
        RecyclerDataSource _dataSource;
        CGRect _rect;
        int? _initialIndex;
        NSString _cellId;

        RecyclerDataSource DataSource
        {
            get => _dataSource ?? (_dataSource = new RecyclerDataSource(GetCell, RowsInSection, ItemSelected));
        }

        void SetNativeView()
        {
            if (Element == null)
                return;

            _rect = new CGRect((nfloat)Element.X, (nfloat)Element.Y, (nfloat)Element.Bounds.Width, (nfloat)Element.Bounds.Height);
            _control = new RecyclerCollectionView(_rect)
            {
                AllowsSelection = false,
                AllowsMultipleSelection = false,
            };

            _control.DataSource = (Element.ItemsSource != null) ? DataSource : null;
            _control.Delegate = new RecyclerViewDelegate(ItemSelected, HandleOnScrolled, DataSource);
            _control.BackgroundColor = Element.BackgroundColor.ToUIColor();

            SetNativeControl(_control);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<RecyclerView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control == null)
                SetNativeView();

            if (e.NewElement != null)
            {
                e.NewElement.PropertyChanging += ElementPropertyChanging;
                e.NewElement.PropertyChanged += ElementPropertyChanged;
                // e.NewElement.SizeChanged += ElementSizeChanged;
                if (e.NewElement.ItemsSource is INotifyCollectionChanged)
                {
                    (e.NewElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
                }

                ScrollToInitialIndex();
            }
            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanging -= ElementPropertyChanging;
                e.OldElement.PropertyChanged -= ElementPropertyChanged;
                // e.OldElement.SizeChanged -= ElementSizeChanged;
                var itemsSource = e.OldElement.ItemsSource as INotifyCollectionChanged;
                if (itemsSource != null)
                {
                    itemsSource.CollectionChanged -= DataCollectionChanged;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RecyclerView.ItemsSourceProperty.PropertyName)
            {
                if (Element.ItemsSource != null)
                {
                    _control.DataSource = DataSource;
                    ReloadData();
                }
                else
                {
                    _control.DataSource = null;
                }

                ScrollToInitialIndex();
            }
        }

        void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == RecyclerView.ItemsSourceProperty.PropertyName)
            {
                var itemsSource = Element != null ? Element.ItemsSource as INotifyCollectionChanged : null;
                if (itemsSource != null)
                {
                    itemsSource.CollectionChanged -= DataCollectionChanged;
                }
            }
        }

        void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == RecyclerView.ItemsSourceProperty.PropertyName)
            {
                var itemsSource = Element != null ? Element.ItemsSource as INotifyCollectionChanged : null;
                if (itemsSource != null)
                {
                    itemsSource.CollectionChanged += DataCollectionChanged;
                }
            }
        }

        void ScrollToInitialIndex()
        {
            if (_initialIndex.HasValue && _control?.DataSource != null)
            {
                ScrollToItemWithIndex(_initialIndex.Value, false);
                _initialIndex = null;
            }
        }

        void ScrollToItemWithIndex(int index, bool animated)
        {
            if (_control != null)
            {
                var indexPath = NSIndexPath.FromIndex((nuint)index);
                InvokeOnMainThread(() =>
                {
                    _control.ScrollToItem(indexPath, UICollectionViewScrollPosition.Top, animated);
                });
            }
            else
            {
                _initialIndex = index;
            }
        }

        void DataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Control != null)
            {
                ReloadData();
            }
        }

        void HandleOnScrolled(CGPoint contentOffset)
        {
            Element.InvokeScrolledEvent(contentOffset.X, contentOffset.Y);
        }

        public void ItemSelected(UICollectionView view, NSIndexPath indexPath)
        {
            object item = Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);;
            Element.InvokeItemTappedEvent(this, item);
        }

        int RowsInSection(UICollectionView collectionView, nint section)
        {
            var items = Element?.ItemsSource as ICollection;
            return items == null ? 0 : items.Count;
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            _cellId = _cellId ?? new NSString(RecyclerViewCell.Key);

            //Get the sections.
            var sections = Element.ItemsSource.OfType<IEnumerable>().ToList();
            object item = Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);

            var collectionCell = collectionView.DequeueReusableCell(_cellId, indexPath) as RecyclerViewCell;
            collectionCell.RecycleCell(item, Element.ItemTemplate, Element);

            return collectionCell;
        }

        public void ReloadData()
        {
            if (_control != null)
            {
                InvokeOnMainThread(() =>
                {
                    _control.ReloadData();
                    _control.Delegate = new RecyclerViewDelegate(ItemSelected, HandleOnScrolled, DataSource);
                });
            }
        }
    }
}
