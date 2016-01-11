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
	/// <summary>
	/// FlowListView.
	/// </summary>
	public class FlowListView : ListView, IDisposable
	{
		/// <summary>
		/// Used to avoid linking issues
		/// eg. when using only XAML
		/// </summary>
		public static void Init()
		{
			#pragma warning disable 0219
			var dummy = new FlowListView();
			#pragma warning restore 0219
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Forms.Controls.FlowListView"/> class.
		/// </summary>
		public FlowListView()
		{
			RefreshDesiredColumnCount();
			SizeChanged += FlowListSizeChanged;
			PropertyChanged += FlowListViewPropertyChanged;
			PropertyChanging += FlowListViewPropertyChanging;
			FlowGroupKeySorting = FlowSorting.Ascending;
			FlowGroupItemSorting = FlowSorting.Ascending;
			FlowColumnExpand = FlowColumnExpand.None;
			FlowColumnsTemplates = new List<FlowColumnTemplateSelector>();
			GroupDisplayBinding = new Binding("Key");
			FlowAutoColumnCount = false;
			FlowColumnDefaultMinimumWidth = 50d;

			var flowListViewRef = new WeakReference<FlowListView>(this);
			ItemTemplate = new DataTemplate(() => new FlowListViewInternalCell(flowListViewRef));
			ItemSelected += FlowListViewItemSelected;
		}

		/// <summary>
		/// OBSOLETE! Gets or sets FlowListView group key selector.
		/// </summary>
		/// <value>The flow group key selector.</value>
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

		/// <summary>
		/// OBSOLETE! Gets or sets FlowListView group item sorting selector.
		/// </summary>
		/// <value>The flow group item sorting selector.</value>
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

		List<Func<object, Type>> flowColumnsDefinitions = null;

		/// <summary>
		/// OBSOLETE! Gets or sets FlowListView columns definitions.
		/// </summary>
		/// <value>The flow columns definitions.</value>
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

		/// <summary>
		/// Gets or sets FlowListView group grouping key selector.
		/// Make your own implementation of FlowPropertySelector
		/// </summary>
		/// <value>FlowListView group grouping key selector.</value>
		public FlowPropertySelector FlowGroupGroupingKeySelector { get; set; }

		/// <summary>
		/// Gets or sets FlowListView group item sorting key selector.
		/// Make your own implementation of FlowPropertySelector
		/// </summary>
		/// <value>FlowListView group item sorting key selector.</value>
		public FlowPropertySelector FlowGroupItemSortingKeySelector { get; set; }

		/// <summary>
		/// Gets or sets FlowListView group key sorting order.
		/// </summary>
		/// <value>FlowListView group key sorting order.</value>
		public FlowSorting FlowGroupKeySorting { get; set; }

		/// <summary>
		/// Gets or sets FlowListView group item sorting order.
		/// </summary>
		/// <value>FlowListView group item sorting order.</value>
		public FlowSorting FlowGroupItemSorting { get; set; }

		/// <summary>
		/// Gets or sets FlowListView column expand mode.
		/// It defines how columns should expand when 
		/// row current column count is less than defined columns templates count
		/// </summary>
		/// <value>FlowListView column expand mode.</value>
		public FlowColumnExpand FlowColumnExpand { get; set; }

		/// <summary>
		/// Enables or disables FlowListView auto column count.
		/// Column count is calculated basing on View width 
		/// and <c>FlowColumnDefaultMinimumWidth</c> property
		/// </summary>
		/// <value><c>true</c> enables auto column count, <c>false</c> disables.</value>
		public bool FlowAutoColumnCount { get; set; }

		/// <summary>
		/// Gets or sets the minimum column width of FlowListView.
		/// Currently used only with <c>FlowAutoColumnCount</c> option
		/// </summary>
		/// <value>The minimum column width.</value>
		public double FlowColumnDefaultMinimumWidth { get; set; }

		/// <summary>
		/// Occurs when FlowListView item is tapped.
		/// </summary>
		public event EventHandler<ItemTappedEventArgs> FlowItemTapped;

		/// <summary>
		/// FlowLastTappedItemProperty.
		/// </summary>
		public static BindableProperty FlowLastTappedItemProperty = BindableProperty.Create<FlowListView, object>(v => v.FlowLastTappedItem, default(object), BindingMode.OneWayToSource);

		/// <summary>
		/// Gets FlowListView last tapped item.
		/// </summary>
		/// <value>FlowListView last tapped item.</value>
		public object FlowLastTappedItem
		{
			get { return GetValue(FlowLastTappedItemProperty); }
			private set { SetValue(FlowLastTappedItemProperty, value);  }
		}

		/// <summary>
		/// FlowItemTappedCommandProperty.
		/// </summary>
		public static BindableProperty FlowItemTappedCommandProperty = BindableProperty.Create<FlowListView, ICommand>(v => v.FlowItemTappedCommand, null);

		/// <summary>
		/// Gets or sets FlowListView item tapped command.
		/// </summary>
		/// <value>FlowListView item tapped command.</value>
		public ICommand FlowItemTappedCommand
		{
			get { return (ICommand)GetValue(FlowItemTappedCommandProperty); }
			set { SetValue(FlowItemTappedCommandProperty, value); }
		}

		/// <summary>
		/// FlowItemsSourceProperty.
		/// </summary>
		public static BindableProperty FlowItemsSourceProperty = BindableProperty.Create<FlowListView, IList>(v => v.FlowItemsSource, default(IList));

		/// <summary>
		/// Gets FlowListView items source.
		/// </summary>
		/// <value>FlowListView items source.</value>
		public IList FlowItemsSource
		{
			get { return (IList)GetValue(FlowItemsSourceProperty); }
			private set { SetValue(FlowItemsSourceProperty, value); }
		}

		/// <summary>
		/// FlowColumnsTemplatesProperty.
		/// </summary>
		public static readonly BindableProperty FlowColumnsTemplatesProperty = BindableProperty.Create<FlowListView, List<FlowColumnTemplateSelector>>(v => v.FlowColumnsTemplates, new List<FlowColumnTemplateSelector>());

		/// <summary>
		/// Gets or sets FlowListView columns templates.
		/// Use instance of <c>FlowColumnSimpleTemplateSelector</c> for simple single view scenarios
		/// or implement your own FlowColumnTemplateSelector which can return cell type 
		/// basing on current cell BindingContext
		/// </summary>
		/// <value>FlowListView columns templates.</value>
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

		/// <summary>
		/// Forces FlowListView reload.
		/// </summary>
		public void ForceReload()
		{
			RefreshDesiredColumnCount();

			if (IsGroupingEnabled)
				ReloadGroupedContainerList();
			else
				ReloadContainerList();
		}

		internal void FlowPerformTap(object item)
		{
			FlowLastTappedItem = item;

			EventHandler<ItemTappedEventArgs> handler = FlowItemTapped;
			if (handler != null)
			{
				handler(this, new ItemTappedEventArgs(null, item));
			}

			if (FlowItemTappedCommand != null && FlowItemTappedCommand.CanExecute(item)) 
			{
				FlowItemTappedCommand.Execute(item);
			}
		}

		int desiredColumnCount;
		internal int DesiredColumnCount
		{
			get
			{
				if (desiredColumnCount == 0)
					return 1;
				
				return desiredColumnCount;
			}
			set
			{
				desiredColumnCount = value;
			}
		}

		private void RefreshDesiredColumnCount()
		{
			if (FlowAutoColumnCount)
			{
				double listWidth = Math.Max(Math.Max(Width, WidthRequest), MinimumWidthRequest);

				if (listWidth > 0)
				{
					DesiredColumnCount = (int)Math.Floor(listWidth / FlowColumnDefaultMinimumWidth);
				}
			}
			else
			{
				DesiredColumnCount = FlowColumnsTemplates.Count;
			}
		}

		double? lastWidth = null;
		private void FlowListSizeChanged(object sender, EventArgs e)
		{
			if (FlowAutoColumnCount)
			{
				double listWidth = Math.Max(Math.Max(Width, WidthRequest), MinimumWidthRequest);

				if (listWidth > 0)
				{
					if ((lastWidth.HasValue && Math.Abs(lastWidth.Value - listWidth) > double.Epsilon)
						|| !lastWidth.HasValue)
					{
						if (ItemsSource != null)
							ForceReload();
					}

					lastWidth = listWidth;
				}	
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

		private void FlowListViewItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			SelectedItem = null;
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
			var sortedKeys = FlowGroupKeySorting == FlowSorting.Ascending 
				? groupDict.Keys.OrderBy(v => v) : groupDict.Keys.OrderByDescending(v => v);


			foreach (var key in sortedKeys)
			{
				var flowGroup = new FlowGroup(key);
				var sortedItems = FlowGroupItemSorting == FlowSorting.Ascending
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

		/// <summary>
		/// Releases all resource used by the <see cref="DLToolkit.Forms.Controls.FlowListView"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="DLToolkit.Forms.Controls.FlowListView"/>.
		/// The <see cref="Dispose"/> method leaves the <see cref="DLToolkit.Forms.Controls.FlowListView"/> in an unusable
		/// state. After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="DLToolkit.Forms.Controls.FlowListView"/> so the garbage collector can reclaim the memory that the
		/// <see cref="DLToolkit.Forms.Controls.FlowListView"/> was occupying.</remarks>
		public void Dispose()
		{
			ItemSelected -= FlowListViewItemSelected;
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

