using Destiny.Core;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DestinyNet.ViewModels
{
    public class ToDoViewModel : ViewModeDataBase, IDragSource
    {
        //private DTask _selectedTask;
        public ToDoViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager) {
            Subscribe();
        }
        
        public ObservableCollection<Calendar> Calendars { get => _data.Calendars; }
        public ObservableCollection<DTask> Tasks { get => _data.Tasks; }
        public ICommand AddTaskCommand { get => new ActionCommand(DoAddTaskCommand); }

        private void UnSubscribe()
        {
            foreach (var t in Tasks)
                t.OnRemoveFromParent -= DoRemoveFromParent;
        }
        private void Subscribe()
        {
            foreach (var t in Tasks)
                t.OnRemoveFromParent += DoRemoveFromParent;
        }

        private void DoRemoveFromParent(DTask obj)
        {
            Tasks.Remove(obj);
        }

        private void DoAddTaskCommand(object obj)
        {
            var t = new DTask();
            t.Name = "Test";
            t.OnRemoveFromParent += DoRemoveFromParent;
            _data.Tasks.Add(t);
        }
        public static void SubscribeToTasks(ObservableCollection<DTask> tasks, Dictionary<string, DTask> tasksDictionary)
        {
            foreach (var t in tasks)
            {
                ToDoViewModel.SubscribeToTasks(t.SubTasks, tasksDictionary);
                tasksDictionary.Add(t.GUID, t);
            }
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            
        }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            return true;
        }

        public void Dropped(IDropInfo dropInfo)
        {
            
        }

        public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {

        }

        public void DragCancelled()
        {

        }

        public bool TryCatchOccurredException(Exception exception)
        {
            return true;
        }
    }
}
