using System;
using Android.Bluetooth;


namespace Plugin.BluetoothLE.Android.Server
{
    public interface IDroidGattDescriptor : BluetoothLE.Server.IGattDescriptor
    {
        BluetoothGattDescriptor Native { get; }
    }
}