using System;
using Android.Bluetooth;
using Plugin.BluetoothLE.Android.Internals;

namespace Plugin.BluetoothLE.Android.Internals
{
    public class ConnectionStateEventArgs : GattEventArgs
    {
        public ProfileState NewState { get; }


        public ConnectionStateEventArgs(BluetoothGatt gatt, GattStatus status, ProfileState newState) : base(gatt, status)
        {
            this.NewState = newState;
        }
    }
}