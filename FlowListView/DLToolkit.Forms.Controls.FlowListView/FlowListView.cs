using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace DLToolkit.Forms.Controls
{
	public class FlowListView : ListView, IDisposable
	{
		public FlowListView()
		{
			PropertyChanged += FlowListViewPropertyChanged;
			PropertyChanging += FlowListViewPropertyChanging;
			ItemsSource = new ObservableList<ObservableList<object>>();
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
					ItemTemplate = new DataTemplate(() => new FlowListViewInternalCell(flowColumnsDefinitions));
				}
			}
		}

		public static BindableProperty FlowItemsSourceProperty = BindableProperty.Create<FlowListView, IList>(v => v.FlowItemsSource, default(IList));
		public IList FlowItemsSource
		{
			get { return (IList)GetValue(FlowItemsSourceProperty); }
			set { SetValue(FlowItemsSourceProperty, value); }
		}

		void FlowListViewPropertyChanging (object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == FlowItemsSourceProperty.PropertyName)
			{
				var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
				if (flowItemSource != null)
					flowItemSource.CollectionChanged -= FlowItemSourceCollectionChanged;
			}
		}

		private void FlowListViewPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == FlowItemsSourceProperty.PropertyName)
			{
				var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
				if (flowItemSource != null)
					flowItemSource.CollectionChanged += FlowItemSourceCollectionChanged;

				var containerList = ItemsSource as ObservableList<ObservableList<object>>;
				if (containerList == null)
					throw new NotSupportedException("You cannot change ItemsSource property instance");

				if (FlowColumnsDefinitions == null || FlowColumnsDefinitions.Count == 0 || FlowItemsSource == null)
				{
					containerList.Clear();
					return;
				}

				ReloadContainerList();
			}
		}

		void FlowItemSourceCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			ReloadContainerList();
		}

		private void ReloadContainerList()
		{
			var tempList = new List<ObservableList<object>>();
			var colCount = FlowColumnsDefinitions.Count;
			int position = -1;

			for (int i = 0; i < FlowItemsSource.Count; i++)
			{
				if (i % colCount == 0)
				{
					position++;
					var newContItm = new ObservableList<object>();
					newContItm.Add(FlowItemsSource[i]);
					tempList.Add(newContItm);
				}
				else
				{
					var exContItm = tempList[position];
					exContItm.Add(FlowItemsSource[i]);
					tempList[position] = exContItm;
				}
			}

			var imageSource = ItemsSource as ObservableList<ObservableList<object>>;
			imageSource.Repopulate(tempList);
		}
			
		public void Dispose()
		{
			PropertyChanged -= FlowListViewPropertyChanged;
			PropertyChanging -= FlowListViewPropertyChanging;

			var flowItemSource = FlowItemsSource as INotifyCollectionChanged;
			if (flowItemSource != null)
			{
				flowItemSource.CollectionChanged -= FlowItemSourceCollectionChanged;
			}
		}
	}
}

