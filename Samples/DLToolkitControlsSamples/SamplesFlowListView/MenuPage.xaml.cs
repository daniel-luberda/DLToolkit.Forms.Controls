using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLToolkit.PageFactory;
using Xamarin.Forms;

namespace DLToolkitControlsSamples.SamplesFlowListView
{
    public partial class MenuPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private async Task ButtonSimpleFlowView_OnClicked(object sender, EventArgs e)
        {
           var  page = PageFactory.Instance.GetPageFromCache<SimplePageModel>() as Page;
            await Navigation.PushAsync(page);
        }

        private async Task ButtonImageFlowView_OnClicked(object sender, EventArgs e)
        {
            var page = PageFactory.Instance.GetPageFromCache<ImagePageModel>() as Page;
            await Navigation.PushAsync(page);
        }
    }
}
