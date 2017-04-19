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
	/// SmartObservableCollection.
	/// </summary>
	public class SmartObservableCollection<T> : ObservableCollection<T>, ISmartObservableCollection
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public SmartObservableCollection()
				: base()
		{
		}

		/// <summary>
		/// Constructor from items.
		/// </summary>
		public SmartObservableCollection(IEnumerable<T> collection)
				: base(collection)
		{
		}

		/// <summary>
		/// Constructor from items.
		/// </summary>
		public SmartObservableCollection(List<T> list)
				: base(list)
		{
		}

		private bool _isBatch;
		private bool _isBatchChanged;

		/// <summary>
		/// Start Batch (update data).
		/// </summary>
		public void BatchStart()
		{
			_isBatch = true;
			_isBatchChanged = false;
		}

		/// <summary>
		/// End Batch (update data).
		/// </summary>
		public void BatchEnd()
		{
			if (_isBatch && _isBatchChanged)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
				base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}

			_isBatch = false;
			_isBatchChanged = false;
		}

		/// <summary>
		/// Cancel Batch (update data).
		/// </summary>
		public void BatchCancel()
		{
			_isBatch = false;
			_isBatchChanged = false;
		}

		/// <summary>
		/// Override OnCollectionChanged to Batch process.
		/// </summary>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (_isBatch)
			{
				_isBatchChanged = true;
			}
			else
			{
				base.OnCollectionChanged(e);
			}
		}

		/// <summary>
		/// Add many items to list.
		/// </summary>
		public void AddRange(IEnumerable<T> range)
		{
			foreach (var item in range)
			{
				Items.Add(item);
			}

			this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		/// <summary>
		/// Reset data.
		/// </summary>
		public void Reset(IEnumerable<T> range)
		{
			this.Items.Clear();

			AddRange(range);
		}

		/// <summary>
		/// Sync data.
		/// </summary>
		public void Sync(IList<T> newItems)
		{
			Sync(this, newItems.Cast<object>());
		}

		/// <summary>
		/// Sync list to current list.
		/// </summary>
		public static void Sync(IList currentItems, IEnumerable<object> updateItems)
		{
			SyncPrivate(currentItems, updateItems);
		}

		private static bool SyncPrivate(IList currentItems, IEnumerable<object> updateItems)
		{
			var smartOldItems = currentItems as ISmartObservableCollection;
			smartOldItems?.BatchStart();

			bool structureIsChanged = false;
			for (int i = 0; i < updateItems.Count(); i++)
			{
				var item = updateItems.ElementAt(i);

				if (currentItems.Count <= i)
				{
					structureIsChanged = true;
					currentItems.Add(item);
				}
				else
				{
					var itemList = (item as IEnumerable)?.Cast<object>();
					var currentItem = currentItems[i];
					var currentItemList = (currentItem as IList);

					if (itemList != null && currentItemList != null)
					{
						if (SyncPrivate(currentItemList, itemList))
						{
							structureIsChanged = true;
						}
					}
					else if (structureIsChanged)
					{
						currentItems[i] = item;
					}
					else if (item != currentItem)
					{
						structureIsChanged = true;
						currentItems[i] = item;
					}
				}
			}

			while (currentItems.Count > updateItems.Count())
			{
				structureIsChanged = true;
				currentItems.RemoveAt(currentItems.Count - 1);
			}

			if (structureIsChanged)
			{
				smartOldItems?.BatchEnd();
			}
			else
			{
				smartOldItems?.BatchCancel();
			}

			return structureIsChanged;
		}
	}
}
