using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DLToolkitControlsSamples
{
	public class GroupingPageModel : SimplePageModel
	{
		public new void ReloadData()
		{
			var exampleData = new ObservableCollection<SimpleItem>();

			var howMany = new Random().Next(100, 200);

			for (int i = 0; i < howMany; i++)
			{
				exampleData.Add(new SimpleItem() { Title = Guid.NewGuid().ToString("N").Substring(0, 8) });
			}

			var sorted = exampleData
				.OrderBy(item => item.Title)
				.GroupBy(item => item.Title[0].ToString())
				.Select(itemGroup => new Grouping<string, SimpleItem>(itemGroup.Key, itemGroup));

			Items = new ObservableCollection<object>(sorted);
		}
	}
}
