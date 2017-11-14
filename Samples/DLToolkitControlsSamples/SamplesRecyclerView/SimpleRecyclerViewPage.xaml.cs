using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples.SamplesRecyclerView
{
    public partial class SimpleRecyclerViewPage : ContentPage, IBasePage<SimpleRecyclerViewPageModel>
    {
        public SimpleRecyclerViewPage()
        {
            InitializeComponent();

            _recyclerView.OnItemTapped += (sender, e) =>
            {

            };
        }
    }
}
