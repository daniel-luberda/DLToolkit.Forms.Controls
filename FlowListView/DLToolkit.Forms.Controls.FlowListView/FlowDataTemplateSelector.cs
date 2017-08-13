using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace DLToolkit.Forms.Controls
{
    [Helpers.FlowListView.Preserve(AllMembers = true)]
	public class FlowDataTemplateSelector : DataTemplateSelector
	{
		readonly WeakReference<FlowListView> _flowListViewRef;

		private DataTemplate _defaultTemplate;
		
		public FlowDataTemplateSelector(WeakReference<FlowListView> flowListViewRef)
		{
			_flowListViewRef = flowListViewRef;

			_defaultTemplate = new DataTemplate(() => new FlowListViewInternalCell(flowListViewRef));
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{                     
			if (item is FlowLoadingModel)
			{
				FlowListView flowListView = null;
				if (_flowListViewRef.TryGetTarget(out flowListView))
				{
					return flowListView.FlowLoadingTemplate;
				}
			}
			else if (item is FlowEmptyModel)
			{
				FlowListView flowListView = null;
				if (_flowListViewRef.TryGetTarget(out flowListView))
				{
					return flowListView.FlowEmptyTemplate;
				}
			}

			return _defaultTemplate;
		}
	}
}
