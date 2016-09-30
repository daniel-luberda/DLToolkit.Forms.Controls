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
		public FlowListView() : base(ListViewCachingStrategy.RecycleElement)
		{
            InitialSetup();
		}

        public FlowListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            InitialSetup();
        }

        private void InitialSetup()
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
            FlowColumnDefaultMinimumWidth = 100d;
            FlowRowBackgroundColor = Color.Transparent;
            FlowTappedBackgroundColor = Color.Transparent;
            FlowTappedBackgroundDelay = 0;

            var flowListViewRef = new WeakReference<FlowListView>(this);
            ItemTemplate = new DataTemplate(() => new FlowListViewInternalCell(flowListViewRef));
            SeparatorVisibility = SeparatorVisibility.None;
            SeparatorColor = Color.Transparent;

            ItemSelected += FlowListViewItemSelected;
            ItemAppearing += FlowListViewItemAppearing;
            ItemDisappearing += FlowListViewItemDisappearing;
        }

		/// <summary>
		/// The flow group grouping key selector property.
		/// </summary>
		public static BindableProperty FlowGroupGroupingKeySelectorProperty = BindableProperty.Create(nameof(FlowGroupGroupingKeySelector), typeof(FlowPropertySelector), typeof(FlowListView));

		/// <summary>
		/// Gets or sets FlowListView group grouping key selector.
		/// Make your own implementation of FlowPropertySelector
		/// </summary>
		/// <value>FlowListView group grouping key selector.</value>
		public FlowPropertySelector FlowGroupGroupingKeySelector
		{
			get { return (FlowPropertySelector)GetValue(FlowGroupGroupingKeySelectorProperty); }
			set { SetValue(FlowGroupGroupingKeySelectorProperty, value); }
		}

		/// <summary>
		/// The flow group item sorting key selector property.
		/// </summary>
		public static BindableProperty FlowGroupItemSortingKeySelectorProperty = BindableProperty.Create(nameof(FlowGroupItemSortingKeySelector), typeof(FlowPropertySelector), typeof(FlowListView));

		/// <summary>
		/// Gets or sets FlowListView group item sorting key selector.
		/// Make your own implementation of FlowPropertySelector
		/// </summary>
		/// <value>FlowListView group item sorting key selector.</value>
		public FlowPropertySelector FlowGroupItemSortingKeySelector
		{
			get { return (FlowPropertySelector)GetValue(FlowGroupItemSortingKeySelectorProperty); }
			set { SetValue(FlowGroupItemSortingKeySelectorProperty, value); }
		}

		/// <summary>
		/// The flow group key sorting property.
		/// </summary>
		public static BindableProperty FlowGroupKeySortingProperty = BindableProperty.Create(nameof(FlowGroupKeySorting), typeof(FlowSorting), typeof(FlowListView), FlowSorting.Ascending);

		/// <summary>
		/// Gets or sets FlowListView group key sorting order.
		/// </summary>
		/// <value>FlowListView group key sorting order.</value>
		public FlowSorting FlowGroupKeySorting
		{
			get { return (FlowSorting)GetValue(FlowGroupKeySortingProperty); }
			set { SetValue(FlowGroupKeySortingProperty, value); }
		}

		/// <summary>
		/// The flow group grouping key selector property.
		/// </summary>
		public static BindableProperty FlowGroupItemSortingProperty = BindableProperty.Create(nameof(FlowGroupItemSorting), typeof(FlowSorting), typeof(FlowListView), FlowSorting.Ascending);

		/// <summary>
		/// Gets or sets FlowListView group item sorting order.
		/// </summary>
		/// <value>FlowListView group item sorting order.</value>
		public FlowSorting FlowGroupItemSorting
		{
			get { return (FlowSorting)GetValue(FlowGroupItemSortingProperty); }
			set { SetValue(FlowGroupItemSortingProperty, value); }
		}

		/// <summary>
		/// The flow group grouping key selector property.
		/// </summary>
		public static BindableProperty FlowColumnExpandProperty = BindableProperty.Create(nameof(FlowColumnExpand), typeof(FlowColumnExpand), typeof(FlowListView), FlowColumnExpand.None);

		/// <summary>
		/// Gets or sets FlowListView column expand mode.
		/// It defines how columns should expand when 
		/// row current column count is less than defined columns templates count
		/// </summary>
		/// <value>FlowListView column expand mode.</value>
		public FlowColumnExpand FlowColumnExpand
		{
			get { return (FlowColumnExpand)GetValue(FlowColumnExpandProperty); }
			set { SetValue(FlowColumnExpandProperty, value); }
		}

		public static BindableProperty FlowAutoColumnCountProperty = BindableProperty.Create(nameof(FlowAutoColumnCount), typeof(bool), typeof(FlowListView), false);

		/// <summary>
		/// Enables or disables FlowListView auto column count.
		/// Column count is calculated basing on View width 
		/// and <c>FlowColumnDefaultMinimumWidth</c> property
		/// </summary>
		/// <value><c>true</c> enables auto column count, <c>false</c> disables.</value>
		public bool FlowAutoColumnCount
		{
			get { return (bool)GetValue(FlowAutoColumnCountProperty); }
			set { SetValue(FlowAutoColumnCountProperty, value); }
		}

		public static BindableProperty FlowColumnDefaultMinimumWidthProperty = BindableProperty.Create(nameof(FlowColumnDefaultMinimumWidth), typeof(double), typeof(FlowListView), 100d);

		/// <summary>
		/// Gets or sets the minimum column width of FlowListView.
		/// Currently used only with <c>FlowAutoColumnCount</c> option
		/// </summary>
		/// <value>The minimum column width.</value>
		public double FlowColumnDefaultMinimumWidth
		{
			get { return (double)GetValue(FlowColumnDefaultMinimumWidthProperty); }
			set { SetValue(FlowColumnDefaultMinimumWidthProperty, value); }
		}

		public static BindableProperty FlowRowBackgroundColorProperty = BindableProperty.Create(nameof(FlowRowBackgroundColor), typeof(Color), typeof(FlowListView), Color.Transparent);

		/// <summary>
		/// Gets or sets the color of the flow default row background.
		/// Default: Transparent
		/// </summary>
		/// <value>The color of the flow default row background.</value>
		public Color FlowRowBackgroundColor
		{
			get { return (Color)GetValue(FlowRowBackgroundColorProperty); }
			set { SetValue(FlowRowBackgroundColorProperty, value); }
		}

		/// <summary>
		/// Occurs when FlowListView item is tapped.
		/// </summary>
		public event EventHandler<ItemTappedEventArgs> FlowItemTapped;

		/// <summary>
		/// Occurs when flow item is appearing.
		/// </summary>
		public event EventHandler<ItemVisibilityEventArgs> FlowItemAppearing;

		/// <summary>
		/// Occurs when flow item is disappearing.
		/// </summary>
		public event EventHandler<ItemVisibilityEventArgs> FlowItemDisappearing;

		/// <summary>
		/// FlowTappedBackgroundColor property.
		/// </summary>
		public static BindableProperty FlowTappedBackgroundColorProperty = BindableProperty.Create(nameof(FlowTappedBackgroundColor), typeof(Color), typeof(FlowListView), Color.Transparent);

		/// <summary>
		/// Gets or sets the background color of the cell when tapped.
		/// </summary>
		/// <value>The color of the flow tapped background.</value>
		public Color FlowTappedBackgroundColor
		{
			get { return (Color)GetValue(FlowTappedBackgroundColorProperty); }
			set { SetValue(FlowTappedBackgroundColorProperty, value);  }
		}

		/// <summary>
		/// FlowTappedBackgroundDelay property.
		/// </summary>
		public static BindableProperty FlowTappedBackgroundDelayProperty = BindableProperty.Create(nameof(FlowTappedBackgroundDelay), typeof(int), typeof(FlowListView), 0);

		/// <summary>
		/// Gets or sets the background color delay of the cell when tapped (miliseconds).
		/// </summary>
		/// <value>The flow tapped background delay.</value>
		public int FlowTappedBackgroundDelay
		{
			get { return (int)GetValue(FlowTappedBackgroundDelayProperty); }
			set { SetValue(FlowTappedBackgroundDelayProperty, value);  }
		}

		/// <summary>
		/// FlowLastTappedItemProperty.
		/// </summary>
		public static BindableProperty FlowLastTappedItemProperty = BindableProperty.Create(nameof(FlowLastTappedItem), typeof(object), typeof(FlowListView), default(object), BindingMode.OneWayToSource);

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
		public static BindableProperty FlowItemTappedCommandProperty = BindableProperty.Create(nameof(FlowItemTappedCommand), typeof(ICommand), typeof(FlowListView), null);

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
		/// FlowItemAppearingCommandProperty.
		/// </summary>
		public static BindableProperty FlowItemAppearingCommandProperty = BindableProperty.Create(nameof(FlowItemAppearingCommand), typeof(ICommand), typeof(FlowListView), null);

		/// <summary>
		/// Gets or sets FlowListView item tapped command.
		/// </summary>
		/// <value>FlowListView item tapped command.</value>
		public ICommand FlowItemAppearingCommand
		{
			get { return (ICommand)GetValue(FlowItemAppearingCommandProperty); }
			set { SetValue(FlowItemAppearingCommandProperty, value); }
		}

		/// <summary>
		/// FlowItemDisappearingCommandProperty.
		/// </summary>
		public static BindableProperty FlowItemDisappearingCommandProperty = BindableProperty.Create(nameof(FlowItemDisappearingCommand), typeof(ICommand), typeof(FlowListView), null);

		/// <summary>
		/// Gets or sets FlowListView item tapped command.
		/// </summary>
		/// <value>FlowListView item tapped command.</value>
		public ICommand FlowItemDisappearingCommand
		{
			get { return (ICommand)GetValue(FlowItemDisappearingCommandProperty); }
			set { SetValue(FlowItemDisappearingCommandProperty, value); }
		}

		/// <summary>
		/// FlowItemsSourceProperty.
		/// </summary>
		public static BindableProperty FlowItemsSourceProperty = BindableProperty.Create(nameof(FlowItemsSource), typeof(IList), typeof(FlowListView), default(IList));

		/// <summary>
		/// Gets FlowListView items source.
		/// </summary>
		/// <value>FlowListView items source.</value>
		public IList FlowItemsSource
		{
			get { return (IList)GetValue(FlowItemsSourceProperty); }
			set { SetValue(FlowItemsSourceProperty, value); }
		}

		/// <summary>
		/// FlowColumnsTemplatesProperty.
		/// </summary>
		public static readonly BindableProperty FlowColumnsTemplatesProperty = BindableProperty.Create(nameof(FlowColumnsTemplates), typeof(List<FlowColumnTemplateSelector>), typeof(FlowListView), new List<FlowColumnTemplateSelector>());

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

			var command = FlowItemTappedCommand;
			if (command != null && command.CanExecute(item)) 
			{
				command.Execute(item);
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
					if ((lastWidth.HasValue && Math.Abs(lastWidth.Value - listWidth) > Epsilon.DoubleValue)
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

		private void FlowListViewPropertyChanged(object sender, PropertyChangedEventArgs e)
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

		private void FlowItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ForceReload();
		}

		private void FlowListViewItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			SelectedItem = null;
		}

		private void FlowListViewItemAppearing (object sender, ItemVisibilityEventArgs e)
		{
			var container = e.Item as IEnumerable;
			if (container != null)
			{
				EventHandler<ItemVisibilityEventArgs> handler = FlowItemAppearing;
				var command = FlowItemAppearingCommand;

				if (handler == null && command == null)
					return;

				foreach (var item in container)
				{
					handler?.Invoke(this, new ItemVisibilityEventArgs(item));

					if (command != null && command.CanExecute(item))
						command.Execute(item);
				}
			}
		}

		private void FlowListViewItemDisappearing(object sender, ItemVisibilityEventArgs e)
		{
			var container = e.Item as IEnumerable;
			if (container != null)
			{
				EventHandler<ItemVisibilityEventArgs> handler = FlowItemDisappearing;
				var command = FlowItemDisappearingCommand;

				if (handler == null && command == null)
					return;

				foreach (var item in container)
				{
					handler?.Invoke(this, new ItemVisibilityEventArgs(item));

					if (command != null && command.CanExecute(item))
						command.Execute(item);
				}
			}
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

			IEnumerable<object> sortedKeys;
			if (FlowGroupKeySorting == FlowSorting.Ascending)
			{
				sortedKeys = groupDict.Keys.OrderBy(v => v);
			}
			else if (FlowGroupKeySorting == FlowSorting.Descending)
			{
				sortedKeys = groupDict.Keys.OrderByDescending(v => v);
			}
			else
			{
				sortedKeys = groupDict.Keys;
			}

			foreach (var key in sortedKeys)
			{
				var flowGroup = new FlowGroup(key);

				IList<object> sortedItems;
				if (FlowGroupItemSorting == FlowSorting.Ascending)
				{
					sortedItems = groupDict[key];
				}
				else if (FlowGroupItemSorting == FlowSorting.Descending)
				{
					sortedItems = groupDict[key].OrderBy(v => FlowGroupItemSortingKeySelector.GetProperty(v)).ToList();
				}
				else
				{
					sortedItems = groupDict[key].OrderByDescending(v => FlowGroupItemSortingKeySelector.GetProperty(v)).ToList();
				}

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
			ItemAppearing -= FlowListViewItemAppearing;
			ItemDisappearing -= FlowListViewItemDisappearing;
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

