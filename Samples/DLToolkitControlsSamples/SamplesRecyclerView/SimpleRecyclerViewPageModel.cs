using System;
using System.Collections.Generic;
using System.Windows.Input;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples.SamplesRecyclerView
{
    public class SimpleRecyclerViewPageModel : BasePageModel
    {
        public SimpleRecyclerViewPageModel()
        {
            ItemTappedCommand = new BaseCommand((param) =>
            {
                var item = param as SimpleItem;
                if (item != null)
                    System.Diagnostics.Debug.WriteLine("Tapped {0}", item.Title);

            });
        }

        public FlowObservableCollection<object> Items
        {
            get { return GetField<FlowObservableCollection<object>>(); }
            set { SetField(value); }
        }

        public void ReloadData()
        {
            var exampleData = new List<object>();

            var howMany = 120;

            exampleData.Add(new SimpleItem() { Title = string.Format("Some very long text test Some very long text test Some very long text test Some very long text test") });

            for (int i = 0; i < howMany; i++)
            {
                exampleData.Add(new SimpleItem() { Title = string.Format("Item nr {0}", i) });
            }

            Items = new FlowObservableCollection<object>(exampleData);
        }

        public ICommand ItemTappedCommand
        {
            get { return GetField<ICommand>(); }
            set { SetField(value); }
        }

        public class SimpleItem : BaseModel
        {
            string title;
            public string Title
            {
                get { return title; }
                set { SetField(ref title, value); }
            }

            public Color Color { get; private set; } = Colors.RandomColor;
        }
    }
}
