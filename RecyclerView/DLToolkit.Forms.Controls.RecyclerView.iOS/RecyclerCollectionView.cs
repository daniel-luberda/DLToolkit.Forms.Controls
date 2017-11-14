using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace DLToolkit.Forms.Controls
{
    internal class RecyclerCollectionView : UICollectionView
    {
        readonly RecyclerUICollectionViewLayout _layout;

        public RecyclerCollectionView() : this (default(CGRect))
        {
        }

        public RecyclerCollectionView(CGRect frm) : this (default(CGRect), new RecyclerUICollectionViewLayout())
        {
        }

        public RecyclerCollectionView(CGRect frm, RecyclerUICollectionViewLayout layout): base(default(CGRect), layout)
        {
            _layout = layout;
            AutoresizingMask = UIViewAutoresizing.All;
            ContentMode = UIViewContentMode.ScaleToFill;
            SetOrientation(RecyclerViewOrientation.Horizontal);
            RegisterClassForCell(typeof(RecyclerViewCell), new NSString(RecyclerViewCell.Key));
        }

        public void SetOrientation(RecyclerViewOrientation orientation)
        {
            AlwaysBounceVertical = orientation == RecyclerViewOrientation.Vertical;
            AlwaysBounceHorizontal = orientation == RecyclerViewOrientation.Horizontal;

            switch (orientation)
            {
                case RecyclerViewOrientation.Horizontal:
                    _layout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
                    break;
                case RecyclerViewOrientation.Vertical:
                    _layout.ScrollDirection = UICollectionViewScrollDirection.Vertical;
                    break;
            }
        }

        public override UICollectionViewCell CellForItem(NSIndexPath indexPath)
        {
            if (indexPath == null)
            {
                return null;
            }
            return base.CellForItem(indexPath);
        }

        public override void Draw(CGRect rect)
        {
            CollectionViewLayout.InvalidateLayout();

            base.Draw(rect);
        }

        public CGSize ItemSize
        {
            get => _layout.ItemSize;
            set => _layout.ItemSize = value;
        }
    }
}
