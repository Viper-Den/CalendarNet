using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Destiny.Core;

namespace DestinyNet
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
        public SettingsViewModel(ICommand closeWindowCommand, PaletteManager paletteManager)
        {
            CloseCommand = closeWindowCommand ?? throw new NullReferenceException(nameof(CloseCommand));
            Nodes = new ObservableCollection<Node>()
            {
                new Node( "General", null),
                new Node( "Presets palette", null),
                new Node( "Weather", null)
            };
            paletteManager.PaletteCollection.Add(new Palette() { Name = "White" });
            paletteManager.PaletteCollection.Add(new Palette() { Name = "Black" });
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
