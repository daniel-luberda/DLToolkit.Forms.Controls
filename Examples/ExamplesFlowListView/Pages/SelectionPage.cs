using System;

using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using Examples.ExamplesFlowListView.ViewModels;
using System.Collections.Generic;
using DLToolkit.PageFactory;

namespace Examples.ExamplesFlowListView.Pages
{
	public class SelectionPage : PFContentPage<SelectionViewModel>
	{
		public SelectionPage()
		{
			Title = "FlowListView Simple Example";

			var flowListView = new FlowListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None,

				FlowTappedBackgroundColor = Color.Accent,
				FlowTappedBackgroundDelay = 250,

				FlowColumnsTemplates = new List<FlowColumnTemplateSelector>() {
					// First column definition:
					new FlowColumnSimpleTemplateSelector() { ViewType = typeof(SelectionExampleView) }, 

					// Second column definition:
					new FlowColumnSimpleTemplateSelector() { ViewType = typeof(SelectionExampleView) },

					// Third column definition:
					new FlowColumnSimpleTemplateSelector() { ViewType = typeof(SelectionExampleView) },
				},
			};
			// BINDINGS:

			// FlowListView FlowItemsSource:
			flowListView.SetBinding<SelectionViewModel>(FlowListView.FlowItemsSourceProperty, v => v.Items);

			flowListView.SetBinding<SelectionViewModel>(FlowListView.FlowLastTappedItemProperty, v => v.LastTappedItem);
			flowListView.SetBinding<SelectionViewModel>(FlowListView.FlowItemTappedCommandProperty, v => v.ItemTappedCommand);

			Content = flowListView;
		}

		// View template definitions used for columns:
		class SelectionExampleView : ContentView
		{
			public SelectionExampleView()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand;
				VerticalOptions = LayoutOptions.FillAndExpand;

				var label = new Label() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					XAlign = TextAlignment.Start,
					YAlign = TextAlignment.Center,
				};

				label.SetBinding<SelectionViewModel.SimpleItem>(Label.TextProperty, v => v.Title);

				Content = label;
			}
		}
	}
}


