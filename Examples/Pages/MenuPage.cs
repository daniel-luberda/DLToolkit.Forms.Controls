using System;

using Xamarin.Forms;
using DLToolkit.PageFactory;
using Examples.ViewModels;

namespace Examples.Pages
{
	public class MenuPage : PFContentPage<MenuViewModel>
	{
		public MenuPage()
		{
			var buttonFlowListView = new Button() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "FlowListView Example",
				Command = ViewModel.FlowListViewDemoCommand
			};

			Content = new ScrollView() {
				Content = new StackLayout { 
					Children = {
						buttonFlowListView
					}
				}
			};
		}
	}
}


