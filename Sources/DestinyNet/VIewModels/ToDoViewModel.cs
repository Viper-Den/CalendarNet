using Destiny.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DestinyNet
{
    public class ToDoViewModel : ViewModeDataBase
    {
        private DTask _selectedTask;
        public ToDoViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager) {
        }
        
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
        public ObservableCollection<DTask> Tasks { get => _data.Tasks; }
        public ICommand AddTaskCommand { get => new ActionCommand(DoAddTaskCommand); }
        public ICommand DeleteTaskCommand { get => new ActionCommand(DoDeleteTaskCommand); }
        public ICommand SelectedTaskCommand { get => new ActionCommand(DoSelectedTaskCommand); }
        private void DoAddTaskCommand(object obj)
        {
            var t = new DTask();
            t.Name = "Test";
            if(_selectedTask == null)
                _data.Tasks.Add(t);
            else
                _selectedTask.SubTasks.Add(t);
        }
        private void DoDeleteTaskCommand(object obj)
        {
            if (_selectedTask == null)
                return;

            _selectedTask.RemoveFromParent();
            if (Tasks.Contains(_selectedTask))
                Tasks.Remove(_selectedTask);
            _selectedTask = null;
        }
        private void DoSelectedTaskCommand(object obj)
        {
            _selectedTask = obj as DTask;
        }
        public static void SubscribeToTasks(ObservableCollection<DTask> tasks, Dictionary<string, DTask> tasksDictionary)
        {
            foreach (var t in tasks)
            {
                ToDoViewModel.SubscribeToTasks(t.SubTasks, tasksDictionary);
                tasksDictionary.Add(t.GUID, t);
            }
        }
    }
}
