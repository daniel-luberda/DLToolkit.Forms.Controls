using System;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;
using Examples.ExamplesFlowListView.Views;

namespace Examples.ExamplesFlowListView.FlowSelectors
{
	public class CustomAdvancedTemplateSelector : FlowColumnTemplateSelector
	{
		public override Type GetColumnType(object bindingContext)
		{
			// YOUR CUSTOM LOGIC HERE

			if (bindingContext == null)
				return typeof(ContentView);

			return typeof(FlowListViewExpandCell);
		}
	}
}

