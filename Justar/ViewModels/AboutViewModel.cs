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

        private List<Report> reports = new List<Report>();

        public AboutViewModel()
        {
            CopyCommand = new Command(Copy);
            SaveCommand = new Command(Save);
            UpdateCommand = new Command(Update);
        }

        public async Task SetText()
        {
            reports = await ReportDatabase.SelectReports(Date);
            Debug.WriteLine($"Вызван SetText! {reports.Count}");
            if(reports.Count > 0 && Date != prev)
            {
                Text = string.Join("\n", reports.Where(f => f.StudentState != StudentActionState.Present).Select(conv => conv.Action));
                prev = Date;
                ReportDatabase.UpdateStudents(reports);
            }
            else
            {
                var std = StudentDatabase.Items.ToArray();
                reports = std.Select(conv => conv.MakeReport(Date)).ToList();
                Text = string.Join("\n", std.Select(conv => conv.MakeReport(Date).Action));
            }

        }

        private async void Copy()
        {
            await Clipboard.SetTextAsync(Text);
        }

        private async void Save()
        {
            await ReportDatabase.InsertDate(reports);
        }

        private async void Update()
        {
            await SetText();
        }

    }
}