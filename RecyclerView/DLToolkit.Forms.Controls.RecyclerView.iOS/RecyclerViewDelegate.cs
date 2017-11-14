using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;

namespace DLToolkit.Forms.Controls
{
    internal class RecyclerViewDelegate : UICollectionViewDelegateFlowLayout
    {
        readonly OnItemSelected _onItemSelected;
        readonly OnScrolled _onScrolled;
        readonly RecyclerDataSource _dataSource;

        public delegate void OnItemSelected(UICollectionView tableView, NSIndexPath indexPath);
        public delegate void OnScrolled(CGPoint contentOffset);

        public RecyclerViewDelegate(OnItemSelected onItemSelected, OnScrolled onScrolled, RecyclerDataSource dataSource)
        {
            _dataSource = dataSource;
            _onItemSelected = onItemSelected;
            _onScrolled = onScrolled;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            _onItemSelected(collectionView, indexPath);
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            _onScrolled(scrollView.ContentOffset);
        }

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return new CGSize(200, 100);
        }
    }
}
