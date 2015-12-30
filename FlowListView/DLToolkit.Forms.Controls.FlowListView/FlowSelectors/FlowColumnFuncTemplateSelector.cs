using System;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// Flow column func template selector.
	/// </summary>
	internal class FlowColumnFuncTemplateSelector : FlowColumnTemplateSelector
	{
		readonly Func<object, Type> selector;

		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Forms.Controls.FlowColumnFuncTemplateSelector"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		public FlowColumnFuncTemplateSelector(Func<object, Type> selector)
		{	
			this.selector = selector;
		}
			
		/// <summary>
		/// Gets the type of the column.
		/// </summary>
		/// <returns>The column type.</returns>
		/// <param name="bindingContext">Binding context.</param>
		public override Type GetColumnType(object bindingContext)
		{
			return selector(bindingContext);
		}
	}
}

