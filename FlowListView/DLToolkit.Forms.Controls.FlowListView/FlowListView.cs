    using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// FlowListView.
	/// </summary>
	[Helpers.FlowListView.Preserve(AllMembers = true)]
    public class FlowListView : ListView, IDisposable
	{
		/// <summary>
		/// Used to avoid linking issues
		/// eg. when using only XAML
		/// </summary>
		public static void Init()
		{
#pragma warning disable 0219
            var dummy1 = typeof(FlowListView);
            var dummy2 = typeof(FlowListViewInternalCell);
            var dummy3 = typeof(FlowDataTemplateSelector);
#pragma warning restore 0219
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DLToolkit.Forms.Controls.FlowListView"/> class.
		/// </summary>
		public FlowListView() : base(ListViewCachingStrategy.RecycleElement)
		{
			InitialSetup();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:DLToolkit.Forms.Controls.FlowListView"/> class.
		/// </summary>
		/// <param name="cachingStrategy">Caching strategy.</param>
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

			FlowColumnExpand = FlowColumnExpand.None;
			FlowColumnCount = default(int?);
			FlowColumnMinWidth = 50d;
			FlowRowBackgroundColor = Color.Transparent;
			FlowTappedBackgroundColor = Color.Transparent;
			FlowTappedBackgroundDelay = 0;

            var flowListViewRef = new WeakReference<FlowListView>(this);
			ItemTemplate = new FlowDataTemplateSelector(flowListViewRef);
            SeparatorVisibility = SeparatorVisibility.None;
            SeparatorColor = Color.Transparent;
            GroupDisplayBinding = new Binding(nameof(FlowGroup.Key));

			ItemSelected += FlowListViewItemSelected;
			ItemAppearing += FlowListViewItemAppearing;
			ItemDisappearing += FlowListViewItemDisappearing;
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

		BindingBase _flowGroupColumnCountBinding;

		/// <summary>
		/// Gets or sets the flow column count.
		/// </summary>
		/// <value>The flow column count binding.</value>
		public BindingBase FlowGroupColumnCountBinding
		{
			get { return _flowGroupColumnCountBinding; }
			set
			{
				if (_flowGroupColumnCountBinding == value)
					return;

				OnPropertyChanging();
				_flowGroupColumnCountBinding = value;
				OnPropertyChanged();
			}
		}

        BindingBase _flowItemVisibleBinding;

		/// <summary>
		/// Gets or sets the flow item is visible.
		/// </summary>
		/// <value>The flow item is visible.</value>
		public BindingBase FlowItemVisibleBinding
		{
			get { return _flowItemVisibleBinding; }
			set
			{
				if (_flowItemVisibleBinding == value)
					return;

				OnPropertyChanging();
				_flowItemVisibleBinding = value;
				OnPropertyChanged();
			}
		}

        BindingBase _flowGroupHeaderModelBinding;

        /// <summary>
        /// Gets or sets the flow group model binding.
        /// It will be available as Model property in your
        /// group header BindingContext
        /// </summary>
        /// <value>The flow group model binding.</value>
        public BindingBase FlowGroupHeaderModelBinding
        {
            get { return _flowGroupHeaderModelBinding; }
            set
            {
                if (_flowGroupHeaderModelBinding == value)
                    return;

                OnPropertyChanging();
                _flowGroupHeaderModelBinding = value;
                OnPropertyChanged();
            }
        }

        BindingBase _flowGroupDisplayBinding = new Binding(nameof(FlowGroup.Key));

		/// <summary>
		/// Gets or sets the flow group display binding.
		/// </summary>
		/// <value>The flow group display binding.</value>
		public BindingBase FlowGroupDisplayBinding
		{
			get { return _flowGroupDisplayBinding; }
			set
			{
				if (_flowGroupDisplayBinding == value)
					return;

				OnPropertyChanging();
				_flowGroupDisplayBinding = value;
				OnPropertyChanged();

				GroupDisplayBinding = value;
			}
		}

		BindingBase _flowGroupShortNameBinding;

		/// <summary>
		/// Gets or sets the flow group short name binding.
		/// </summary>
		/// <value>The flow group short name binding.</value>
		public BindingBase FlowGroupShortNameBinding
		{
			get { return _flowGroupShortNameBinding; }
			set
			{
				if (_flowGroupShortNameBinding == value)
					return;

				OnPropertyChanging();
				_flowGroupShortNameBinding = value;
				OnPropertyChanged();

				GroupShortNameBinding = value;
			}
		}

        /// <summary>
        /// The flow column count property.
        /// </summary>
        public static BindableProperty FlowColumnCountProperty = BindableProperty.Create(nameof(FlowColumnCount), typeof(int?), typeof(FlowListView), default(int?), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var list = (FlowListView)bindable;
            if (list.FlowItemsSource == null)
                return;

            if (!list.FlowColumnCount.HasValue || list.FlowColumnCount != list.FlowDesiredColumnCount)
                list.ForceReload();
        });

		/// <summary>
		/// Enables or disables FlowListView auto/manual column count.
		/// Auto Column count is calculated basing on View width 
		/// and <c>FlowColumnMinWidth</c> property
		/// </summary>
		/// <value>The flow column count.</value>
		public int? FlowColumnCount
		{
			get { return (int?)GetValue(FlowColumnCountProperty); }
			set { SetValue(FlowColumnCountProperty, value); }
		}

        /// <summary>
        /// The flow desired column count property key.
        /// </summary>
        public static readonly BindablePropertyKey FlowDesiredColumnCountPropertyKey = BindableProperty.CreateReadOnly(nameof(FlowDesiredColumnCount), typeof(int), typeof(FlowListView), 1, BindingMode.OneWayToSource);
		/// <summary>
		/// The flow column count property.
		/// </summary>
		public static readonly BindableProperty FlowDesiredColumnCountProperty = FlowDesiredColumnCountPropertyKey.BindableProperty;

		/// <summary>
		/// Flow Column count desired.
		/// </summary>
		/// <value>The flow column count desired.</value>
		public int FlowDesiredColumnCount
		{
			get { return (int)GetValue(FlowDesiredColumnCountProperty); }
			private set { SetValue(FlowDesiredColumnCountPropertyKey, value > 0 ? value : 1); }
		}

		/// <summary>
		/// The flow column default minimum width property.
		/// </summary>
		public static BindableProperty FlowColumnMinWidthProperty = BindableProperty.Create(nameof(FlowColumnMinWidth), typeof(double), typeof(FlowListView), 50d);

		/// <summary>
		/// Gets or sets the minimum column width of FlowListView.
		/// Currently used only with <c>FlowAutoColumnCount</c> option
		/// </summary>
		/// <value>The minimum column width.</value>
		public double FlowColumnMinWidth
		{
			get { return (double)GetValue(FlowColumnMinWidthProperty); }
			set { SetValue(FlowColumnMinWidthProperty, value); }
		}

		/// <summary>
		/// The flow row background color property.
		/// </summary>
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
		/// Forces FlowListView to use AbsoluteLayout internally
		/// When Enabled, auto row height can't be measured automatically,
		/// but it can improve performance
		/// </summary>
		/// <value><c>true</c> if flow use absolute layout internally; otherwise, <c>false</c>.</value>
		public bool FlowUseAbsoluteLayoutInternally { get; set; } = false;

		/// <summary>
		/// Gets or sets the background color of the cell when tapped.
		/// </summary>
		/// <value>The color of the flow tapped background.</value>
		public Color FlowTappedBackgroundColor
		{
			get { return (Color)GetValue(FlowTappedBackgroundColorProperty); }
			set { SetValue(FlowTappedBackgroundColorProperty, value); }
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
			set { SetValue(FlowTappedBackgroundDelayProperty, value); }
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
			private set { SetValue(FlowLastTappedItemProperty, value); }
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
		public static BindableProperty FlowItemsSourceProperty = BindableProperty.Create(nameof(FlowItemsSource), typeof(ICollection), typeof(FlowListView), default(ICollection));

		/// <summary>
		/// Gets FlowListView items source.
		/// </summary>
		/// <value>FlowListView items source.</value>
		public ICollection FlowItemsSource
		{
			get { return (IList)GetValue(FlowItemsSourceProperty); }
			set { SetValue(FlowItemsSourceProperty, value); }
		}

		/// <summary>
		/// FlowColumnsTemplatesProperty.
		/// </summary>
		public static readonly BindableProperty FlowColumnTemplateProperty = BindableProperty.Create(nameof(FlowColumnTemplate), typeof(DataTemplate), typeof(FlowListView), default(DataTemplate));

		/// <summary>
		/// Gets or sets FlowListView columns templates.
		/// Use instance of <c>FlowColumnSimpleTemplateSelector</c> for simple single view scenarios
		/// or implement your own FlowColumnTemplateSelector which can return cell type 
		/// basing on current cell BindingContext
		/// </summary>
		/// <value>FlowListView columns templates.</value>
		public DataTemplate FlowColumnTemplate
		{
			get
			{
				return (DataTemplate)GetValue(FlowColumnTemplateProperty);
			}
			set
			{
				SetValue(FlowColumnTemplateProperty, value);
			}
		}

		/// <summary>
		/// The is loading infinite is enabled property.
		/// </summary>
		public static BindableProperty FlowIsLoadingInfiniteEnabledProperty = BindableProperty.Create(nameof(FlowIsLoadingInfiniteEnabled), typeof(bool), typeof(FlowListView), false);

		/// <summary>
		/// Gets or sets FlowIsLoadingInfiniteEnabled loading is enabled.
		/// </summary>
		/// <value>FlowIsLoadingInfiniteEnabled loading is enabled.</value>
		public bool FlowIsLoadingInfiniteEnabled
		{
			get { return (bool)GetValue(FlowIsLoadingInfiniteEnabledProperty); }
			set { SetValue(FlowIsLoadingInfiniteEnabledProperty, value); }
		}

		/// <summary>
		/// The is loading infinite is running property.
		/// </summary>
		public static BindableProperty FlowIsLoadingInfiniteProperty = BindableProperty.Create(nameof(FlowIsLoadingInfinite), typeof(bool), typeof(FlowListView), false, BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets FlowIsLoadingInfinite loading is running.
		/// </summary>
		/// <value>FlowIsLoadingInfinite loading is running</value>
		public bool FlowIsLoadingInfinite
		{
			get { return (bool)GetValue(FlowIsLoadingInfiniteProperty); }
			set { SetValue(FlowIsLoadingInfiniteProperty, value); }
		}

		/// <summary>
		/// The total of records to loading infinite property.
		/// </summary>
		public static BindableProperty FlowTotalRecordsProperty = BindableProperty.Create(nameof(FlowTotalRecords), typeof(int), typeof(FlowListView), 0);

		/// <summary>
		/// Gets or sets FlowTotalRecords total records to loading infinite.
		/// It defines how columns should expand when 
		/// row current column count is less than defined columns templates count
		/// </summary>
		/// <value>FlowTotalRecords total records to loading infinite.</value>
		public int FlowTotalRecords
		{
			get { return (int)GetValue(FlowTotalRecordsProperty); }
			set { SetValue(FlowTotalRecordsProperty, value); }
		}

		/// <summary>
		/// FlowLoadingTemplateProperty.
		/// </summary>
		public static readonly BindableProperty FlowLoadingTemplateProperty = BindableProperty.Create(nameof(FlowLoadingTemplate), typeof(DataTemplate), typeof(FlowListView), default(DataTemplate));

		/// <summary>
		/// Gets or sets FlowLoadingTemplate loading template (ViewCell type).
		/// </summary>
		/// <value>FlowLoadingTemplate loading template (ViewCell type).</value>
		public DataTemplate FlowLoadingTemplate
		{
			get { return (DataTemplate)GetValue(FlowLoadingTemplateProperty); }
			set { SetValue(FlowLoadingTemplateProperty, value); }
		}

		/// <summary>
		/// FlowLoadingCommandProperty.
		/// </summary>
		public static BindableProperty FlowLoadingCommandProperty = BindableProperty.Create(nameof(FlowLoadingCommand), typeof(ICommand), typeof(FlowListView), null);

		/// <summary>
		/// Gets or sets FlowLoadingCommand loading execute command.
		/// </summary>
		/// <value>FlowLoadingCommand loading execute command.</value>
		public ICommand FlowLoadingCommand
		{
			get { return (ICommand)GetValue(FlowLoadingCommandProperty); }
			set { SetValue(FlowLoadingCommandProperty, value); }
		}

		/// <summary>
		/// FlowEmptyTemplateProperty.
		/// </summary>
		public static readonly BindableProperty FlowEmptyTemplateProperty = BindableProperty.Create(nameof(FlowEmptyTemplate), typeof(DataTemplate), typeof(FlowListView), default(DataTemplate));

		/// <summary>
		/// Gets or sets FlowEmptyTemplate empty data template (ViewCell type).
		/// </summary>
		/// <value>FlowEmptyTemplate empty data template (ViewCell type).</value>
		public DataTemplate FlowEmptyTemplate
		{
			get { return (DataTemplate)GetValue(FlowEmptyTemplateProperty); }
			set { SetValue(FlowEmptyTemplateProperty, value); }
		}

		/// <summary>
		/// Forces FlowListView reload.
		/// </summary>
		public void ForceReload(bool updateOnly = false)
		{
            System.Diagnostics.Debug.WriteLine("ForceReload");

			if (updateOnly)
			{
				if (IsGroupingEnabled)
					UpdateGroupedContainerList();
				else
					UpdateContainerList();
			}
			else
			{
                RefreshDesiredColumnCount();

				if (IsGroupingEnabled)
					ReloadGroupedContainerList();
				else
					ReloadContainerList();
			}
		}

        /// <summary>
        /// OnSizeAllocated.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			RefreshDesiredColumnCount();
		}

		internal void FlowPerformTap(object sender, object item)
		{
			FlowLastTappedItem = item;

			EventHandler<ItemTappedEventArgs> handler = FlowItemTapped;
			if (handler != null)
			{
				handler(this, new ItemTappedEventArgs(sender, item));
			}

			var command = FlowItemTappedCommand;
			if (command != null && command.CanExecute(item))
			{
				command.Execute(item);
			}
		}

		private void RefreshDesiredColumnCount()
		{
			if (!FlowColumnCount.HasValue)
			{
                var oldColumnCount = FlowDesiredColumnCount;
				double listWidth = Math.Max(Math.Max(Width, WidthRequest), MinimumWidthRequest);

				if (listWidth > 0)
				{
					FlowDesiredColumnCount = (int)Math.Floor(listWidth / FlowColumnMinWidth);
				}

                if (ItemsSource != null && oldColumnCount != FlowDesiredColumnCount)
                {
					foreach (var item in ItemsSource)
					{
						var internalModel = item as FlowGroupColumn;
						if (internalModel != null)
							internalModel.ForceInvalidateColumns = true;
					}
                }
			}
			else
			{
				FlowDesiredColumnCount = FlowColumnCount.Value;
			}
		}

		double? lastWidth = null;
		private void FlowListSizeChanged(object sender, EventArgs e)
		{
			if (!FlowColumnCount.HasValue)
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

		private void FlowListViewPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
		{
			if (e.PropertyName == FlowItemsSourceProperty.PropertyName)
			{
				var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
				if (flowItemSource != null)
					flowItemSource.CollectionChanged -= FlowItemsSourceCollectionChanged;

				if (IsGroupingEnabled)
				{
					var groupedSource = FlowItemsSource;
					if (groupedSource != null)
					{
						foreach (var gr in groupedSource)
						{
                            var collectionChanged = gr as INotifyCollectionChanged;
                            if (collectionChanged != null)
                                collectionChanged.CollectionChanged -= FlowItemsSourceCollectionChanged;
						}
					}
				}
			}
		}

		private void FlowListViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == FlowItemsSourceProperty.PropertyName)
			{
                if (FlowColumnTemplate == null || FlowItemsSource == null)
                {
                    ItemsSource = null;
                    return;
                }

				var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
				if (flowItemSource != null)
					flowItemSource.CollectionChanged += FlowItemsSourceCollectionChanged;

				if (IsGroupingEnabled)
				{
					var groupedSource = FlowItemsSource;
					if (groupedSource != null)
					{
						foreach (var gr in groupedSource)
						{
                            var collectionChanged = gr as INotifyCollectionChanged;
                            if (collectionChanged != null)
                                collectionChanged.CollectionChanged += FlowItemsSourceCollectionChanged;
						}
					}
				}

				ForceReload();
			}
		}

		private void FlowItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ForceReload(updateOnly: true);
		}

		private void FlowListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			SelectedItem = null;
		}

		private void FlowListViewItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			if (IsRefreshing || FlowIsLoadingInfinite || ItemsSource == null || !ItemsSource.Cast<object>().Any())
				return;

			if (!FlowIsLoadingInfinite && e.Item is FlowLoadingModel)
			{
				FlowIsLoadingInfinite = true;

				if (FlowLoadingCommand != null && FlowLoadingCommand.CanExecute(null))
				{
					FlowLoadingCommand.Execute(null);
				}
			}

			EventHandler<ItemVisibilityEventArgs> handler = FlowItemAppearing;
			var command = FlowItemAppearingCommand;

			if (handler == null && command == null)
				return;
			
			var container = e.Item as IEnumerable;
			if (container != null)
			{
				foreach (var item in container)
				{
					handler?.Invoke(this, new ItemVisibilityEventArgs(item));

					if (command != null && command.CanExecute(item))
						command.Execute(item);
				}
			}
			else
			{
				handler?.Invoke(this, new ItemVisibilityEventArgs(e.Item));

				if (command != null && command.CanExecute(e.Item))
					command.Execute(e.Item);
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

		private FlowObservableCollection<object> GetContainerList()
		{
			var colCount = FlowDesiredColumnCount;
			var tempList = new List<object>();

			var flowItemVisibleBindingPropertyName = (FlowItemVisibleBinding as Binding)?.Path;

			if (FlowItemsSource.Count <= 0 && FlowEmptyTemplate != null)
			{
				tempList.Add(new FlowEmptyModel());
			}
			else
			{
				int position = -1;
                var i = 0;
				var ix = 0;

				foreach (var item in FlowItemsSource)
				{
					if (flowItemVisibleBindingPropertyName != null)
					{
						var itemVisibleBindingPropertyName = item.GetType().GetRuntimeProperty(flowItemVisibleBindingPropertyName);

						if (itemVisibleBindingPropertyName != null)
						{
                            if (!(bool)itemVisibleBindingPropertyName.GetValue(item))
                            {
                                i++;
                                continue;
                            }
						}
					}

					if (ix % colCount == 0)
					{
						position++;

						tempList.Add(new FlowObservableCollection<object>() { item });
					}
					else
					{
                        var exContItm = (tempList[position] as IList);
						exContItm?.Add(item);
					}

                    i++;
					ix++;
				}

				if (FlowIsLoadingInfiniteEnabled && FlowItemsSource.Count < FlowTotalRecords)
				{
					tempList.Add(new FlowLoadingModel());
				}
			}

			return new FlowObservableCollection<object>(tempList);
		}

		private void UpdateContainerList()
		{
			var currentSource = ItemsSource as FlowObservableCollection<object>;

			if (currentSource != null && currentSource.Count > 0)
			{
				var tempList = GetContainerList();
				currentSource.Sync(tempList);
			}
			else
			{
				ReloadContainerList();
			}
		}

		private void ReloadContainerList()
		{
			ItemAppearing -= FlowListViewItemAppearing;
			ItemsSource = GetContainerList();
			ItemAppearing += FlowListViewItemAppearing;
		}

		private void UpdateGroupedContainerList()
		{
			var currentSource = ItemsSource as FlowObservableCollection<FlowGroup>;

			if (currentSource != null && currentSource.Count > 0)
			{
				var tempList = GetGroupedContainerList();
				currentSource.Sync(tempList);
            }
			else
			{
				ReloadGroupedContainerList();
			}
		}

		private FlowObservableCollection<FlowGroup> GetGroupedContainerList()
		{
			var colCount = FlowDesiredColumnCount;
			var flowGroupsList = new List<FlowGroup>(FlowItemsSource.Count);
			var groupDisplayPropertyName = (FlowGroupDisplayBinding as Binding)?.Path;
            var groupModelPropertyName = (FlowGroupHeaderModelBinding as Binding)?.Path;
			var groupColumnCountPropertyName = (FlowGroupColumnCountBinding as Binding)?.Path;
			var flowItemVisibleBindingPropertyName = (FlowItemVisibleBinding as Binding)?.Path;

			if (FlowItemsSource.Count <= 0 && FlowEmptyTemplate != null)
			{
                flowGroupsList.Add(new FlowGroup(null, null) { new FlowEmptyModel() });
			}

			foreach (var groupContainer in FlowItemsSource)
			{
				var isAlreadyFlowGroup = groupContainer as FlowGroup;

				if (isAlreadyFlowGroup != null)
				{
					flowGroupsList.Add(isAlreadyFlowGroup);
				}
				else
				{
					var gr = groupContainer as ICollection;
					if (gr != null)
					{
						var type = gr?.GetType();

						object groupKeyValue = null;
                        object groupModelValue = null;
						int? groupColumnCount = colCount;

						if (type != null && groupDisplayPropertyName != null)
						{
							PropertyInfo groupDisplayProperty = type?.GetRuntimeProperty(groupDisplayPropertyName);
							groupKeyValue = groupDisplayProperty?.GetValue(gr);
						}

                        if (type != null && groupModelPropertyName != null)
                        {
                            PropertyInfo groupModelProperty = type?.GetRuntimeProperty(groupModelPropertyName);
                            groupModelValue = groupModelProperty?.GetValue(gr);
                        }

						if (type != null && groupColumnCountPropertyName != null)
						{
							PropertyInfo groupColumnCountProperty = type?.GetRuntimeProperty(groupColumnCountPropertyName);

							groupColumnCount = (int?)groupColumnCountProperty?.GetValue(gr);
							groupColumnCount = groupColumnCount.GetValueOrDefault() > 0 ? groupColumnCount.Value : colCount;
						}

						if (groupKeyValue == null)
						{
							groupKeyValue = groupContainer;
						}

                        var flowGroup = new FlowGroup(groupKeyValue, groupModelValue);

						if (gr.Count <= 0 && FlowEmptyTemplate != null)
						{
							flowGroup.Add(new FlowEmptyModel());
						}
						else
						{
							int position = -1;
							var ix = 0;
                            var i = 0;

							foreach (var item in gr)
							{
								if (flowItemVisibleBindingPropertyName != null)
								{
									var itemVisibleBindingPropertyName = item.GetType().GetRuntimeProperty(flowItemVisibleBindingPropertyName);

									if (itemVisibleBindingPropertyName != null)
									{
                                        if (!(bool)itemVisibleBindingPropertyName.GetValue(item))
                                        {
                                            i++;
                                            continue;
                                        }
									}
								}

								if (ix % groupColumnCount == 0)
								{
									position++;

									flowGroup.Add(new FlowGroupColumn(groupColumnCount.GetValueOrDefault()) { item });
								}
								else
								{
									var exContItm = (flowGroup[position] as IList);
									exContItm?.Add(item);
								}

								ix++;
                                i++;
							}
						}

						flowGroupsList.Add(flowGroup);
					}
				}
			}

			if (FlowIsLoadingInfiniteEnabled && FlowItemsSource.Cast<object>().Sum(s => (s as IList).Count) < FlowTotalRecords)
			{
				flowGroupsList.LastOrDefault()?.Add(new FlowLoadingModel());
			}

			return new FlowObservableCollection<FlowGroup>(flowGroupsList);
		}

		private void ReloadGroupedContainerList()
		{
			ItemAppearing -= FlowListViewItemAppearing;
            var ctrList = GetGroupedContainerList();

            try
            {
                
                ItemsSource = ctrList;
            }
            catch (NullReferenceException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                //TODO HACK some strange Xamarin.Forms exceptionw when using grouping + fast scroll shortname list  !?
            }

			ItemAppearing += FlowListViewItemAppearing;
		}

		/// <summary>
		/// Scrolls list to specified item
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="position">Position.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public void FlowScrollTo(object item, ScrollToPosition position, bool animated)
		{
			if (!IsGroupingEnabled)
			{
				var castedItemsSource = ItemsSource as IEnumerable<object>;
                var internalItem = castedItemsSource?.FirstOrDefault(v => v == item || ((v as IEnumerable)?.Cast<object>().Contains(item)).GetValueOrDefault());
				ScrollTo(internalItem, position, animated);
			}
			else
			{
				var castedItemsSource = ItemsSource as IEnumerable<FlowGroup>;
				var internalItem = castedItemsSource?.Select(v => v.FirstOrDefault(itm => ((itm as IEnumerable)?.Cast<object>().Contains(item)).GetValueOrDefault())).FirstOrDefault(v => v != null);
				ScrollTo(internalItem, position, animated);
			}
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

