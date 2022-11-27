using System;
using System.Collections.Generic;
using System.Windows.Input;
using MapControls.Core;
using MapControls.MapPoint;
using MapControls.MapStyle;

namespace MapControls.MapText
{
    public class MapTextViewModel : BaseViewModel, IMapPoint
    {
        private MapStyleStatus _status;
        private string _text = "test";
        private double _left;
        private double _top;
        private double _minWidth = 300;
        private double _minHeight = 40;
        private double _Width;
        private double _Height; 
        public Action<object> OnDeleteElement { get; set; }
        public ICommand EditElementCommand { get; private set; }
        public ICommand DeleteElementCommand { get; private set; }
        public MapStyleView Style { get; } = new MapStyleView();
        public string StyleName { get => ""; }
        public MapTextViewModel()
        {
            DeleteElementCommand = new ActionCommand(DoDeleteElementCommand);
            EditElementCommand = new ActionCommand(DoEditElementCommand);
        }
        private void DoEditElementCommand(object obj)
        {
            switch (Status)
            {
                case MapStyleStatus.None:
                    Status = MapStyleStatus.Edited;
                    break;
                default:
                    Status = MapStyleStatus.None;
                    break;
            }
        }

        private void DoDeleteElementCommand(object obj)
        {
            OnDeleteElement?.Invoke(this);
        }
        public string Text { 
            get => _text; 
            set { SetField(ref _text, value, nameof(Text)); }
        }
        public MapStyleStatus Status
        {
            get => _status;
            set { SetField(ref _status, value, nameof(Status)); }
        }
        public double Left
        {
            get => _left;
            set 
            { 
                SetField(ref _left, value, nameof(Left));
                OnPositionChanged?.Invoke(this);
            }
        }
        public double Top
        {
            get => _top;
            set 
            { 
                SetField(ref _top, value, nameof(Top));
                OnPositionChanged?.Invoke(this);
            }
        }
        public double MinWidth
        {
            get => _minWidth;
            set 
            { 
                SetField(ref _minWidth, value, nameof(MinWidth));
                if (Width < _minWidth)
                    Width = _minWidth;
            }
        }
        public double MinHeight
        {
            get => _minHeight;
            set
            {
                SetField(ref _minHeight, value, nameof(MinHeight));
                if (Height < _minHeight)
                    Height = _minHeight;
            }
        }
        public double Width
        {
            get => _Width;
            set  
            {
                SetField(ref _Width, value, nameof(Width));
                OnPositionChanged?.Invoke(this);
            }
        }
        public double Height
        {
            get => _Height;
            set 
            {
                SetField(ref _Height, value, nameof(Height));
                OnPositionChanged?.Invoke(this);
            }
        }

        public Action<IMapPoint> OnPositionChanged { get; set; }
        double IMapPoint.X => Left + (Width / 2);
        double IMapPoint.Y => Top + (Height / 2);
    }
}
