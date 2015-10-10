using System;
using System.Collections.ObjectModel;
using System.Collections;

namespace DLToolkit.Forms.Controls
{
	internal class FlowGroup : ObservableCollection<ObservableCollection<object>>
	{
		public object Key { get; private set; }

		public FlowGroup(object key)
		{
			Key = key;
		}
	}
}

