using Destiny.Core;
using Microsoft.Win32;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;

namespace DestinyNet.ViewModels.Dialog
{
    public class PersonRedactorViewModel: BaseViewModel
    {
        private  ObservableCollection<PersonViewModel> _people;
        public static PersonRedactorViewModel EditPerson(ICommand closeWindowCommand, PersonViewModel person, ObservableCollection<PersonViewModel> people)
        {
            var redactor = new PersonRedactorViewModel(closeWindowCommand, person, people);
            redactor.IsEditable = true;
            return redactor;
        }
        public static PersonRedactorViewModel AddPerson(ICommand closeWindowCommand, ObservableCollection<PersonViewModel> people)
        {
            var redactor = new PersonRedactorViewModel(closeWindowCommand, new PersonViewModel(), people);
            redactor.IsEditable = false;
            return redactor;
        }
        public PersonRedactorViewModel(ICommand closeWindowCommand, PersonViewModel person, ObservableCollection<PersonViewModel> people)
        {
            _people = people;
            Person = person;
            CloseWindowCommand = closeWindowCommand;
        }
        public PersonViewModel Person { get; }
        public bool IsEditable { get; set; }
        public ICommand LoadImageCommad { get => new ActionCommand(DoLoadImageCommad); }
        public ICommand AddPersonCommad { get => new ActionCommand(DoAddPersonCommad); }
        public ICommand DeletePersonCommad { get => new ActionCommand(DoDeletePersonCommad); }
        public ICommand CloseWindowCommand { get; }

        private void DoAddPersonCommad(object obj)
        {
            if (!_people.Contains(Person))
                _people.Add(Person);
            CloseWindowCommand.Execute(null);
        }
        private void DoDeletePersonCommad(object obj)
        {
            if (_people.Contains(Person))
                _people.Remove(Person);
            CloseWindowCommand.Execute(null);
        }

        private void DoLoadImageCommad(object obj)
        {
            var ofd = new OpenFileDialog() { Filter = "All files|*|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg" };
            if (ofd.ShowDialog() == false) return;
            if ((Path.GetExtension(ofd.FileName) == ".jpeg") ||
                (Path.GetExtension(ofd.FileName) == ".png") ||
                (Path.GetExtension(ofd.FileName) == ".jpg"))
            {
                var ImageDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Images\\People\\";
                Directory.CreateDirectory(ImageDir);

                if (!File.Exists(ImageDir + Path.GetFileName(ofd.FileName))) 
                    File.Copy(ofd.FileName, ImageDir + Path.GetFileName(ofd.FileName));

                string relativePath = System.IO.Path.GetRelativePath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), ImageDir);
                Person.ImageURL = relativePath + Path.GetFileName(ofd.FileName);
            }
        }
    }
}
