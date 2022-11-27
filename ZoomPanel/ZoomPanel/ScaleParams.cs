using ZoomPanel.Common;

namespace ZoomPanel
{
    public class ScaleParams: BaseViewModel
    {
        private double _Min = 0.1;
        private double _Max = 3;
        private double _Step = 0.1;
        private double _Value = 1;
        public double Min { get => _Min; set { SetField(ref _Min, value); } }
        public double Max { get => _Max; set { SetField(ref _Max, value); } }
        public double Step { get => _Step; set { SetField(ref _Step, value); } }
        public double Value { get => _Value; set { SetField(ref _Value, value); } }
    }
}
