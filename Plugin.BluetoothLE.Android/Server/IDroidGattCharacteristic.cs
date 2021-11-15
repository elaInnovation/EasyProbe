using System;
using Android.Bluetooth;


namespace Plugin.BluetoothLE.Android.Server
{
    public interface IDroidGattCharacteristic : BluetoothLE.Server.IGattCharacteristic
    {
        BluetoothGattCharacteristic Native { get; }
    }
}