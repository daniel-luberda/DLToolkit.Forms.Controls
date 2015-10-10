using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DLToolkit.Forms.Controls
{
	internal class ObservableList<T> : ObservableCollection<T> 
	{

		public ObservableList() 
		{ 
		}

		public ObservableList(IEnumerable<T> items) : base(items) 
		{ 
		} 

		public virtual void AddRange(IEnumerable<T> items) 
		{
			foreach (var item in items)
			{
				Items.Add(item);
			}

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
		}


		public virtual void Repopulate(IEnumerable<T> items) 
		{
			Clear();

			foreach (var item in items)
			{
				Items.Add(item);
			}
				
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}

