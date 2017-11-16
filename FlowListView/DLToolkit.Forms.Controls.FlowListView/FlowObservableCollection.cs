using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// FlowObservableCollection.
	/// </summary>
	[Helpers.FlowListView.Preserve(AllMembers = true)]
    public class FlowObservableCollection<T> : ObservableCollection<T>
	{
		private bool _disableOnCollectionChanged;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FlowObservableCollection() : base() { }

        /// <summary>
        /// Constructor from items.
        /// </summary>
        public FlowObservableCollection(IEnumerable<T> items) : base(items) { }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">Items.</param>
		public virtual void AddRange(IEnumerable<T> items)
		{
            _disableOnCollectionChanged = true;

			foreach (var item in items)
				Items.Add(item);

            _disableOnCollectionChanged = false;
			NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
		}

        /// <summary>
        /// Repopulate the specified items.
        /// </summary>
        /// <returns>The repopulate.</returns>
        /// <param name="items">Items.</param>
		public virtual void Repopulate(IEnumerable<T> items)
		{
            _disableOnCollectionChanged = true;
			Clear();

			foreach (var item in items)
				Items.Add(item);

            _disableOnCollectionChanged = false;

			NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">Items.</param>
		public virtual void RemoveRange(IEnumerable<T> items)
		{
            _disableOnCollectionChanged = false;

			foreach (var item in items)
			{
				Items.Remove(item);
			}

            _disableOnCollectionChanged = true;

			NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
		}

        internal void OnCollectionChangedSuspend()
		{
            _disableOnCollectionChanged = true;
		}

		internal void OnCollectionChangedResume()
		{
            _disableOnCollectionChanged = false;
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

        internal void OnCollectionChangedCancel()
		{
			_disableOnCollectionChanged = false;
		}

		/// <summary>
		/// Override OnCollectionChanged to Batch process.
		/// </summary>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (!_disableOnCollectionChanged)
			{
                try
                {
                    base.OnCollectionChanged(e);
                }
                catch (NullReferenceException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    //TODO HACK some strange Xamarin.Forms exceptionw when using grouping + fast scroll shortname list  !?
                }                
			}
		}

        internal void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            this.OnCollectionChanged(args);
			this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        internal bool Sync(FlowObservableCollection<T> newItems, bool notify = true)
		{
            return SyncPrivate(this, newItems, notify);
		}

        private static bool SyncPrivate(FlowObservableCollection<T> currentItems, FlowObservableCollection<T> updateItems, bool notify)
		{
            currentItems?.OnCollectionChangedSuspend();

            var itemsAdded = notify ? new List<T>() : null;
            var itemsRemoved = notify ? new List<T>() : null;
            //var itemsMoved = notify ? new List<T>() : null;
            //var itemsMovedIndexes = notify ? new List<Tuple<int, int>>() : null;

            bool structureIsChanged = false;
            bool forceReset = false;

            var flowGroup = updateItems.FirstOrDefault() as FlowGroup;
            if (flowGroup != null)
            {
                var result = updateItems
                    .Join(currentItems, k => ((FlowGroup)(object)k).Key, i => ((FlowGroup)(object)i).Key , (k, i) => i)
                    .ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    var oldGroup = currentItems[i] as FlowGroup;
                    var newGroup = result[i] as FlowGroup;

                    if (oldGroup.Key != newGroup.Key)
                    {
                        forceReset = true;
                        currentItems[i] = (T)(object)newGroup;
                    }
                }
            }

			for (int i = 0; i < updateItems.Count; i++)
			{
				var item = updateItems[i];

				if (currentItems.Count <= i)
				{
					structureIsChanged = true;
					currentItems.Add(item);
                    itemsAdded?.Add(item);
				}
				else
				{
                    var itemList = item as FlowObservableCollection<object>;
					var currentItem = currentItems[i];
					var currentItemList = currentItem as FlowObservableCollection<object>;

					if (itemList != null && currentItemList != null)
					{
                        if (currentItemList.Sync(itemList, false))
						{
							structureIsChanged = true;
						}
					}
					else if (structureIsChanged)
					{
						currentItems[i] = item;
					}
                    else if ((object)item != (object)currentItem)
					{
						structureIsChanged = true;
						currentItems[i] = item;
					}
				}
			}

			while (currentItems.Count > updateItems.Count)
			{
				structureIsChanged = true;
                itemsRemoved?.Add(currentItems[currentItems.Count - 1]);
				currentItems.RemoveAt(currentItems.Count - 1);
			}

            if (forceReset || structureIsChanged)
			{
                if (!forceReset && itemsAdded != null && itemsRemoved == null && itemsAdded.Count < 100)
                {
                    currentItems?.OnCollectionChangedCancel();
                    currentItems?.NotifyCollectionChanged(
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsAdded));
                }
                else if (!forceReset && itemsRemoved != null && itemsAdded == null && itemsRemoved.Count < 100)
                {
                    currentItems?.NotifyCollectionChanged(
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, itemsRemoved));
                }
                else
                {
                    currentItems?.OnCollectionChangedResume();
                }
			}
			else
			{
                currentItems?.OnCollectionChangedCancel();
			}

			return structureIsChanged;
		}
    }
}
