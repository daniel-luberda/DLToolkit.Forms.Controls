using System;

using Xamarin.Forms;
using DLToolkit.PageFactory;
using Examples.ExamplesFlowListView.ViewModels;
using DLToolkit.Forms.Controls;
using System.Collections.Generic;

namespace Examples.ExamplesFlowListView.Pages
{
	public class MultipleTemplatesPage : PFContentPage<MultipleTemplatesViewModel>
	{
		public MultipleTemplatesPage()
		{
			Title = "FlowListView Multiple templates Example";

			var flowListView = new FlowListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None,

				FlowColumnsTemplates = new List<FlowColumnTemplateSelector>() {
					// First column definition:
					new FlowColumnSimpleTemplateSelector() { ViewType = typeof(FirstTemplate) }, 

					// Second column definition:
					new FlowColumnSimpleTemplateSelector() { ViewType = typeof(SecondTemplate) },

					// Third column definition:
					new FlowColumnSimpleTemplateSelector() { ViewType = typeof(ThirdTemplate) },
				},
			};

			// BINDINGS:

			// FlowListView FlowItemsSource:
			flowListView.SetBinding<MultipleTemplatesViewModel>(FlowListView.FlowItemsSourceProperty, v => v.Items);

			flowListView.SetBinding<MultipleTemplatesViewModel>(FlowListView.FlowLastTappedItemProperty, v => v.LastTappedItem);
			flowListView.SetBinding<MultipleTemplatesViewModel>(FlowListView.FlowItemTappedCommandProperty, v => v.ItemTappedCommand);

			Content = flowListView;
		}

		class FirstTemplate : FlowStackCell
		{
			public FirstTemplate()
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
				label.SetBinding<MultipleTemplatesViewModel.SimpleItem>(Label.TextProperty, v => v.Title);

				Orientation = StackOrientation.Horizontal;
				Children.Add(box);
				Children.Add(label);
			}
		}

		class SecondTemplate : FlowViewCell
		{
			public SecondTemplate()
			{
				var label = new Label() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					XAlign = TextAlignment.Center,
					YAlign = TextAlignment.Center,
				};
				label.SetBinding<MultipleTemplatesViewModel.SimpleItem>(Label.TextProperty, v => v.Title);

				Content = label;
			}
		}

		class ThirdTemplate : FlowGridCell
		{
			public ThirdTemplate()
			{
				RowDefinitions = new RowDefinitionCollection() {
					new RowDefinition() { Height = new GridLength(1d, GridUnitType.Star) }	
				};

				ColumnDefinitions = new ColumnDefinitionCollection() {
					new ColumnDefinition() { Width = new GridLength(1d, GridUnitType.Star) },
					new ColumnDefinition() { Width = GridLength.Auto }
				};

				var box = new BoxView() {
					Color = Color.Gray
				};

				var label = new Label() {
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					XAlign = TextAlignment.Center,
					YAlign = TextAlignment.End,
				};
				label.SetBinding<MultipleTemplatesViewModel.SimpleItem>(Label.TextProperty, v => v.Title);

				Children.Add(label, 0, 0);
				Children.Add(box, 1, 0);
			}
		}
	}
}


