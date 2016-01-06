using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;

namespace DLToolkit.Forms.Controls
{
	public class FlowListView : ListView, IDisposable
	{
		public FlowListView()
		{
			PropertyChanged += FlowListViewPropertyChanged;
			PropertyChanging += FlowListViewPropertyChanging;
			SizeChanged += FlowListSizeChanged;
			FlowGroupKeySorting = FlowGroupSorting.Ascending;
			FlowGroupItemSorting = FlowGroupSorting.Ascending;
			FlowColumnExpand = FlowColumnExpand.None;
			FlowColumnsTemplates = new List<FlowColumnTemplateSelector>();
			GroupDisplayBinding = new Binding("Key");
			FlowAutoColumnCount = false;
			FlowColumnDefaultMinimumWidth = 100d;

			var flowListViewRef = new WeakReference<FlowListView>(this);
			ItemTemplate = new DataTemplate(() => new FlowListViewInternalCell(flowListViewRef));
		}
			
		[Obsolete("You should use FlowGroupGroupingKeySelector property as it's XAML compatible")]
		public Func<object, object> FlowGroupKeySelector 
		{ 
			get
			{
				return ((FlowFuncPropertySelector)FlowGroupGroupingKeySelector).Selector;
			}
			set
			{
				FlowGroupGroupingKeySelector = new FlowFuncPropertySelector(value);
			}
		}

		[Obsolete("You should use FlowGroupItemSortingKeySelector property as it's XAML compatible")]
		public Func<object, object> FlowGroupItemSortingSelector
		{ 
			get
			{
				return ((FlowFuncPropertySelector)FlowGroupItemSortingKeySelector).Selector;
			}
			set
			{
				FlowGroupItemSortingKeySelector = new FlowFuncPropertySelector(value);
			}
		}

		public FlowPropertySelector FlowGroupGroupingKeySelector { get; set; }

		public FlowPropertySelector FlowGroupItemSortingKeySelector { get; set; }

		public FlowGroupSorting FlowGroupKeySorting { get; set; }

		public FlowGroupSorting FlowGroupItemSorting { get; set; }

		public FlowColumnExpand FlowColumnExpand { get; set; }

		public bool FlowAutoColumnCount { get; set; }

		public double FlowColumnDefaultMinimumWidth { get; set; }

		public event EventHandler<ItemTappedEventArgs> FlowItemTapped;

		internal void FlowPerformTap(object item)
		{
			EventHandler<ItemTappedEventArgs> handler = FlowItemTapped;
			if (handler != null) handler(this, new ItemTappedEventArgs(null, item));
		}

		internal int DesiredColumnCount { get; set; }

		void RefreshDesiredColumnCount()
		{
			if (FlowAutoColumnCount)
			{
				double listWidth = Math.Max(Math.Max(Width, WidthRequest), MinimumWidthRequest);
				DesiredColumnCount = (int)Math.Ceiling(listWidth / FlowColumnDefaultMinimumWidth);
			}
			else
			{
				DesiredColumnCount = FlowColumnsTemplates.Count;
			}
		}

		List<Func<object, Type>> flowColumnsDefinitions = null;

		[Obsolete("You should use FlowColumnsTemplates property as it's XAML compatible")]
		public List<Func<object, Type>> FlowColumnsDefinitions 
		{ 
			get 
			{
				return flowColumnsDefinitions;
			}
			set
			{
				flowColumnsDefinitions = value;

				var templates = new List<FlowColumnTemplateSelector>();

				if (flowColumnsDefinitions != null && flowColumnsDefinitions.Count > 0)
				{
					foreach (var item in flowColumnsDefinitions)
					{
						templates.Add(new FlowColumnFuncTemplateSelector(item));
					}
				}

				FlowColumnsTemplates = templates;
			}
		}

		public void ForceReload()
		{
			RefreshDesiredColumnCount();

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

		public static readonly BindableProperty FlowColumnsTemplatesProperty = BindableProperty.Create<FlowListView, List<FlowColumnTemplateSelector>>(v => v.FlowColumnsTemplates, new List<FlowColumnTemplateSelector>());

		public List<FlowColumnTemplateSelector> FlowColumnsTemplates
		{
			get
			{
				return (List<FlowColumnTemplateSelector>)GetValue(FlowColumnsTemplatesProperty);
			}
			set
			{		
				SetValue(FlowColumnsTemplatesProperty, value);
			}
		}

		double? lastWidth = null;
		private void FlowListSizeChanged(object sender, EventArgs e)
		{
			if (!FlowAutoColumnCount)
				return;

			var width = Width;

			if (width > 0)
			{
				if (lastWidth.HasValue && Math.Abs(lastWidth.Value - width) > double.Epsilon)
				{
					ForceReload();
				}

				lastWidth = width;
			}
		}

		private void FlowListViewPropertyChanging(object sender, PropertyChangingEventArgs e)
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

				if (FlowColumnsTemplates == null || FlowColumnsTemplates.Count == 0 || FlowItemsSource == null)
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
			var colCount = DesiredColumnCount;

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
			var colCount = DesiredColumnCount;
			var groupDict = new Dictionary<object, IList<object>>();

			foreach (var item in FlowItemsSource)
			{
				var itemGroupKey = FlowGroupGroupingKeySelector.GetProperty(item);
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
					? groupDict[key].OrderBy(v => FlowGroupItemSortingKeySelector.GetProperty(v)).ToList()
					: groupDict[key].OrderByDescending(v => FlowGroupItemSortingKeySelector.GetProperty(v)).ToList();

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
			SizeChanged -= FlowListSizeChanged;

			var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
			if (flowItemSource != null)
			{
				flowItemSource.CollectionChanged -= FlowItemsSourceCollectionChanged;
			}
		}
	}
}

