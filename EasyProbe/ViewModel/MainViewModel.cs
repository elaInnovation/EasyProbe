using EasyProbe.Models.Bluetooth;
using EasyProbe.ViewModel.Base;
using EasyProbe.ViewModel.Popups;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyProbe.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        async Task OnBluetooth_OnAppearing()
        {
            await Task.Delay(1000);
            MessagingCenter.Subscribe<App, TagBluetooth>((App)Application.Current, MessengerKeys.UPDATE_TAG, OnUpdate);
            MessagingCenter.Subscribe<App, string>((App)Application.Current, MessengerKeys.UPDATE_FILTER, OnUpdateFilter);
            BluetoothService.StartScanner();
        }

        void OnUpdate(App sender, TagBluetooth tag)
        {
            TagName = tag.TagName;
            TagTemperature = tag.Temperature;
            TagFarenheit = tag.Farenheit;
        }

        void OnUpdateFilter(App sender, string filter)
        {
            BluetoothService.SetFilter(filter);
            TagBluetooth tag = BluetoothService.GetLastItem(filter);
            if(tag != null)
            {
                TagName = tag.TagName;
                TagTemperature = tag.Temperature;
                TagFarenheit = tag.Farenheit;
            }
        }

        #region Binding commands
        public ICommand SearchTags => new AsyncCommand(OpenListTags);
        public ICommand CommandPicto1 => new AsyncCommand(commandPicto1);
        public ICommand CommandPicto2 => new AsyncCommand(commandPicto2);
        public ICommand CommandPicto3 => new AsyncCommand(commandPicto3);
        public ICommand OpenBasicWebsite => new AsyncCommand(OnOpenBasicWebsite);
        public ICommand Bluetooth_OnAppearing => new AsyncCommand(OnBluetooth_OnAppearing);

        async Task OnOpenBasicWebsite()
        {
            await Browser.OpenAsync(new Uri("https://elainnovation.com/produit/blue-puck-t-probe/"), BrowserLaunchMode.SystemPreferred);
        }

        Task OpenListTags() => NavigationService.NavigateToPopupAsync<ListTagsViewModel>(true);
        
        async Task commandPicto1()
        {
            Picto1 = Color.LightGray;
            await Task.Delay(200);
            Picto1 = Color.White;

            Picto1Page = true;
            Picto2Page = false;
            Picto3Page = false;
        }
        async Task commandPicto2()
        {
            Picto2 = Color.LightGray;
            await Task.Delay(200);
            Picto2 = Color.White;

            Picto1Page = false;
            Picto2Page = true;
            Picto3Page = false;
        }
        async Task commandPicto3()
        {
            Picto3 = Color.LightGray;
            await Task.Delay(200);
            Picto3 = Color.White;

            Picto1Page = false;
            Picto2Page = false;
            Picto3Page = true;
        }
        #endregion

        #region Binding Variables
        private string tagName = String.Empty;
        private float tagTemperature = 0.0f;
        private float tagFaren = 0.0f;
        private Color picto1 = Color.White;
        private Color picto2 = Color.White;
        private Color picto3 = Color.White;
        private bool picto1Page = false;
        private bool picto2Page = true;
        private bool picto3Page = false;
        public string TagName
        {
            get => tagName;
            set => SetProperty(ref tagName, value);
        }
        public float TagTemperature
        {
            get => tagTemperature;
            set => SetProperty(ref tagTemperature, value);
        }

        public float TagFarenheit
        {
            get => tagFaren;
            set => SetProperty(ref tagFaren, value);
        }

        public Color Picto1
        {
            get => picto1;
            set => SetProperty(ref picto1, value);
        }
        public Color Picto2
        {
            get => picto2;
            set => SetProperty(ref picto2, value);
        }
        public Color Picto3
        {
            get => picto3;
            set => SetProperty(ref picto3, value);
        }
        public bool Picto1Page
        {
            get => picto1Page;
            set => SetProperty(ref picto1Page, value);
        }
        public bool Picto2Page
        {
            get => picto2Page;
            set => SetProperty(ref picto2Page, value);
        }
        public bool Picto3Page
        {
            get => picto3Page;
            set => SetProperty(ref picto3Page, value);
        }
        #endregion
    }
}
