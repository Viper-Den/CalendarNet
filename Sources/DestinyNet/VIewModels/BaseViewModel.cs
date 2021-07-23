using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DestinyNet
{
    public interface IViewModel
    {
        Action UpdateData { get; set; }
    }

    public class BaseViewModel : IViewModel, INotifyPropertyChanged
    { 
        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetField<T>(ref T field, T value, [CallerMemberName] string caller = null)
        {
            if (field == null && value == null)
            {
                return false;
            }

            if (field != null && field.Equals(value))
            {
                return false;
            }

            field = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public void OnPropertyChanged(object sender, [CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(caller));
        }
        public Action UpdateData { get; set; }
    }
}
