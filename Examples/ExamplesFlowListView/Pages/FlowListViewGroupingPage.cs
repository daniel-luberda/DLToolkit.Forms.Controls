using System;
using Xamarin.Forms;
using DLToolkit.PageFactory;
using Examples.ExamplesFlowListView.PageModels;
using DLToolkit.Forms.Controls;
using System.Collections.Generic;
using Examples.ExamplesFlowListView.Models;
using Examples.ExamplesFlowListView.FlowSelectors;

namespace Examples.ExamplesFlowListView.Pages
{
    public class FlowListViewGroupingPage : ContentPage, IBasePage<FlowListViewGroupingPageModel>
	{
		public FlowListViewGroupingPage()
		{
			Title = "FlowListView Grouping Example";

			var flowListView = new FlowListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None,

				FlowColumnsTemplates = new List<FlowColumnTemplateSelector>() {
					new FlowColumnSimpleTemplateSelector() { ViewType = typeof(FlowGroupingExampleViewCell) },
					new FlowColumnSimpleTemplateSelector() { ViewType = typeof(FlowGroupingExampleViewCell) },
					new CustomTemplateSelector(), // custom selector basing on bindingContext
				},
					
				IsGroupingEnabled = true,
				FlowGroupKeySorting = FlowSorting.Ascending,
				FlowGroupItemSorting = FlowSorting.Ascending,
				FlowGroupGroupingKeySelector = new CustomGroupKeySelector(),
				FlowGroupItemSortingKeySelector = new CustomItemSortingKeySelector(),
			};

			flowListView.SetBinding<FlowListViewGroupingPageModel>(FlowListView.FlowItemsSourceProperty, v => v.Items);

			flowListView.FlowItemTapped += (sender, e) => {
				var item = e.Item as FlowItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("FlowListView tapped: {0}", item.Title);
			};

			var button1 = new Button() {
				Text = "Remove few first collection items",
			};
            button1.SetBinding<FlowListViewGroupingPageModel>(Button.CommandProperty, v => v.ModifyCollectionCommand);

			var button2 = new Button() {
				Text = "Modify collection items",
			};
            button1.SetBinding<FlowListViewGroupingPageModel>(Button.CommandProperty, v => v.ModifyCollectionItemsCommand);

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

		class CustomTemplateSelector : FlowColumnTemplateSelector
		{
			public override Type GetColumnType(object bindingContext)
			{
				// YOUR CUSTOM LOGIC HERE

				return typeof(FlowGroupingExampleViewCell);
			}
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
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
				};
				label.SetBinding<FlowItem>(Label.TextProperty, v => v.Title);

				Orientation = StackOrientation.Horizontal;
				Children.Add(box);
				Children.Add(label);
			}
		}
	}
}


