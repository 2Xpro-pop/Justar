using Justar.ViewModels;
using Justar.Services;

using System;
using System.Linq;
using System.Diagnostics;

using Xamarin.Forms;

namespace Justar.Views
{
    [QueryProperty(nameof(Student), "student")]
    public partial class ItemDetailPage : ContentPage
    {
        public int Student { get; set; }
        public ItemDetailPage()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetBindingContext();
        }

        private async void SetBindingContext()
        {
            try
            {
                BindingContext = new StudentInfoViewModel(await StudentDatabase.Select(Student));
            }
            catch(Exception exc)
            {
                Debug.WriteLine($"{Student} {exc}");
            }
        }
    }
}