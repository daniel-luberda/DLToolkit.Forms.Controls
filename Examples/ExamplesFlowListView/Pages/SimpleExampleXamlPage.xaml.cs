using System;
using Xamarin.Forms;
using DLToolkit.PageFactory;
using Examples.ExamplesFlowListView.PageModels;
using Xamarin.Forms.Xaml;

namespace Examples.ExamplesFlowListView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleExampleXamlPage : ContentPage, IBasePage<SimpleExampleXamlPageModel>
	{
		public SimpleExampleXamlPage()
		{
			InitializeComponent();
		}
	}
}

