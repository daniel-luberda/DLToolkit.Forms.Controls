using System;
using DLToolkit.Forms.Controls;
using Examples.ExamplesFlowListView.Models;

namespace Examples.ExamplesFlowListView.FlowSelectors
{
	public class CustomGroupKeySelector : FlowPropertySelector
	{
		public override object GetProperty(object bindingContext)
		{
			// YOUR CUSTOM LOGIC HERE

			var flowItem = (FlowItem)bindingContext;
			return flowItem.TitleGroupSelector;
		}
	}
}

