using System;
using Android.Bluetooth;
using Plugin.BluetoothLE.Server;

namespace Plugin.BluetoothL.AndroidE.Server
{
    public interface IDroidGattService : IGattService
    {
        BluetoothGattService Native { get; }
    }
}