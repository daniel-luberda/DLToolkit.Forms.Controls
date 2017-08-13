using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// Flow template selector.
	/// </summary>
	[Helpers.FlowListView.Preserve(AllMembers = true)]
    public abstract class FlowTemplateSelector : DataTemplate
	{
		/// <summary>
		/// Selects the template.
		/// </summary>
		/// <returns>The template.</returns>
		/// <param name="item">Item.</param>
		/// <param name="columnIndex">Column index.</param>
		/// <param name="container">Container.</param>
		public DataTemplate SelectTemplate(object item, int columnIndex, BindableObject container)
		{
			DataTemplate result = OnSelectTemplate(item, columnIndex, container);
			if (result is DataTemplateSelector || result is FlowTemplateSelector)
				throw new NotSupportedException("FlowTemplateSelector.OnSelectTemplate must not return another DataTemplateSelector");
			return result;
		}

		/// <summary>
		/// Ons the select template.
		/// </summary>
		/// <returns>The select template.</returns>
		/// <param name="item">Item.</param>
		/// <param name="columnIndex">Column index.</param>
		/// <param name="container">Container.</param>
		protected abstract DataTemplate OnSelectTemplate(object item, int columnIndex, BindableObject container);
	}
}
