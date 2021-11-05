using Justar.Services;
using Justar.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Justar
{
    public partial class App : Application
    {

        public static readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db");

        public App()
        {
            InitializeComponent();
            StudentDatabase.Init();
            ReportDatabase.Init();
            MainPage = new AppShell();
        }

    }
}
