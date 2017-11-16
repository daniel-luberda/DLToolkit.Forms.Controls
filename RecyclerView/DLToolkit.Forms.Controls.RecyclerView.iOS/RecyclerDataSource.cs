using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace DLToolkit.Forms.Controls
{
    internal class RecyclerDataSource : UICollectionViewDataSource
    {
        readonly OnGetCell _onGetCell;
        readonly OnRowsInSection _onRowsInSection;
        readonly OnItemSelected _onItemSelected;

        public RecyclerDataSource(OnGetCell onGetCell, OnRowsInSection onRowsInSection, OnItemSelected onItemSelected)
        {
            _onGetCell = onGetCell;
            _onRowsInSection = onRowsInSection;
            _onItemSelected = onItemSelected;
        }

        public delegate UICollectionViewCell OnGetCell(UICollectionView collectionView, NSIndexPath indexPath);
        public delegate int OnRowsInSection(UICollectionView collectionView, nint section);
        public delegate void OnItemSelected(UICollectionView collectionView, NSIndexPath indexPath);

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _onRowsInSection(collectionView, section);
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return _onGetCell(collectionView, indexPath);
        }

        //public RecyclerViewCell ConfiguredCellForIndexPath(NSIndexPath index, bool prototype)
        //{
        //    //let cell = reusableCellForIndexPath(indexPath, prototype: prototype) as!CollectionViewCell
        //    //let model: AnyObject = data[indexPath.row]
        //    //cell.configure(model, prototype: prototype)
        //    //return cell
        //}
    }
}
