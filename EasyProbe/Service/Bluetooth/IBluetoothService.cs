using EasyProbe.Models.Bluetooth;
using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyProbe.Service.Bluetooth
{
    public interface IBluetoothService
    {
        void SetFilter(string filter);
        bool CheckBluetoothState();
        void StartScanner();
        void StopScanner();
        object GetPropValue(object src, string propName);
        TagBluetooth GetLastItem(string tagName);
    }

}
