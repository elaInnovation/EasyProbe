using System;
using Android.Bluetooth;
using Java.Util;
using Plugin.BluetoothL.AndroidE.Server;
using Plugin.BluetoothLE.Android.Server.Internals;
using Plugin.BluetoothLE.Server;

namespace Plugin.BluetoothLE.Android.Server
{
    public class GattService : BluetoothLE.Server.AbstractGattService, IDroidGattService
    {
        public BluetoothGattService Native { get; }
        readonly GattContext context;


        public GattService(GattContext context,
                           IGattServer server,
                           Guid uuid,
                           bool primary) : base(server, uuid, primary)
        {
            this.context = context;
            this.Native = new BluetoothGattService(
                UUID.FromString(uuid.ToString()),
                primary ? GattServiceType.Primary : GattServiceType.Secondary
            );
        }


        protected override BluetoothLE.Server.IGattCharacteristic CreateNative(Guid uuid, CharacteristicProperties properties, GattPermissions permissions)
        {
            var ch = new GattCharacteristic(this.context, this, uuid, properties, permissions);
            this.Native.AddCharacteristic(ch.Native);
            return ch;
        }
    }
}
