using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamvvm;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using DLToolkit.Forms.Controls;
using System.Collections.Generic;

namespace DLToolkitControlsSamples.SamplesFlowListView
{
    public class TotalRowSamplePageModel : BasePageModel
    {
        public TotalRowSamplePageModel()
        {
            ItemTappedCommand = new BaseCommand((param) =>
            {

                var item = LastTappedItem as SimpleItem;
                if (item != null)
                    System.Diagnostics.Debug.WriteLine("Tapped {0}", item.Title);

            });
        }

        public FlowObservableCollection<object> Items
        {
            get { return GetField<FlowObservableCollection<object>>(); }
            set { SetField(value); }
        }

        public ICommand ChangeColumnCountCommand
        {
            get { return GetField<ICommand>(); }
            set { SetField(value); }
        }

        public void ReloadData()
        {
            var exampleData = new List<object>();

            var howMany = 60;

            for (int i = 0; i < howMany; i++)
            {
                exampleData.Add(new SimpleItem() { Title = string.Format("Item nr {0}", i) });
            }

            exampleData.Add(new SimpleTotalModel());

            Items = new FlowObservableCollection<object>(exampleData);
        }

        public ICommand ItemTappedCommand
        {
            get { return GetField<ICommand>(); }
            set { SetField(value); }
        }

        public object LastTappedItem
        {
            get { return GetField<object>(); }
            set { SetField(value); }
        }

        public class SimpleTotalModel : FlowEmptyModel
        {
            public SimpleTotalModel()
            {
                Total = 3;
            }

            public int Total { get; set; }
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
