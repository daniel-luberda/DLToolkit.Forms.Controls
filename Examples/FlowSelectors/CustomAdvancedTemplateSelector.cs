using System;
using DLToolkit.Forms.Controls;
using Examples.Views;
using Xamarin.Forms;

namespace Examples.FlowSelectors
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

