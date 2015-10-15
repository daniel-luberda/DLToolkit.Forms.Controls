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
			Title = "DLToolkit.Forms.Controls Demo";

			var listView = new ListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate(() => {
					var cell = new TextCell();
					cell.SetBinding<MenuViewModel.MenuItem>(TextCell.TextProperty, v => v.Title);
					cell.SetBinding<MenuViewModel.MenuItem>(TextCell.DetailProperty, v => v.SubTitle);
					cell.SetBinding<MenuViewModel.MenuItem>(TextCell.CommandProperty, v => v.Command);
					return cell;
				})
			};

			listView.SetBinding<MenuViewModel>(ListView.ItemsSourceProperty, v => v.Items);
			listView.ItemSelected += (sender, e) => {
				listView.SelectedItem = null;
			};

			Content = listView;
		}
	}
}


