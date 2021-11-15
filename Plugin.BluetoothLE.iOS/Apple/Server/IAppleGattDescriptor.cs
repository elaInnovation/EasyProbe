using System;
using CoreBluetooth;


namespace Plugin.BluetoothLE.iOS.Server
{
    public interface IAppleGattDescriptor : BluetoothLE.Server.IGattDescriptor
    {
        CBMutableDescriptor Native { get; }
    }
}
