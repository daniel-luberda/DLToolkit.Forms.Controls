using System;
using DLToolkit.PageFactory;
using System.Collections.ObjectModel;

namespace Examples.ViewModels
{
	public class MenuViewModel : BaseViewModel
	{
		public MenuViewModel()
		{
			Items = new ObservableCollection<MenuItem>() {
				
				new MenuItem() {
					Title = "FlowListView",
					SubTitle = "Flow List View Demo",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<FlowListViewViewModel>()
						.SendMessageToViewModel("FillWithData")
						.PushPage())
				},

				new MenuItem() {
					Title = "FlowListView",
					SubTitle = "Flow List View Grouping Demo",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<FlowListViewGroupingViewModel>()
						.SendMessageToViewModel("FillWithData")
						.PushPage())
				},

				new MenuItem() {
					Title = "FlowListView",
					SubTitle = "Flow List View Expanding Columns Demo",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<FlowListViewExpandViewModel>()
						.SendMessageToViewModel("FillWithData")
						.PushPage())
				},

				new MenuItem() {
					Title = "TagEntryView",
					SubTitle = "Tag Entry View Demo",
					Command = new PageFactoryCommand(() => 
						PageFactory.GetMessagablePageFromCache<TagEntryViewViewModel>()
						.SendMessageToViewModel("FillWithData")
						.PushPage())
				},
			};
		}

		public ObservableCollection<MenuItem> Items
		{
			get { return GetField<ObservableCollection<MenuItem>>(); }
			set { SetField(value); }
		}

		public class MenuItem : BaseModel
		{
			IPageFactoryCommand command;
			public IPageFactoryCommand Command
			{
				get { return command; }
				set { SetField(ref command, value); }
			}

			string title;
			public string Title
			{
				get { return title; }
				set { SetField(ref title, value); }
			}

			string subTitle;
			public string SubTitle
			{
				get { return subTitle; }
				set { SetField(ref subTitle, value); }
			}
		}
	}
}

