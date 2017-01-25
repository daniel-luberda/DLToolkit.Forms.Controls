using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;

namespace DLToolkitControlsSamples
{
	public class UpdateItemsPageModel : BasePageModel
	{
		static int insertId = 0;

		public UpdateItemsPageModel()
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
				Items.Insert(10, new SimpleItem() { Title = string.Format("New {0}", insertId) }); 
			});

			RemoveCommand = new BaseCommand((arg) =>
			{
				Items.RemoveAt(10);
			});
		}

		public ObservableCollection<object> Items
		{
			get { return GetField<ObservableCollection<object>>(); }
			set { SetField(value); }
		}

		public void ReloadData()
		{
			var exampleData = new ObservableCollection<object>();

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
