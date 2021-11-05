﻿using Justar.Models;
using Justar.Views;
using Justar.Services;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Linq;

namespace Justar.ViewModels
{
    public class StudentsViewModel : BaseViewModel
    {
        public ObservableCollection<GuidStudent> Items
        {
            get => students;
            set => SetProperty(ref students, value, nameof(Items));
        }
        ObservableCollection<GuidStudent> students;
        public Command AddItemCommand { get; }
        public Command RefreshCommand { get; set; }
        public Command Info { get; set; }
        public Command<string> SearchCommand { get; set; }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value, nameof(IsRefreshing));
        }
        bool isRefreshing = true;

        public StudentsViewModel()
        {
            AddItemCommand = new Command(OnAddItem);

            RefreshCommand = new Command(SetDefaultItems);

            SearchCommand = new Command<string>(Search);

            SetDefaultItems();
        }

        async void SetDefaultItems()
        {
            await Task.Delay(1);
            OnPropertyChanged(nameof(Items));
            Items = new ObservableCollection<GuidStudent>(BinaryDatabase.GetStudents());
            IsRefreshing = false;
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        private void Search(string text)
        {
            text = Regex.Replace(text.ToLower().Trim(), @"\s+", " ");
            Items =new ObservableCollection<GuidStudent>(BinaryDatabase.GetStudents().Where(std => Middleware(std.Fio, text)));
        }

        private bool Middleware(string fio, string search)
        {
            return fio.ToLower().Contains(search);
        }

    }
}