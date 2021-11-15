using System;
using Android.Bluetooth;


namespace Plugin.BluetoothLE.Android.Server
{
    public class GattDescriptor : BluetoothLE.Server.AbstractGattDescriptor, IDroidGattDescriptor
    {
        public GattDescriptor(BluetoothLE.Server.IGattCharacteristic characteristic,
                              Guid descriptorUuid,
                              byte[] value) : base(characteristic, descriptorUuid, value)
        {
            this.Native = new BluetoothGattDescriptor(
                descriptorUuid.ToUuid(),
                GattDescriptorPermission.Read // TODO
            );
            this.Native.SetValue(value);
        }


        public BluetoothGattDescriptor Native { get; }
    }
}