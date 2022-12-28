using System.Windows.Input;
using System.Windows;
using MapControls;
using System.Collections.ObjectModel;
using ZoomPanel;
using System;
using ServiceLocation;
using MapControls.Core;
using MapControls.MapPositionValidator;
using MapControls.MapLine;
using MapControls.MapText;
using Microsoft.Win32;

namespace WpfApp4
{
    public class MainWindowB2ViewModel : BaseViewModel
    {
        private double _viewOffsetX;
        private double _viewOffsetY;
        private double _viewWidth;
        private double _viewHeight;
        private ScaleParams _scale = new ScaleParams();
        public double ViewOffsetX { get => _viewOffsetX; set { SetField(ref _viewOffsetX, value); } }
        public double ViewOffsetY { get => _viewOffsetY; set { SetField(ref _viewOffsetY, value); } }
        public ScaleParams Scale  { get => _scale; set { SetField(ref _scale, value); } }
        public double ViewWidth { get => _viewWidth; set { SetField(ref _viewWidth, value); } }
        public double ViewHeight { get => _viewHeight; set { SetField(ref _viewHeight, value); } }
        public double ContentWidth { get; private set; }
        public double ContentHeight { get; private set; }
        public object SelectedMapElement { private get; set; }
        public ICommand AddControllTextCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public IServiceLocator ServiceLocator { get; private set; }
        public IPositionValidator PositionValidator { get;private set; } = new PositionValidator();
        public ObservableCollection<Object> Controls { get; } = new ObservableCollection<Object>();

        public MainWindowB2ViewModel()
        {
            ServiceLocator = new MapElementServiceLocator();
            ContentWidth = 5000;
            ContentHeight = 5000;
            ViewOffsetX = -(ContentWidth / 2);
            ViewOffsetY = -(ContentHeight / 2);

            AddControllTextCommand = new ActionCommand(DoAddControllText);
            LoadCommand = new ActionCommand(DoLoadCommand);
            SaveCommand = new ActionCommand(DoSaveCommand);


            AddElement(false);
        }

        private void DoLoadCommand(object o)
        {
            Load();
        }

        private void DoSaveCommand(object o)
        {
            Save();
        }

        private void Load()
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = MapFileFactory.File_Extension;
                openFileDialog.Filter = MapFileFactory.File_Filter;

                if (!(bool)openFileDialog.ShowDialog())
                    return;

                var data = MapFileFactory.LoadSettingsFromJsonFile(openFileDialog.FileName);

                Controls.Clear();
                SelectedMapElement = null;

                foreach (var element in data.Elements)
                {
                    var mapTextViewModel = new MapTextViewModel()
                    {
                        Left = element.Left,
                        Top = element.Top,
                        Width = element.Width,
                        Height = element.Height,
                        Text = element.Text
                    };
                    mapTextViewModel.OnDeleteElement += DoDeleteElement;
                    Controls.Add(mapTextViewModel);
                    if (Controls.Count > 1)
                    {
                        var mapLinesViewModel = new MapLinesViewModel();
                        mapLinesViewModel.Start = ((MapTextViewModel)Controls[0]);
                        mapLinesViewModel.Finish = ((MapTextViewModel)Controls[Controls.Count - 1]);
                        Controls.Add(mapLinesViewModel);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private void Save()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = MapFileFactory.File_Extension;
                saveFileDialog.Filter = MapFileFactory.File_Filter;

                if (!(bool)saveFileDialog.ShowDialog())
                    return;

                var data = new MapFile();
                data.DateOfCreating = DateTime.Now;
                foreach (var control in Controls) 
                    if(control is MapTextViewModel element)
                    data.Elements.Add(new MapElement() { Top = element.Top, Left = element.Left, Width = element.Width, Height = element.Height, Style = element.StyleName, Text = element.Text });

                MapFileFactory.SaveSettingsToJsonFile(data, saveFileDialog.FileName);
            }
            catch (Exception e)
            {

            }
        }

        private void DoAddControllText(object o)
        {
            AddElement();
        }

        private void AddElement(bool addBinding = true)
        {
            var mapTextViewModel = new MapTextViewModel();
            mapTextViewModel.OnDeleteElement += DoDeleteElement;
            mapTextViewModel.Left = (ContentWidth - mapTextViewModel.Width) / 2;
            mapTextViewModel.Top = (ContentHeight - mapTextViewModel.Height) / 2;
            Controls.Add(mapTextViewModel);

            if (addBinding)
            {
                var mapLinesViewModel = new MapLinesViewModel();
                mapLinesViewModel.Start = ((MapTextViewModel)Controls[0]);
                mapLinesViewModel.Finish = ((MapTextViewModel)Controls[Controls.Count - 1]);
                Controls.Add(mapLinesViewModel);
            }
        }

        private void DoDeleteElement(object o)
        {
            if (Controls.Contains(o))
                Controls.Remove(o);
        }

    }
}
