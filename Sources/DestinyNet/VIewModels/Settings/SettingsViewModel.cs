using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet.ViewModels
{
    public class Node
    {
        public Node(string name, BaseViewModel viewModel)
        {
            Name = name;
            ViewModel = viewModel;
            Nodes = new ObservableCollection<Node>();
        }
        public string Name { get; }
        public ObservableCollection<Node> Nodes { get;  }
        public BaseViewModel ViewModel { get; }
    }
    public class SettingsViewModel: BaseViewModel
    {
        private Node _selectedNode;
        private Settings _settings;
        public SettingsViewModel(ICommand closeWindowCommand, PaletteManager paletteManager, Settings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            CloseCommand = closeWindowCommand ?? throw new NullReferenceException(nameof(CloseCommand));
            Nodes = new ObservableCollection<Node>()
            {
                new Node( "General", new GeneralSettingsViewModel(_settings.WindowSettings)),
                new Node( "Presets palette", null),
                new Node( "Weather", new WeatherSettingsViewModel(_settings.WeatherSettings))
            };
            foreach (var p in paletteManager.PaletteCollection)
                Nodes[1].Nodes.Add(new Node(p.Name, new PaletteViewModel(p)));
        }
        public ObservableCollection<Node> Nodes { get; }
        public ICommand CloseCommand { get; }
        public ICommand SelectedNodeCommand { get => new ActionCommand(DoSelectedNodeCommand); }
        
        private void DoSelectedNodeCommand(Object o)
        {
            if ((o == null)||(!(o is Node)))
                return;
            SelectedNode = (Node)o;
        }
        public Node SelectedNode { 
            get => _selectedNode; 
            set { 
                    SetField(ref _selectedNode, value);
                    OnPropertyChanged(nameof(SettingsContent));
                } 
            }
        public BaseViewModel SettingsContent { get => SelectedNode == null  ? null : SelectedNode.ViewModel; }
    }
}
