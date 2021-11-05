using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Justar.Models;
using Justar.Services;

using Xamarin.Forms;
using Xamarin.Essentials;

namespace Justar.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {

        public ICommand CopyCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public string Text 
        {
            get => text;
            set => SetProperty(ref text, value, nameof(Text));
        }
        string text;

        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value, nameof(Date), async ()=> await SetText());
        }
        DateTime date = DateTime.Today;
        DateTime prev = DateTime.MinValue;

        public AboutViewModel()
        {
            CopyCommand = new Command(Copy);
            SaveCommand = new Command(Save);
            UpdateCommand = new Command(Update);
        }

        public async Task SetText()
        {
            await Task.Run(() => Text = BinaryDatabase.TextInfo(Date, Views.AboutPage.ViewStudents.Absent));

        }

        private async void Copy()
        {
            await Clipboard.SetTextAsync(Text);
        }

        private async void Save()
        {
            await Task.Run(BinaryDatabase.Save);
        }

        private async void Update()
        {
            await SetText();
        }

    }
}