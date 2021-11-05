using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Justar.Models;

namespace Justar.Views
{

    //Новая версия View
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentElement : ViewCell
    {
        //для внутреннего использование
        private static readonly Type type = typeof(StudentElement);

        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(StudentActionState), type, propertyChanged: StateChange);
        public static readonly BindableProperty StudentIdProperty = BindableProperty.Create(nameof(StudentId), typeof(int), type);
        public static readonly BindableProperty FioProperty = BindableProperty.Create(nameof(Fio), typeof(string), type);

        private static readonly Color absentColor = new Color(242, 65, 58);
        private static readonly Color lateColor = new Color(160, 166, 15);

        private static bool isInfoStyle = false;
        private static List<StudentState> allViews = new List<StudentState>();
        Rectangle rect;

        public StudentActionState State
        {
            get => (StudentActionState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public int StudentId
        {
            get => (int)GetValue(StudentIdProperty);
            set => SetValue(StudentIdProperty, value);
        }

        public string Fio
        {
            get => (string)GetValue(FioProperty);
            set => SetValue(FioProperty, value);
        }

        public StudentElement()
        {
            InitializeComponent();
        }

        private static void StateChange(BindableObject bindable, object oldValue, object newValue)
        {
            if(oldValue != newValue)
            {
                var state = (StudentActionState)newValue;
                var self = (StudentElement)bindable;

                if (state == StudentActionState.Present)
                {
                    self.check.IsChecked = true;
                    self.check.Color = absentColor;
                }
                else if (state == StudentActionState.Absent)
                {
                    self.check.IsChecked = false;
                    self.check.Color = absentColor;
                }
                else
                {
                    self.check.IsChecked = true;
                    self.check.Color = lateColor;
                }
            }
        }

        private async void Once(object sender, EventArgs e)
        {
            if (isInfoStyle)
            {
                await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?student={StudentId}");
                return;
            }
            if (State != StudentActionState.Absent)
            {
                State = StudentActionState.Absent;
            }
            else
            {
                State = StudentActionState.Present;
            }
        }

        private async void Double(object sender, EventArgs e)
        {
            if (isInfoStyle)
            {
                await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?student={StudentId}");
                return;
            }
            if (State != StudentActionState.Late)
                State = StudentActionState.Late;
            else
                State = StudentActionState.Absent;
        }

        private void ChakedCnage(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                State = StudentActionState.Present;
            }
            else
            {
                State = StudentActionState.Absent;
            }
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