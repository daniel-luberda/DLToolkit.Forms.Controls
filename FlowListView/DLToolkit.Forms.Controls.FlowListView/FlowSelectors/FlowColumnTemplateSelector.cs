using System;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// Flow column template selector.
	/// </summary>
	public abstract class FlowColumnTemplateSelector
	{
		public abstract Type GetColumnType(object bindingContext);
	}
}

