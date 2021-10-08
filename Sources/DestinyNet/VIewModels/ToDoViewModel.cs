using Destiny.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DestinyNet
{
    public class ToDoViewModel : ViewModeDataBase
    {
        //private DTask _selectedTask;
        public ToDoViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager) {
        }
        
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
        public ObservableCollection<DTask> Tasks { get => _data.Tasks; }
        public ICommand AddTaskCommand { get => new ActionCommand(DoAddTaskCommand); }
        //public ICommand SelectedTaskCommand { get => new ActionCommand(DoSelectedTaskCommand); }
        
        private void DoAddTaskCommand(object obj)
        {
            var t = new DTask();
            t.Name = "Test";
            _data.Tasks.Add(t);
        }
        //private void DoSelectedTaskCommand(object obj)
        //{
        //    _selectedTask = obj as DTask;
        //}
        //public DTask SelectedTask { get => _selectedTask; }
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
