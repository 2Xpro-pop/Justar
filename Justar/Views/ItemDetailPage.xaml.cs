using Justar.ViewModels;
using Justar.Services;

using System;
using System.Linq;
using System.Diagnostics;

using Xamarin.Forms;

namespace Justar.Views
{
    [QueryProperty(nameof(GuidStudent), "student")]
    public partial class ItemDetailPage : ContentPage
    {
        public string GuidStudent { get; set; }
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
                BindingContext = new StudentInfoViewModel(BinaryDatabase.GetStudent(Guid.Parse(GuidStudent)));
            }
            catch(Exception exc)
            {
                Debug.WriteLine($"{GuidStudent} {exc}");
            }
        }
    }
}