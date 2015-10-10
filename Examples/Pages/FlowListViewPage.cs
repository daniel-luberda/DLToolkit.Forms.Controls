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

			var button1 = new Button() {
				Text = "Remove few first collection items",
				Command = ViewModel.ModifyCollectionCommand
			};

			var button2 = new Button() {
				Text = "Modify collection items",
				Command = ViewModel.ModifyCollectionItemsCommand
			};

			Content = new ScrollView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new StackLayout() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Children = {
						flowListView,
						button1,
						button2,
					}
				}
			};
		}

		class FlowExampleLeftViewCell : FlowViewCell
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

				var root = new StackLayout() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal,
					Children = {
						box,
						label,
					}
				};
			
				Content = root;
			}

			public override void OnTapped()
			{
				var item = BindingContext as FlowItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("FlowExampleLeftViewCell tapped ({0})", item.Title);
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

			public override void OnTapped()
			{
				var item = BindingContext as FlowItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("FlowExampleCenterViewCell tapped ({0})", item.Title);
			}
		}

		class FlowExampleRightViewCell : FlowViewCell
		{
			public FlowExampleRightViewCell()
			{
				var box = new BoxView() {
					Color = Color.Accent
				};

				var label = new Label() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					XAlign = TextAlignment.Center,
					YAlign = TextAlignment.End,
				};
				label.SetBinding<FlowItem>(Label.TextProperty, v => v.Title);

				var root = new StackLayout() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal,
					Children = {
						label,
						box,
					}
				};

				Content = root;
			}

			public override void OnTapped()
			{
				var item = BindingContext as FlowItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("FlowExampleRightViewCell tapped ({0})", item.Title);
			}
		}
	}
}


