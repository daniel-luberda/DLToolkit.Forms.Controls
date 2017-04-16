using System;
using System.Collections.ObjectModel;

namespace DLToolkit.Forms.Controls
{
	internal class FlowGroupColumn : ObservableCollection<object>
	{
		public int ColumnCount { get; set; }

		public FlowGroupColumn(int columnCount)
		{
			ColumnCount = columnCount;
		}
	}
}
