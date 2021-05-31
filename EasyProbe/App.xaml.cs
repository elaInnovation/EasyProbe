using EasyProbe.ViewModel.Base;
using EasyProbe.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyProbe
{
    public partial class App : Application
    {
        static App()
        {
            BuildDependencies();
        }
        public static void BuildDependencies()
        {
            Locator.Instance.Build();
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
