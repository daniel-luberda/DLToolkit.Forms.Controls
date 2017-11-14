using System;
using UIKit;

namespace DLToolkit.Forms.Controls
{
    internal class RecyclerUICollectionViewLayout : UICollectionViewFlowLayout
    {
        public RecyclerUICollectionViewLayout()
        {
            MinimumInteritemSpacing = nfloat.MaxValue;
            MinimumLineSpacing = default(nfloat);
            ScrollDirection = UICollectionViewScrollDirection.Horizontal;
        }
    }
}
