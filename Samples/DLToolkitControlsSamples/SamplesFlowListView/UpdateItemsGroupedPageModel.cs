using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples
{
	public class UpdateItemsGroupedPageModel : BasePageModel
	{
		static int insertId = 0;

		public UpdateItemsGroupedPageModel()
		{
			ItemTappedCommand = new BaseCommand((param) =>
			{

				var item = LastTappedItem as SimpleItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("Tapped {0}", item.Title);

			});

			AddCommand = new BaseCommand((arg) =>
			{
				insertId++;
				Items[0].Insert(10, new SimpleItem() { Title = string.Format("New {0}", insertId) });
			});

			RemoveCommand = new BaseCommand((arg) =>
			{
				Items[0].RemoveAt(10);
			});
		}

		public ObservableCollection<Grouping<string, SimpleItem>> Items
		{
			get { return GetField<ObservableCollection<Grouping<string, SimpleItem>>>(); }
			set { SetField(value); }
		}

		public void ReloadData()
		{
			var exampleData = new ObservableCollection<SimpleItem>();

			for (int grId = 0; grId < 5; grId++)
			{
				var howMany = new Random().Next(15, 20);

				for (int i = 0; i < howMany; i++)
				{
					exampleData.Add(new SimpleItem() { Title = string.Format("{0}:{1}", grId, i) });
				}
			}

			var sorted = exampleData
				.OrderBy(item => item.Title.Length)
				.ThenBy(item => item.Title)
				.GroupBy(item => item.Title[0].ToString())
				.Select(itemGroup => new Grouping<string, SimpleItem>(itemGroup.Key, itemGroup));

			Items = new ObservableCollection<Grouping<string, SimpleItem>>(sorted);
		}

		public ICommand ItemTappedCommand
		{
			get { return GetField<ICommand>(); }
			set { SetField(value); }
		}

		public ICommand AddCommand
		{
			get { return GetField<ICommand>(); }
			set { SetField(value); }
		}

		public ICommand RemoveCommand
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

			public Color Color { get; private set; } = Colors.RandomColor;
		}
	}
}
