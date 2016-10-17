using System;
using Xamvvm;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DLToolkitControlsSamples
{
	public class TemplateSelectorPageModel : BasePageModel
	{
		public TemplateSelectorPageModel()
		{
			ItemTappedCommand = new BaseCommand((param) =>
			{

				var item = LastTappedItem as SimpleItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("Tapped {0}", item.Title);

			});
		}

		public ObservableCollection<SimpleItem> Items
		{
			get { return GetField<ObservableCollection<SimpleItem>>(); }
			set { SetField(value); }
		}

		public void ReloadData()
		{
			var exampleData = new ObservableCollection<SimpleItem>();

			var howMany = new Random().Next(100, 500);

			for (int i = 0; i < howMany; i++)
			{
				exampleData.Add(new SimpleItem() { Title = string.Format("Item nr {0}", i) , Color = Colors.RandomColor});
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

			Color color;
			public Color Color
			{
				get { return color; }
				set { SetField(ref color, value); }
			}
		}
	}
}
