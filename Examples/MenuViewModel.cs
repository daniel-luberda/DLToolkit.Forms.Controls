using System;
using DLToolkit.PageFactory;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Examples
{
	public class MenuViewModel : BaseViewModel
	{
		public MenuViewModel()
		{
			var menuItems = new List<MenuItem>() {
				new MenuItem() {
					Section = "FlowListView",
					Title = "Simple Example",
					Detail = "Simplest fixed column number example",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<ExamplesFlowListView.ViewModels.SimpleExampleViewModel>()
						.SendMessageToViewModel("Reload")
						.PushPage())
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Simple Example (XAML)",
					Detail = "Simplest fixed column number example",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<ExamplesFlowListView.ViewModels.SimpleExampleXamlViewModel>()
						.SendMessageToViewModel("Reload")
						.PushPage())
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Multiple columns templates",
					Detail = "Each column has a different view template",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<ExamplesFlowListView.ViewModels.MultipleTemplatesViewModel>()
						.SendMessageToViewModel("Reload")
						.PushPage())
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Multiple columns templates (XAML)",
					Detail = "Each column has a different view template",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<ExamplesFlowListView.ViewModels.MultipleTemplatesXamlViewModel>()
						.SendMessageToViewModel("Reload")
						.PushPage())
				},
						
				new MenuItem() {
					Section = "FlowListView",
					Title = "Flow List View Grouping Demo",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<ExamplesFlowListView.ViewModels.FlowListViewGroupingViewModel>()
						.SendMessageToViewModel("FillWithData")
						.PushPage())
				},

				new MenuItem() {
					Section = "FlowListView",
					Title = "Flow List View Expanding Columns Demo",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<ExamplesFlowListView.ViewModels.FlowListViewExpandViewModel>()
						.SendMessageToViewModel("FillWithData")
						.PushPage())
				},

				new MenuItem() {
					Section = "TagEntryView",
					Title = "Tag Entry View Demo",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<ExamplesTagEntryView.ViewModels.TagEntryViewViewModel>()
						.SendMessageToViewModel("FillWithData")
						.PushPage())
				},
			};

			var sorted = menuItems
				.OrderBy(item => item.Section)
				// .ThenBy(item => item.Title)
				.GroupBy(item => item.Section)
				.Select(itemGroup => new Grouping<string, MenuItem>(itemGroup.Key, itemGroup));

			MenuItems = new ObservableCollection<Grouping<string, MenuItem>>(sorted);
		}

		public ObservableCollection<Grouping<string, MenuItem>> MenuItems
		{
			get { return GetField<ObservableCollection<Grouping<string, MenuItem>>>(); }
			set { SetField(value); }
		}

		public class Grouping<K, T> : ObservableCollection<T>
		{
			public K Key { get; private set; }

			public Grouping(K key, IEnumerable<T> items)
			{
				Key = key;
				foreach (var item in items)
					this.Items.Add(item);
			}
		}
	}
}

