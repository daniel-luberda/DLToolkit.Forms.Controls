using System;
using DLToolkit.PageFactory;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Examples.ExamplesFlowListView.ViewModels
{
	public class SelectionViewModel : BaseViewModel
	{
		public SelectionViewModel()
		{
			ItemTappedCommand = new Command(async() => {

				var item = LastTappedItem as SimpleItem;
				if (item != null)
				{
					System.Diagnostics.Debug.WriteLine("Tapped {0}", item.Title);
				}
			});
		}

		public ObservableCollection<SimpleItem> Items
		{
			get { return GetField<ObservableCollection<SimpleItem>>(); }
			set { SetField(value); }
		}

		public override void PageFactoryMessageReceived(string message, object sender, object arg)
		{
			if (message == "Reload")
			{
				ReloadData();
			}
		}

		public void ReloadData()
		{
			var exampleData = new ObservableCollection<SimpleItem>();

			var howMany = new Random().Next(100, 500);

			for (int i = 0; i < howMany; i++)
			{
				exampleData.Add(new SimpleItem() { Title = string.Format("Item nr {0}", i) });
			}

			Items = exampleData;
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

