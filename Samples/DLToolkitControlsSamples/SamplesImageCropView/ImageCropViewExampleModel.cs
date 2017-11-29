using System;
using Xamvvm;
using Xamarin.Forms;

namespace DLToolkitControlsSamples.SamplesImageCropView
{
    public class ImageCropViewExampleModel : BasePageModel
    {
        public ImageCropViewExampleModel()
        {
        }

        public ImageSource SavedImage
        {
            get { return GetField<ImageSource>(); }
            set { SetField(value); }
        }
    }
}
