using System;
using CoreBluetooth;
using Foundation;


namespace Plugin.BluetoothLE.iOS.Server
{
    public class GattDescriptor : BluetoothLE.Server.AbstractGattDescriptor, IAppleGattDescriptor
    {
        public CBMutableDescriptor Native { get; }


        public GattDescriptor(BluetoothLE.Server.IGattCharacteristic characteristic,
                              Guid descriptorUuid,
                              byte[] value) : base(characteristic, descriptorUuid, value)
        {
            this.Native = new CBMutableDescriptor(descriptorUuid.ToCBUuid(), NSData.FromArray(value));
        }
    }
}
