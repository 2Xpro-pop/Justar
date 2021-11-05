using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Justar.Views
{
    public partial class AboutPage : ContentPage
    {
        ViewModels.AboutViewModel viewModel;
        public AboutPage()
        {
            InitializeComponent();
            viewModel = new ViewModels.AboutViewModel();
            BindingContext = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.SetText();
        }
    }
}