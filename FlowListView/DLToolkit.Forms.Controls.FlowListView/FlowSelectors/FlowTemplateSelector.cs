using System;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
	public abstract class FlowTemplateSelector : DataTemplate
	{
		public DataTemplate SelectTemplate(object item, int columnIndex, BindableObject container)
		{
			DataTemplate result = OnSelectTemplate(item, columnIndex, container);
			if (result is DataTemplateSelector || result is FlowTemplateSelector)
				throw new NotSupportedException("FlowTemplateSelector.OnSelectTemplate must not return another DataTemplateSelector");
			return result;
		}

		protected abstract DataTemplate OnSelectTemplate(object item, int columnIndex, BindableObject container);
	}
}
