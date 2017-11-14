using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace DLToolkit.Forms.Controls
{
    internal class RecyclerViewCell : UICollectionViewCell
    {
        UIView _view;
        object _originalBindingContext;
        CGSize _lastSize;


        [Export("initWithFrame:")]
        public RecyclerViewCell(CGRect frame) : base(frame)
        {
            
        }

        public const string Key = nameof(RecyclerViewCell);

        ViewCell _viewCell;
        public ViewCell ViewCell { get { return _viewCell; } }

        public void RecycleCell(object data, DataTemplate dataTemplate, VisualElement parent)
        {
            if (_viewCell == null)
            {
                _viewCell = (dataTemplate.CreateContent() as ViewCell);
                _viewCell.BindingContext = data;
                _viewCell.Parent = parent;
                _originalBindingContext = _viewCell.BindingContext;
                var renderer = Platform.CreateRenderer(_viewCell.View);
                _view = renderer.NativeView;

                _view.AutoresizingMask = UIViewAutoresizing.All;
                _view.ContentMode = UIViewContentMode.ScaleToFill;

                ContentView.AddSubview(_view);
            }
            else if (data == _originalBindingContext)
            {
                _viewCell.BindingContext = _originalBindingContext;
            }
            else
            {
                _viewCell.BindingContext = data;
            }
        }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                return base.IntrinsicContentSize;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_lastSize.Equals(CGSize.Empty) || !_lastSize.Equals(Frame.Size))
            {
                _viewCell.View.Layout(Frame.ToRectangle());
                var result = _viewCell.View.Measure(-1, -1, MeasureFlags.IncludeMargins);
                //ContentView.Bounds = new CGRect(ContentView.Bounds.X, ContentView.Bounds.Y, result.Minimum.Width, 200);
                //ContentView.Frame = new CGRect(ContentView.Frame.X, ContentView.Frame.Y, result.Minimum.Width, 200);
                //_viewCell.View.Layout(new Rectangle(0, 0, result.Minimum.Width, 200));
                _lastSize = Frame.Size;
            }

            _view.Frame = ContentView.Bounds;
        }
    }
}
