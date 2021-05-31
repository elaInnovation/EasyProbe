using EasyProbe.Models.Bluetooth;
using EasyProbe.ViewModel.Base;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EasyProbe.ViewModel.Popups
{
    public class ListTagsViewModel : ViewModelBase
    {
        ObservableCollection<TagBluetooth> items = new ObservableCollection<TagBluetooth>();
        TagBluetooth currentItem = null;


        public ListTagsViewModel()
        {
            Items.Clear();
            MessagingCenter.Send<App, string>((App)Application.Current, MessengerKeys.UPDATE_FILTER, "");
            MessagingCenter.Subscribe<App, TagBluetooth>((App)Application.Current, MessengerKeys.UPDATE_TAG, OnUpdate);
        }

        void OnUpdate(App sender, TagBluetooth tagNew)
        {
            bool exist = Items.Any(tag => tag.TagName == tagNew.TagName);
            if (!exist)
                Items.Add(tagNew);
        }

        public ObservableCollection<TagBluetooth> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }
        public TagBluetooth CurrentItem
        {
            get { return currentItem; }
            set { 
                if(currentItem != value)
                {
                    currentItem = value;
                    MessagingCenter.Send<App, string>((App)Application.Current, MessengerKeys.UPDATE_FILTER, value.TagName);
                    SetProperty(ref currentItem, value);
                }
            }
        }
    }
}
