using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Destiny.Core
{
    public class PaletteManager: BaseViewModel, IPalette
    {
        private PaletteSettings _settings;
        private Palette _selectedPalette; 

        public PaletteManager(PaletteSettings settings)
        {
            _settings = settings ?? throw new NullReferenceException(nameof(settings));
            _selectedPalette = _settings.Default;
        }
        public ObservableCollection<Palette> PaletteCollection { get => _settings.PaletteCollection; }
        public Palette DefaultSelectedPalette { get => _settings.Default; }
        public Palette SelectedPalette
        {
            get => _selectedPalette;
            private set { 
                SetField(ref _selectedPalette, value);
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DayNoMonth));
                OnPropertyChanged(nameof(DayOff));
                OnPropertyChanged(nameof(DayFinish));
                OnPropertyChanged(nameof(DayOffFinish));
                OnPropertyChanged(nameof(Selected));
                OnPropertyChanged(nameof(SelectedDefault));
                OnPropertyChanged(nameof(ToDay));
                OnPropertyChanged(nameof(Day));
                OnPropertyChanged(nameof(ViewBorderingMonths));
            }
        }
        public string Name { get => _selectedPalette.Name; }
        public EvementStyle DayNoMonth { get => _selectedPalette.DayNoMonth; }
        public EvementStyle DayOff { get => _selectedPalette.DayOff; }
        public EvementStyle DayFinish { get => _selectedPalette.DayFinish; }
        public EvementStyle DayOffFinish { get => _selectedPalette.DayOffFinish; }
        public EvementStyle Selected { get => _selectedPalette.Selected; }
        public EvementStyle SelectedDefault { get => _selectedPalette.SelectedDefault; }
        public EvementStyle ToDay { get => _selectedPalette.ToDay; }
        public EvementStyle Day { get => _selectedPalette.Day; }
        public Visibility ViewBorderingMonths { get => _selectedPalette.ViewBorderingMonths; }
        public void PaintTitle(ITitleControl control, DateTime date) 
        {
            _selectedPalette.PaintTitle(control, date);
        }
        public void PaintDay(IDayControl control, DateTime date)
        {
            _selectedPalette.PaintDay(control, date);
        }
    }
}
