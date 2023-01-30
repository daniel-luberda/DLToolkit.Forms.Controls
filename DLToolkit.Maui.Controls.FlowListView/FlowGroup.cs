using System;
using System.Collections.ObjectModel;

namespace DLToolkit.Maui.Controls.FlowListView
{
    [Helpers.Preserve(AllMembers = true)]
    internal class FlowGroup : FlowObservableCollection<object>
    {
        public object Key { get; private set; }

        public object Model { get; private set; }

        public FlowGroup(object key, object model)
        {
            Key = key;
            Model = model;
        }
    }
}
