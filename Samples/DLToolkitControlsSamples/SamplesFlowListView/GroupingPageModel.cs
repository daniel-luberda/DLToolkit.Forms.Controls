using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamvvm;
using System.Collections.Generic;

namespace DLToolkitControlsSamples
{
	public class GroupingPageModel : SimplePageModel
	{
		public GroupingPageModel()
		{
			ScrollToCommand = new BaseCommand((arg) =>
			{
				var page = this.GetCurrentPage() as GroupingPage;
				var items = Items.SelectMany(v => (IEnumerable<object>)v).ToList();
				page.FlowScrollTo(items[items.Count / 2]);
			});
		}

		public new void ReloadData()
		{
			var exampleData = new ObservableCollection<SimpleItem>();

			var random = new Random(DateTime.Now.Millisecond);
			var howMany = random.Next(100, 200);

			for (int i = 0; i < howMany; i++)
			{
				exampleData.Add(new SimpleItem() { Title = Guid.NewGuid().ToString("N").Substring(0, 8) });
			}

			var sorted = exampleData
				.OrderBy(item => item.Title)
				.GroupBy(item => item.Title[0].ToString())
				.Select(itemGroup => new Grouping<string, SimpleItem>(itemGroup.Key, itemGroup, random.Next(1, 6)));

			Items = new ObservableCollection<object>(sorted);
		}


	}
}
