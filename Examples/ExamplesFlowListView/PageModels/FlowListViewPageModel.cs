using System;
using DLToolkit.PageFactory;
using System.Collections.ObjectModel;
using Examples.ExamplesFlowListView.Models;
using System.Windows.Input;

namespace Examples.ExamplesFlowListView.PageModels
{
    public class FlowListViewPageModel : BasePageModel
	{
		public FlowListViewPageModel()
		{
			ModifyCollectionCommand = new PageFactoryCommand(() => {
				Items.RemoveAt(0);
				Items.RemoveAt(0);
				Items.RemoveAt(0);
				Items.RemoveAt(0);
				Items.RemoveAt(0);
			});

			ModifyCollectionItemsCommand = new PageFactoryCommand(() => {
				foreach (var item in Items) 
				{
					item.Title = Guid.NewGuid().ToString().Substring(0, 4);
				}
			});
		}

        public ICommand ModifyCollectionCommand 
        {
            get { return GetField<ICommand>(); }
            set { SetField(value); }
        }

        public ICommand ModifyCollectionItemsCommand
        {
            get { return GetField<ICommand>(); }
            set { SetField(value); }
        }

		public ObservableCollection<FlowItem> Items
		{
			get { return GetField<ObservableCollection<FlowItem>>(); }
			set { SetField(value); }
		}
            
        public virtual void Reload()
        {
            var exampleData = new ObservableCollection<FlowItem>();

            var howMany = new Random().Next(100, 500);

            for (int i = 0; i < howMany; i++)
            {
                exampleData.Add(new FlowItem() { Title = string.Format("Item nr {0}", i) });
            }

            Items = exampleData;
        }
	}
}

