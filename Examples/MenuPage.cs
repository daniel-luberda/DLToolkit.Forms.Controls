using System;

using Xamarin.Forms;
using DLToolkit.PageFactory;

namespace Examples
{
	public class MenuPage : PFContentPage<MenuViewModel>
	{
		public MenuPage()
		{
			Title = "DLToolkit.Forms.Controls Demo";

			var menuListView = new ListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,	
				RowHeight = 60,
				ItemTemplate = new DataTemplate(() => {
					var cell = new TextCell();
					cell.SetBinding<MenuItem>(TextCell.TextProperty, v => v.Title);
					cell.SetBinding<MenuItem>(TextCell.DetailProperty, v => v.Detail);
					cell.SetBinding<MenuItem>(TextCell.CommandProperty, v => v.Command);
					cell.SetBinding<MenuItem>(TextCell.CommandParameterProperty, v => v.CommandParameter);
					return cell;
				}),
				IsGroupingEnabled = true,
				GroupDisplayBinding = new Binding("Key"),
			};

			if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
				menuListView.ItemSelected += (sender, e) => { menuListView.SelectedItem = null; };

			menuListView.SetBinding<MenuViewModel>(ListView.ItemsSourceProperty, v => v.MenuItems);

			Content = menuListView;
		}
	}
}


