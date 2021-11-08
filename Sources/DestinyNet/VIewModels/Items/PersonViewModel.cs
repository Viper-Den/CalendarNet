using Destiny.Core;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DestinyNet.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        private string _FirstName;
        private string _SecondName;
        private string _ThirdName;
        private DateTime _DateBirthday;
        private string _ImageURL;
        private string _Description;

        public string FirstName
        {
            get => _FirstName;
            set
            {
                _FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string SecondName
        {
            get => _SecondName;
            set
            {
                _SecondName = value;
                OnPropertyChanged(nameof(SecondName));
            }
        }
        public string ThirdName
        {
            get => _ThirdName;
            set
            {
                _ThirdName = value;
                OnPropertyChanged(nameof(ThirdName));
            }
        }
        
        public DateTime DateBirthday
        {
            get => _DateBirthday;
            set
            {
                _DateBirthday = value;
                OnPropertyChanged(nameof(DateBirthday));
            }
        }
        public string ImageURL
        {
            get => _ImageURL;
            set
            {
                _ImageURL = value;
                OnPropertyChanged(nameof(ImageURL));
                OnPropertyChanged(nameof(Image));
            }
        }
        public ImageSource Image
        {
            get {
                if((_ImageURL == null) || (!File.Exists(Path.GetFullPath(_ImageURL))))
                    return null;

                return new BitmapImage(new Uri(Path.GetFullPath(_ImageURL)));  
            }
        }
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }
}
