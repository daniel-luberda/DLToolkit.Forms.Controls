using System;
using DLToolkit.Forms.Controls;
using Examples.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Examples.ViewModels
{
	public class FlowListViewGroupingViewModel : FlowListViewViewModel
	{
		public override void PageFactoryMessageReceived(string message, object sender, object arg)
		{
			if (message == "FillWithData")
			{
				FillWithData();
			}
		}

		public new void FillWithData()
		{
			var exampleData = new ObservableCollection<FlowItem>();

			var howMany = new Random().Next(100, 200);

			for (int i = 0; i < howMany; i++)
			{
				exampleData.Add(new FlowItem() { Title = Guid.NewGuid().ToString().Substring(0, 4) });
			}

			Items = exampleData;
		}
	}
}

