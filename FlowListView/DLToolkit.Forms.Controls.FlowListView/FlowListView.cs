using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace DLToolkit.Forms.Controls
{
	public class FlowListView : ListView, IDisposable
	{
		public FlowListView()
		{
			PropertyChanged += FlowListViewPropertyChanged;
			PropertyChanging += FlowListViewPropertyChanging;
			FlowGroupKeySorting = FlowGroupSorting.Ascending;
			FlowGroupItemSorting = FlowGroupSorting.Ascending;
			FlowColumnExpand = FlowColumnExpand.None;
		}

		public Func<object, object> FlowGroupKeySelector { get; set; }

		public Func<object, object> FlowGroupItemSortingSelector { get; set; }

		public FlowGroupSorting FlowGroupKeySorting { get; set; }

		public FlowGroupSorting FlowGroupItemSorting { get; set; }

		public FlowColumnExpand FlowColumnExpand { get; set; }

		public event EventHandler<ItemTappedEventArgs> FlowItemTapped;

		internal void FlowPerformTap(object item)
		{
			EventHandler<ItemTappedEventArgs> handler = FlowItemTapped;
			if (handler != null) handler(this, new ItemTappedEventArgs(null, item));
		}

		List<Func<object, Type>> flowColumnsDefinitions;
		public List<Func<object, Type>> FlowColumnsDefinitions 
		{ 
			get 
			{
				return flowColumnsDefinitions;
			}
			set
			{
				flowColumnsDefinitions = value;

				if (flowColumnsDefinitions != null && flowColumnsDefinitions.Count > 0)
				{
					ItemTemplate = new DataTemplate(() => new FlowListViewInternalCell(this));
				}
			}
		}

		public void ForceReload()
		{
			if (IsGroupingEnabled)
				ReloadGroupedContainerList();
			else
				ReloadContainerList();
		}

		public static BindableProperty FlowItemsSourceProperty = BindableProperty.Create<FlowListView, IList>(v => v.FlowItemsSource, default(IList));
		public IList FlowItemsSource
		{
			get { return (IList)GetValue(FlowItemsSourceProperty); }
			set { SetValue(FlowItemsSourceProperty, value); }
		}

		private void FlowListViewPropertyChanging (object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == FlowItemsSourceProperty.PropertyName)
			{
				var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
				if (flowItemSource != null)
					flowItemSource.CollectionChanged -= FlowItemsSourceCollectionChanged;
			}
		}

		private void FlowListViewPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == FlowItemsSourceProperty.PropertyName)
			{
				var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
				if (flowItemSource != null)
					flowItemSource.CollectionChanged += FlowItemsSourceCollectionChanged;

				if (FlowColumnsDefinitions == null || FlowColumnsDefinitions.Count == 0 || FlowItemsSource == null)
				{
					ItemsSource = null;
					return;
				}

				ForceReload();
			}
		}

		private void FlowItemsSourceCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			ForceReload();
		}

		private void ReloadContainerList()
		{
			var colCount = FlowColumnsDefinitions.Count;
			int capacity = (FlowItemsSource.Count / colCount) + 
				(FlowItemsSource.Count % colCount) > 0 ? 1 : 0;
			
			var tempList = new List<ObservableCollection<object>>(capacity);
			int position = -1;

			for (int i = 0; i < FlowItemsSource.Count; i++)
			{
				if (i % colCount == 0)
				{
					position++;

					tempList.Add(new ObservableCollection<object>() {
						FlowItemsSource[i]
					});
				}
				else
				{
					var exContItm = tempList[position];
					exContItm.Add(FlowItemsSource[i]);
				}
			}

			ItemsSource = new ObservableCollection<ObservableCollection<object>>(tempList);
		}

		private void ReloadGroupedContainerList()
		{
			var colCount = FlowColumnsDefinitions.Count;
			var groupDict = new Dictionary<object, IList<object>>();

			foreach (var item in FlowItemsSource)
			{
				var itemGroupKey = FlowGroupKeySelector(item);
				IList<object> groupContainer;
				if (!groupDict.TryGetValue(itemGroupKey, out groupContainer))
				{
					groupContainer = new List<object>();
					groupDict.Add(itemGroupKey, groupContainer);
				}
				groupContainer.Add(item);
			}

			var flowGroupsList = new List<FlowGroup>(groupDict.Keys.Count);
			var sortedKeys = FlowGroupKeySorting == FlowGroupSorting.Ascending 
				? groupDict.Keys.OrderBy(v => v) : groupDict.Keys.OrderByDescending(v => v);


			foreach (var key in sortedKeys)
			{
				var flowGroup = new FlowGroup(key);
				var sortedItems = FlowGroupItemSorting == FlowGroupSorting.Ascending
					? groupDict[key].OrderBy(v => FlowGroupItemSortingSelector(v)).ToList()
					: groupDict[key].OrderByDescending(v => FlowGroupItemSortingSelector(v)).ToList();

				int position = -1;

				for (int i = 0; i < sortedItems.Count; i++)
				{
					if (i % colCount == 0)
					{
						position++;

						flowGroup.Add(new ObservableCollection<object>() {
							sortedItems[i]
						});
					}
					else
					{
						var exContItm = flowGroup[position];
						exContItm.Add(sortedItems[i]);
					}
				}

				flowGroupsList.Add(flowGroup);
			}

			ItemsSource = new ObservableCollection<FlowGroup>(flowGroupsList);
		}
			
		public void Dispose()
		{
			PropertyChanged -= FlowListViewPropertyChanged;
			PropertyChanging -= FlowListViewPropertyChanging;

			var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
			if (flowItemSource != null)
			{
				flowItemSource.CollectionChanged -= FlowItemsSourceCollectionChanged;
			}
		}
	}
}

