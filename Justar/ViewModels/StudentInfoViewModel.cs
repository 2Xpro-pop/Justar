using Justar.Models;
using Justar.Services;

using System.Windows.Input;

using Xamarin.Forms;

namespace Justar.ViewModels
{
    public class StudentInfoViewModel : BaseViewModel
    {
        private Student student;
        public int Id
        {
            get => student.Id;
        }

        public string Fio
        {
            get => student.Fio;
            set => SetProperty(fio => student.Fio = fio, value, nameof(Fio));
        }

        public string Email
        {
            get => student.Email;
            set => SetProperty(email => student.Email = email, value, nameof(Email));
        }

        public string Phone
        {
            get => student.Phone;
            set => SetProperty(phone => student.Phone = phone, value, nameof(Phone));
        }

        public string Title
        {
            get => $"Студент номер {Id}";
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public StudentInfoViewModel(Student student)
        {
            this.student = student;
            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);
        }

        private async void Delete()
        {
            await ReportDatabase.DeleteStudent(student);
            await StudentDatabase.Delete(student);
            await Shell.Current.GoToAsync("..");
        }

        private async void Save()
        {
            await StudentDatabase.Update(student);
            await Shell.Current.GoToAsync("..");
        }

    }
}
