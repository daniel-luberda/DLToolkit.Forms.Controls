using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DLToolkitControlsSamples
{
	public class Grouping<K, T> : ObservableCollection<T>
	{
		public K Key { get; private set; }
		public int ColumnCount { get; private set; }

		public Grouping(K key)
		{
			Key = key;
		}

		public Grouping(K key, IEnumerable<T> items)
			: this(key)
		{
			foreach (var item in items)
				this.Items.Add(item);
		}

		public Grouping(K key, IEnumerable<T> items, int columnCount)
			: this(key, items)
		{
			ColumnCount = columnCount;
		}
	}
}
