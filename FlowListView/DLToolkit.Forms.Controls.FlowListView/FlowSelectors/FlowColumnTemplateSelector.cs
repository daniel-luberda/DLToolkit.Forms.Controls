using System;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// Flow column template selector.
	/// </summary>
	public abstract class FlowColumnTemplateSelector
	{
		/// <summary>
		/// Gets the type of the column basing on BindingContext.
		/// </summary>
		/// <returns>The column type.</returns>
		/// <param name="bindingContext">Binding context.</param>
		public abstract Type GetColumnType(object bindingContext);
	}
}

