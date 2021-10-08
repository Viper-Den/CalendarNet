using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Destiny.Core
{
    public class DTask: BaseViewModel
    {
        private bool _isEditable;
        public DTask()
        {
            GUID = Guid.NewGuid().ToString();
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
        public ICommand AddTaskCommand { get => new ActionCommand(DoAddTaskCommand); }
        public ICommand DeleteTaskCommand { get => new ActionCommand(DoDeleteTaskCommand); }
        public ICommand EditTaskCommand { get => new ActionCommand(DoEditTaskCommand); }
        public ICommand SaveTaskCommand { get => new ActionCommand(DoSaveTaskCommand); }

        public bool IsEditable { get => _isEditable; set => SetField(ref _isEditable, value); }
        private void DoEditTaskCommand(object obj)
        {
            IsEditable = true;
        }
        private void DoSaveTaskCommand(object obj)
        {
            IsEditable = false;
        }
        private void DoAddTaskCommand(object obj)
        {
            var t = new DTask();
            t.Name = "Test";
            SubTasks.Add(t);
        }
        private void DoDeleteTaskCommand(object obj)
        {
            OnRemoveFromParent?.Invoke(this);
        }
    }
}
