using System;
using CoreBluetooth;


namespace Plugin.BluetoothLE.iOS.Server
{
    public interface IAppleGattService : BluetoothLE.Server.IGattService
    {
        CBMutableService Native { get; }
    }
}
