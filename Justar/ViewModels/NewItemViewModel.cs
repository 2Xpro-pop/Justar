using Justar.Models;
using Justar.Services;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Xamarin.Forms;

namespace Justar.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string fio;
        private string email;
        private string phone;

        private Func<string, string, string, Task> display;

        public NewItemViewModel(Func<string, string, string, Task> display)
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
            this.display = display;
        }

        [Required(ErrorMessage = "Фио не должно быть пустым")]
        [RegularExpression(@"^([^0-9]*)$", ErrorMessage = "Фио не должно содержать цифры")]
        public string Fio
        {
            get => fio;
            set => SetProperty(ref fio, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {

            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            if (!Validator.TryValidateObject(this, context, results, true))
            {
                foreach (var error in results)
                {
                    await display?.Invoke("Ошибка ввода данных", error.ErrorMessage, "ок");
                    return;
                }
            }

            await StudentDatabase.Insert(new Student { Fio = Fio, Email= Email, Phone = Phone});

            await Shell.Current.GoToAsync("..");
        }
    }
}
