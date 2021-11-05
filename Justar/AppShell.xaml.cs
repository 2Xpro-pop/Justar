using Justar.ViewModels;
using Justar.Views;
using Justar.Services;

using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Justar
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
