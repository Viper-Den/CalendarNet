using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Destiny.Core
{
    public class ObservableCollectionWithItemNotify<T>: ObservableCollection<T>
    {
        public void UpdateItem(T item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(){item}));
        }
    }
}
