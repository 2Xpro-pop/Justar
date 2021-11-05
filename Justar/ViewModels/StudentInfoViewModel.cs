using Justar.Models;
using Justar.Services;

using System.Windows.Input;

using Xamarin.Forms;

namespace Justar.ViewModels
{
    public class StudentInfoViewModel : BaseViewModel
    {
        private GuidStudent student;
        public string Id
        {
            get => student.Guid.ToString();
        }

        public string Fio
        {
            get => student.Fio;
            set => SetProperty(fio => student.Fio = fio, value, nameof(Fio));
        }

        public string Title
        {
            get => $"Студент номер {Id}";
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public StudentInfoViewModel(GuidStudent student)
        {
            this.student = student;
            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);
        }

        private async void Delete()
        {
            BinaryDatabase.DeleteStudent(student.Guid);
            await Shell.Current.GoToAsync("..");
        }

        private async void Save()
        {
            BinaryDatabase.UpdateFio(student.Guid, student.Fio);
            await Shell.Current.GoToAsync("..");
        }

    }
}
