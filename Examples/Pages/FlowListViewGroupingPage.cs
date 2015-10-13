using System;

using Xamarin.Forms;
using DLToolkit.PageFactory;
using Examples.ViewModels;
using DLToolkit.Forms.Controls;
using System.Collections.Generic;
using Examples.Models;

namespace Examples.Pages
{
	public class FlowListViewGroupingPage : PFContentPage<FlowListViewGroupingViewModel>
	{
		public FlowListViewGroupingPage()
		{
			Title = "FlowListView Grouping Example";

			var flowListView = new FlowListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				FlowColumnsDefinitions = new List<Func<object, Type>>() {
					new Func<object, Type>((bindingContext) => typeof(FlowGroupingExampleViewCell)),
					new Func<object, Type>((bindingContext) => typeof(FlowGroupingExampleViewCell)),
					new Func<object, Type>((bindingContext) => typeof(FlowGroupingExampleViewCell)),
				},

				IsGroupingEnabled = true,

				FlowGroupKeySelector = new Func<object, object>((item) => ((FlowItem)item).TitleGroupSelector),
				FlowGroupKeySorting = FlowGroupSorting.Ascending,

				FlowGroupItemSortingSelector = new Func<object, object>(item => ((FlowItem)item).Title),
				FlowGroupItemSorting = FlowGroupSorting.Ascending,

				GroupDisplayBinding = new Binding("Key"),
			};

			flowListView.SetBinding<FlowListViewGroupingViewModel>(FlowListView.FlowItemsSourceProperty, v => v.Items);
			flowListView.ItemSelected += (sender, e) => { flowListView.SelectedItem = null; };

			flowListView.FlowItemTapped += (sender, e) => {
				var item = e.Item as FlowItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("FlowListView tapped: {0}", item.Title);
			};

			var button1 = new Button() {
				Text = "Remove few first collection items",
				Command = ViewModel.ModifyCollectionCommand
			};

			var button2 = new Button() {
				Text = "Modify collection items",
				Command = ViewModel.ModifyCollectionItemsCommand
			};

			Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					flowListView,
					button1,
					button2,
				}
			};
		}

		class FlowGroupingExampleViewCell : FlowStackCell
		{
			public FlowGroupingExampleViewCell()
			{
				var box = new BoxView() {
					Color = Color.Accent
				};

				var label = new Label() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					XAlign = TextAlignment.End,
					YAlign = TextAlignment.Center,
				};
				label.SetBinding<FlowItem>(Label.TextProperty, v => v.Title);

				Orientation = StackOrientation.Horizontal;
				Children.Add(box);
				Children.Add(label);
			}
		}
	}
}


