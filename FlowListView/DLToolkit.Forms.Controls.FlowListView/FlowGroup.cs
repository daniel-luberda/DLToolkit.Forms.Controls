using System;
using System.Collections.ObjectModel;

namespace DLToolkit.Forms.Controls
{
	internal class FlowGroup : SmartObservableCollection<object>
	{
		public object Key { get; private set; }

		public FlowGroup(object key)
		{
			Key = key;
		}
	}
}
