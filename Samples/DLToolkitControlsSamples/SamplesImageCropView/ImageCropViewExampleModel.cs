using System;
using Xamvvm;
using Xamarin.Forms;
using System.Collections.Generic;
using FFImageLoading.Work;
using ImageSource = Xamarin.Forms.ImageSource;
using FFImageLoading.Transformations;
using System.Windows.Input;

namespace DLToolkitControlsSamples.SamplesImageCropView
{
    public class ImageCropViewExampleModel : BasePageModel
    {
        public ImageCropViewExampleModel()
        {
            PreviewTransformations = new List<ITransformation>() { new CircleTransformation() };

            RotateCommand = new BaseCommand((arg) =>
            {
                var rotation = Rotation + 90;

                if (rotation >= 360)
                    rotation = 0;

                Rotation = rotation;
            });

            Zoom = 1d;
        }

        public ImageSource SavedImage
        {
            get { return GetField<ImageSource>(); }
            set { SetField(value); }
        }

        public List<ITransformation> PreviewTransformations
        {
            get { return GetField<List<ITransformation>>(); }
            set { SetField(value); }
        }

        public List<ITransformation> Transformations
        {
            get { return GetField<List<ITransformation>>(); }
            set { SetField(value); }
        }

        public ICommand RotateCommand
        {
            get { return GetField<ICommand>(); }
            set { SetField(value); }
        }

        public ICommand ManualOffsetCommand
        {
            get { return GetField<ICommand>(); }
            set { SetField(value); }
        }

        public int Rotation
        {
            get { return GetField<int>(); }
            set { SetField(value); }
        }

        public double XOffset
        {
            get { return GetField<double>(); }
            set { SetField(value); }
        }

        public double YOffset
        {
            get { return GetField<double>(); }
            set { SetField(value); }
        }

        public double Zoom
        {
            get { return GetField<double>(); }
            set { SetField(value); }
        }
    }
}
