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

		class SelectionColorConverter : IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				bool isSelected = (bool)value;

				if (isSelected)
					return Color.Accent;

				return Color.Transparent;
			}
			public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				throw new NotImplementedException();
			}
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

				// SELECTION COLOR BINDING:
				this.SetBinding<SelectionViewModel.SimpleItem>(ContentView.BackgroundColorProperty, v => v.IsSelected, 
					converter: new SelectionColorConverter());
			}
		}
	}
}


