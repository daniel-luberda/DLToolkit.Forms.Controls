using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DLToolkit.Forms.Controls
{
	public class SmartObservableCollection<T> : ObservableCollection<T>
	{
		public SmartObservableCollection()
				: base()
		{
		}

		public SmartObservableCollection(IEnumerable<T> collection)
				: base(collection)
		{
		}

		public SmartObservableCollection(List<T> list)
				: base(list)
		{
		}

		private bool _isBatch;
		private bool _isBatchChanged;

		public void BatchStart()
		{
			_isBatch = true;
			_isBatchChanged = false;
		}

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

		public void Reset(IEnumerable<T> range)
		{
			this.Items.Clear();

			AddRange(range);
		}
	}
}
