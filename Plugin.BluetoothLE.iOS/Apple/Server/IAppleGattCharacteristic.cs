using System;
using CoreBluetooth;


namespace Plugin.BluetoothLE.iOS.Server
{
    public interface IAppleGattCharacteristic : BluetoothLE.Server.IGattCharacteristic
    {
        CBMutableCharacteristic Native { get; }
    }
}
