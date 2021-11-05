using Justar.Models;

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
            (nameof(Student), typeof(Student), typeof(StudentState), propertyChanged:PropertChanged);

        static bool isInfoStyle = false;
        static List<StudentState> allViews = new List<StudentState>();
        Rectangle rect;

        public static void ChangeStyle()
        {
            isInfoStyle = !isInfoStyle;
            foreach(var view  in allViews)
            {
                view.Anim();
            }
        }
        protected override void OnSizeAllocated(double x, double y)
        {
            base.OnSizeAllocated(x, y);
            rect = check.Bounds;
            isInfoStyle = false;
            Anim();
        }

        public Student Student
        {
            get => (Student)GetValue(StudentProperty);
            set => SetValue(StudentProperty, value);
        }

        public StudentState()
        {
            InitializeComponent();
            allViews.Add(this);
        }

        private static void PropertChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != newValue && newValue != null)
            {
                var self = (StudentState)bindable;
                var val = (Student)newValue;

                if(val.State == StudentActionState.Present)
                {
                    self.check.IsChecked = true;
                    self.check.Color = (Color)self.Resources["Absent"];
                }
                else if (val.State == StudentActionState.Absent)
                {
                    self.check.IsChecked = false;
                    self.check.Color = (Color)self.Resources["Absent"];
                }
                else
                {
                    self.check.IsChecked = true;
                    self.check.Color = (Color)self.Resources["Late"];
                }

                self.fio.Text = val.Fio;
            }
        }

        private async void Once(object sender, EventArgs e)
        {
            if (isInfoStyle)
            {
                await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?student={Student.Id}");
                return;
            }
            if (Student.State != StudentActionState.Absent)
            {
                Student.State = StudentActionState.Absent;
            }
            else
            {
                Student.State = StudentActionState.Present;
            }
            PropertChanged(this, null, Student);
        }

        private async void Double(object sender, EventArgs e)
        {
            if(isInfoStyle)
            {
                await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?student={Student.Id}");
                return;
            }
            if (Student.State != StudentActionState.Late)
                Student.State = StudentActionState.Late;
            else
                Student.State = StudentActionState.Absent;
            PropertChanged(this, null, Student);
        }

        private void ChakedCnage(object sender, CheckedChangedEventArgs e)
        {
            if(e.Value)
            {
                Student.State = StudentActionState.Present;
            }
            else
            {
                Student.State = StudentActionState.Absent;
            }
            PropertChanged(this, null, Student);
        }

        private async void Anim()
        {
            if (isInfoStyle)
            {
                await check.LayoutTo(new Rectangle(rect.X, rect.Y, 0, rect.Height), 450, Easing.SinInOut);
            }
            else
            {
                await check.LayoutTo(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), 450, Easing.SinInOut);
            }
        }
    }
}