using System;
using Xamarin.Forms;
using Examples.ExamplesFlowListView.PageModels;
using DLToolkit.PageFactory;
using Examples.ExamplesFlowListView.Models;
using Xamarin.Forms.Xaml;

namespace Examples.ExamplesFlowListView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlowListViewExpandPage : ContentPage, IBasePage<FlowListViewExpandPageModel>
	{
		public FlowListViewExpandPage()
		{
			InitializeComponent();

			flowListView.FlowItemTapped += (sender, e) => {
				var item = e.Item as FlowItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("FlowListView tapped: {0}", item.Title);	
			};
		}
	}
}

