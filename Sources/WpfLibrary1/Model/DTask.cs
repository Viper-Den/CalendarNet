using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Destiny.Core
{
    public class DTask
    {
        public DTask()
        {
            SubTasks = new ObservableCollection<DTask>();
            SubTasks.CollectionChanged += OnCollectionChanged;
        }
        ~DTask()
        {
            SubTasks.CollectionChanged -= OnCollectionChanged;
            UnSubscribe();
        }

        public string Name { get; set; }
        public string GUID { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public Calendar Calendar { get; set; }
        public Action<DTask> OnRemoveFromParent { get; set; }
        public ObservableCollection<DTask> SubTasks { get; }
        public void RemoveFromParent()
        {
            OnRemoveFromParent?.Invoke(this);
        }
        private void DoRemoveFromParent(DTask task)
        {
            SubTasks.Remove(task);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var o in e.NewItems)
                        ((DTask)o).OnRemoveFromParent += DoRemoveFromParent;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var o in e.OldItems)
                        ((DTask)o).OnRemoveFromParent -= DoRemoveFromParent;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    UnSubscribe();
                    break;
            }
        }
        private void UnSubscribe()
        {
            foreach (var t in SubTasks)
                t.OnRemoveFromParent -= DoRemoveFromParent;
        }
        private void Subscribe()
        {
            foreach (var t in SubTasks)
                t.OnRemoveFromParent += DoRemoveFromParent;
        }
    }
}
