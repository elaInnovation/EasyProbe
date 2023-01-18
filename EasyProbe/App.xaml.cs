using EasyProbe.ViewModel.Base;
using EasyProbe.Views;
using Plugin.BluetoothLE;
using Plugin.BluetoothLE.iOS;
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

            
            if (Xamarin.Forms.Device.RuntimePlatform.Equals(Xamarin.Forms.Device.iOS))
                CrossBleAdapteriOS.Init();
            MainPage = new NavigationPage(new MainPage());

        }
    }
}
