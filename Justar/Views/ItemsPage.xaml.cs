using Justar.Models;
using Justar.ViewModels;
using Justar.Services;
using Justar.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Justar.Views
{
    public partial class ItemsPage : ContentPage
    {
        StudentsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new StudentsViewModel();

            refresh.IsRefreshing = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(e.OldTextValue!=e.NewTextValue)
            {
                if(e.NewTextValue.Length == 0)
                {
                    _viewModel.Items = StudentDatabase.Items;
                }
            }
        }
    }
}