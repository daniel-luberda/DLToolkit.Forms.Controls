using System.IO;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples.SamplesImageCropView
{
    public partial class ImageCropViewExample : ContentPage, IBasePage<ImageCropViewExampleModel>
    {
        public ImageCropViewExample()
        {
            InitializeComponent();

            saveButton.Command = new BaseCommand(async (arg) =>
            {
                try
                {
                    var result = await cropView.GetImageAsJpegAsync();
                    byte[] bytes = null;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        result.CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    var imageSource = ImageSource.FromStream(() =>
                    {
                        return new MemoryStream(bytes);
                    });

                    ((ImageCropViewExampleModel)BindingContext).SavedImage = imageSource;
                }
                catch (System.Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            });
        }
    }
}
