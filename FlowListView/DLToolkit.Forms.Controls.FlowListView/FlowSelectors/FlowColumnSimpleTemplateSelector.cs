using System;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// Flow column simple single view template selector.
	/// </summary>
	public class FlowColumnSimpleTemplateSelector : FlowColumnTemplateSelector
	{
		/// <summary>
		/// Gets or sets the type of the view for a columns.
		/// XAML usage ViewType="{x:Type someViewClass}"
		/// </summary>
		/// <value>The type of the view.</value>
		public Type ViewType { get; set; }

		/// <summary>
		/// Gets the type of the column.
		/// </summary>
		/// <returns>The column type.</returns>
		/// <param name="bindingContext">Binding context.</param>
		public override Type GetColumnType(object bindingContext)
		{
			return ViewType;
		}
	}
}

