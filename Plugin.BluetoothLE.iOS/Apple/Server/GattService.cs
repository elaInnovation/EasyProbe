using System;
using CoreBluetooth;


namespace Plugin.BluetoothLE.iOS.Server
{
    public class GattService : BluetoothLE.Server.AbstractGattService, IAppleGattService
    {
        readonly CBPeripheralManager manager;
        public CBMutableService Native { get; }


        public GattService(CBPeripheralManager manager,
                           BluetoothLE.Server.IGattServer server,
                           Guid serviceUuid,
                           bool primary) : base(server, serviceUuid, primary)
        {
            this.manager = manager;
            this.Native = new CBMutableService(serviceUuid.ToCBUuid(), primary);
        }


        protected override BluetoothLE.Server.IGattCharacteristic CreateNative(Guid uuid, CharacteristicProperties properties, BluetoothLE.Server.GattPermissions permissions)
            => new GattCharacteristic(this.manager, this, uuid, properties, permissions);
    }
}
