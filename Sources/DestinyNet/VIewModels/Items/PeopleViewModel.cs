using Destiny.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace DestinyNet.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        private Person _person;
        public PersonViewModel(Person person)
        {
            _person = person;
        }
        public string Name { 
            get => _person.Name; 
            set { 
                 _person.Name = value;
                OnPropertyChanged(nameof(Name));
            } 
        }
        public string SecondName
        {
            get => _person.SecondName;
            set
            {
                _person.SecondName = value;
                OnPropertyChanged(nameof(SecondName));
            }
        }
        public ImageSource Image
        {
            get { return  File.Exists(_person.ImageURL) ? new BitmapImage(new Uri(_person.ImageURL)) : null; }
        }
        public string Description
        {
            get => _person.Description;
            set
            {
                _person.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public DateTime Birthday
        {
            get => _person.Birthday;
            set
            {
                _person.Birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
        }
    }


    public class PeopleViewModel: ViewModeDataBase
    {
        private readonly Dictionary<Person, PersonViewModel> _peopleDictionary;
        public PeopleViewModel(Data data, IDialogViewsManager dialogViewsManager) : base(data, dialogViewsManager)
        {
            People = new ObservableCollection<PersonViewModel>();
            _peopleDictionary = new Dictionary<Person, PersonViewModel>();
            _data.People.CollectionChanged += DoCollectionChanged;
            UpdatePeople();
        }
        private void UpdatePeople()
        {
            People.Clear();
            foreach (var p in _data.People)
                AddPerson(p);
        }

        private void AddPerson(Person person)
        {
            var pvm = new PersonViewModel(person);
            People.Add(pvm);
            _peopleDictionary.Add(person, pvm);
        }

        private void DoCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var p in e.NewItems)
                        if((p != null)&&(p is Person person))
                            AddPerson(person);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var p in e.OldItems)
                        if ((p != null) && (p is Person person) && _peopleDictionary.ContainsKey(person))
                        {
                            People.Remove(_peopleDictionary[person]);
                            _peopleDictionary.Remove(person);
                        }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    UpdatePeople();
                    break;
            }
        }

        public ObservableCollection<PersonViewModel> People { get; }
    }
}
