using System;
using DLToolkit.PageFactory;
using System.Collections.ObjectModel;
using Examples.ExamplesFlowListView.Models;

namespace Examples.ExamplesFlowListView.ViewModels
{
	public class FlowListViewViewModel : BaseViewModel
	{
		public FlowListViewViewModel()
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

		public IPageFactoryCommand ModifyCollectionCommand { get; private set; }

		public IPageFactoryCommand ModifyCollectionItemsCommand { get; private set; }

		public ObservableCollection<FlowItem> Items
		{
			get { return GetField<ObservableCollection<FlowItem>>(); }
			set { SetField(value); }
		}

		public override void PageFactoryMessageReceived(string message, object sender, object arg)
		{
			if (message == "FillWithData")
			{
				FillWithData();
			}
		}

		public void FillWithData()
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

