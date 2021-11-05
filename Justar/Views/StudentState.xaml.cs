using Justar.Models;
using Justar.Services;

using System;
using System.Diagnostics;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Justar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentState : ContentView
    {

        public static readonly BindableProperty StudentProperty = BindableProperty.Create
            (nameof(Student), typeof(GuidStudent), typeof(StudentState), propertyChanged:PropertChanged);

        public GuidStudent Student
        {
            get => (GuidStudent)GetValue(StudentProperty);
            set => SetValue(StudentProperty, value);
        }

        public StudentState()
        {
            InitializeComponent();
        }

        private static void PropertChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != newValue && newValue != null)
            {
                var self = (StudentState)bindable;
                var val = (GuidStudent)newValue;

                self.fio.Text = val.Fio;
            }
        }

        private void DoubleClick(object sender, EventArgs e)
        {
            checkFirst.IsChecked = true;
            checkSeccond.IsChecked = true;
            checkThird.IsChecked = true;
            checkFourth.IsChecked = true;
        }

        private void CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
        }

        private async void EditClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?student={Student.Guid}", true);
        }
    }
}