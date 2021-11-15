using System;
using Android.Bluetooth;


namespace Plugin.BluetoothLE.Android.Server.Internals
{
    public class DescriptorReadEventArgs : GattRequestEventArgs
    {
        public DescriptorReadEventArgs(
            BluetoothGattDescriptor descriptor,
            BluetoothDevice device,
            int requestId,
            int offset) : base(device, requestId, offset)
        {
            this.Descriptor = descriptor;
        }


        public BluetoothGattDescriptor Descriptor { get; }
    }
}