using System;

namespace DLToolkit.Forms.Controls
{
	internal class FlowFuncPropertySelector : FlowPropertySelector
	{
		public FlowFuncPropertySelector(Func<object, object> selector)
		{
			Selector = selector;
		}

		public override object GetProperty(object bindingContext)
		{
			return Selector(bindingContext);
		}

		internal Func<object, object> Selector { get; private set; }
	}
}

