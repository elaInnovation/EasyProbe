using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EasyProbe.Models.Bluetooth
{
    public class TagBluetooth: BindableObject
    {
        public IScanResult RawData = null;
        private string tagName = String.Empty;
        public float Temperature = 0.0f;
        public float Farenheit = 0.0f;
        public int Rssi = 0;

        public string TagName
        {
            get => tagName;
            set
            {
                tagName = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return "Name :" + TagName + "\n" +
                "Rssi :" + Rssi + "\n" +
                "Temperature :" + Temperature.ToString() + "\n" +
                "Farenheit :" + Farenheit.ToString() + "\n";
        }
    }
}
