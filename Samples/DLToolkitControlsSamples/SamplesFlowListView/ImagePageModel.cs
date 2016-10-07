using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DLToolkit.PageFactory;

namespace DLToolkitControlsSamples
{
	public class ImagePageModel : BasePageModel
	{
		public ImagePageModel()
		{
			ItemTappedCommand = new BaseCommand((param) =>
			{

				var item = LastTappedItem as ImageItem;
				if (item != null)
					System.Diagnostics.Debug.WriteLine("Tapped {0}", item.FileName);

			});
			ReloadData();
		}

		public ObservableCollection<ImageItem> Items
		{
			get { return GetField<ObservableCollection<ImageItem>>(); }
			set { SetField(value); }
		}

		public void ReloadData()
		{
			var exampleData = new ObservableCollection<ImageItem>();


        	exampleData.Add(new ImageItem() {FileName = "red.jpg"});
            exampleData.Add(new ImageItem() { FileName = "green.jpg" });
            exampleData.Add(new ImageItem() { FileName = "yellow.jpg" });
            exampleData.Add(new ImageItem() { FileName = "blue.jpg" });
            exampleData.Add(new ImageItem() { FileName = "purple.jpg" });
            exampleData.Add(new ImageItem() { FileName = "brown.jpg" });

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

		public class ImageItem : BaseModel
		{
			string fileName;
			public string FileName
			{
				get { return fileName; }
				set { SetField(ref fileName, value); }
			}
		}
	}
}
