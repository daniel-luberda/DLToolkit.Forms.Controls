using System;
using System.Collections.ObjectModel;

namespace DLToolkit.Maui.Controls.FlowListView
{
    [Helpers.Preserve(AllMembers = true)]
    internal class FlowGroupColumn : FlowObservableCollection<object>
    {
        public int ColumnCount { get; set; }

        public bool ForceInvalidateColumns { get; set; }

        public FlowGroupColumn(int columnCount)
        {
            ColumnCount = columnCount;
        }
    }
}
