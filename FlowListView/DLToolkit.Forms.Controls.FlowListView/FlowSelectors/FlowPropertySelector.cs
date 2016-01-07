using System;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// FlowViewCell property selector.
	/// </summary>
	public abstract class FlowPropertySelector
	{
		/// <summary>
		/// Gets the property basing on BindingContext.
		/// </summary>
		/// <returns>The property.</returns>
		/// <param name="bindingContext">Binding context.</param>
		public abstract object GetProperty(object bindingContext);
	}
}

