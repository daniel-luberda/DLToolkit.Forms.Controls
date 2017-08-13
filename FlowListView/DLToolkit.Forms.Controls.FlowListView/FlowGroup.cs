using System;
using System.Collections.ObjectModel;

namespace DLToolkit.Forms.Controls
{
    [Helpers.FlowListView.Preserve(AllMembers = true)]
	internal class FlowGroup : FlowObservableCollection<object>
	{
		public object Key { get; private set; }

		public FlowGroup(object key)
		{
			Key = key;
		}
	}
}
