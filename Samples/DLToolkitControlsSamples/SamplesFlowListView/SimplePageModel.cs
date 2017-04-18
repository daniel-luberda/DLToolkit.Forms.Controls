using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamvvm;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace DLToolkitControlsSamples
{
	public class SimplePageModel : BasePageModel
	{
		public SimplePageModel()
		{
			ItemTappedCommand = new BaseCommand((param) =>
			{

				var item = LastTappedItem as SimpleItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("Tapped {0}", item.Title);

			});

			ScrollToCommand = new BaseCommand((arg) =>
			{
				var page = this.GetCurrentPage() as SimplePage;
				page.FlowScrollTo(Items[Items.Count / 2]);
			});

			LoadingCommand = new BaseCommand(async (arg) =>
			{
				await LoadMore();
			});
		}

		public ObservableCollection<object> Items
		{
			get { return GetField<ObservableCollection<object>>(); }
			set { SetField(value); }
		}

		public ICommand ScrollToCommand
		{
			get { return GetField<ICommand>(); }
			set { SetField(value); }
		}

		public ICommand LoadingCommand
		{
			get { return GetField<ICommand>(); }
			set { SetField(value); }
		}

		public void ReloadData()
		{
			var exampleData = new ObservableCollection<object>();

			var howMany = 60;
			TotalRecords = 240;

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

		public bool IsLoadingInfinite
		{
			get { return GetField<bool>(); }
			set { SetField(value); }
		}

		public int TotalRecords
		{
			get { return GetField<int>(); }
			set { SetField(value); }
		}

		protected virtual async Task LoadMore()
		{
			var oldTotal = Items.Count;
			var items = Items.ToList();

			await Task.Delay(3000);

			var howMany = 60;

			for (int i = oldTotal; i < oldTotal + howMany; i++)
			{
				items.Add(new SimpleItem() { Title = string.Format("Item nr {0}", i) });
			}

			Items = new ObservableCollection<object>(items);

			IsLoadingInfinite = false;
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
