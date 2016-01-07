using System;
using DLToolkit.PageFactory;
using System.Collections.ObjectModel;

namespace Examples.ExamplesFlowListView.ViewModels
{
	public class SimpleExampleXamlViewModel : BaseViewModel
	{
		public ObservableCollection<SimpleItem> Items
		{
			get { return GetField<ObservableCollection<SimpleItem>>(); }
			set { SetField(value); }
		}

		public override void PageFactoryMessageReceived(string message, object sender, object arg)
		{
			if (message == "Reload")
			{
				FillWithData();
			}
		}

		public void FillWithData()
		{
			var exampleData = new ObservableCollection<SimpleItem>();

			var howMany = new Random().Next(100, 500);

			for (int i = 0; i < howMany; i++)
			{
				exampleData.Add(new SimpleItem() { Title = string.Format("Item nr {0}", i) });
			}

			Items = exampleData;
		}

		public class SimpleItem : BaseModel
		{
			string title;
			public string Title
			{
				get { return title; }
				set { SetField(ref title, value); }
			}
		}
	}
}

