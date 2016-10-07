using System;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;

namespace DLToolkitControlsSamples
{
	public class TemplateSelectorPageSelector : FlowTemplateSelector
	{
		// Reuse DataTemplates instances !!!
		readonly DataTemplate _leftTemplate = new DataTemplate(typeof(TemplateSelectorPageTemplateLeft));
		readonly DataTemplate _rightTemplate = new DataTemplate(typeof(TemplateSelectorPageTemplateRight));

		protected override DataTemplate OnSelectTemplate(object item, int columnIndex, BindableObject container)
		{
			if (columnIndex == 0)
				return _leftTemplate;

			return _rightTemplate;
		}
	}
}
