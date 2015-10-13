using System;

using Xamarin.Forms;
using Examples.ViewModels;
using DLToolkit.PageFactory;
using DLToolkit.Forms.Controls;
using Examples.Models;
using System.Collections.Generic;

namespace Examples.Pages
{
	public class FlowListViewPage : PFContentPage<FlowListViewViewModel>
	{
		public FlowListViewPage()
		{
			Title = "FlowListView Example";

			var flowListView = new FlowListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				FlowColumnsDefinitions = new List<Func<object, Type>>() {
					new Func<object, Type>((bindingContext) => typeof(FlowExampleLeftViewCell)),
					new Func<object, Type>((bindingContext) => typeof(FlowExampleCenterViewCell)),
					new Func<object, Type>((bindingContext) => typeof(FlowExampleRightViewCell)),
				},
			};

			flowListView.SetBinding<FlowListViewViewModel>(FlowListView.FlowItemsSourceProperty, v => v.Items);
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

		class FlowExampleLeftViewCell : FlowStackCell
		{
			public FlowExampleLeftViewCell()
			{
				var box = new BoxView() {
					Color = Color.Accent
				};

				var label = new Label() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					XAlign = TextAlignment.Start,
					YAlign = TextAlignment.Center,
				};
				label.SetBinding<FlowItem>(Label.TextProperty, v => v.Title);

				Orientation = StackOrientation.Horizontal;
				Children.Add(box);
				Children.Add(label);
			}
		}

		class FlowExampleCenterViewCell : FlowViewCell
		{
			public FlowExampleCenterViewCell()
			{
				var label = new Label() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					XAlign = TextAlignment.Center,
					YAlign = TextAlignment.Center,
				};
				label.SetBinding<FlowItem>(Label.TextProperty, v => v.Title);

				Content = label;
			}
		}

		class FlowExampleRightViewCell : FlowGridCell
		{
			public FlowExampleRightViewCell()
			{
				var box = new BoxView() {
					Color = Color.Gray
				};

				var label = new Label() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					XAlign = TextAlignment.Center,
					YAlign = TextAlignment.End,
				};
				label.SetBinding<FlowItem>(Label.TextProperty, v => v.Title);

				Children.Add(label, 0, 0);
				Children.Add(box, 1, 0);
			}
		}
	}
}


