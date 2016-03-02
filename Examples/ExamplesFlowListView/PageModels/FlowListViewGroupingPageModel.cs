using System;
using Examples.ExamplesFlowListView.Models;
using System.Collections.ObjectModel;

namespace Examples.ExamplesFlowListView.PageModels
{
	public class FlowListViewGroupingPageModel : FlowListViewPageModel
	{
        public void Reload()
		{
			var exampleData = new ObservableCollection<FlowItem>();

			var howMany = new Random().Next(100, 200);

			for (int i = 0; i < howMany; i++)
			{
				exampleData.Add(new FlowItem() { Title = Guid.NewGuid().ToString("N").Substring(0, 8) });
			}

			Items = exampleData;
		}
	}
}

