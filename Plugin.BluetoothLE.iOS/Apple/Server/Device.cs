using System;
using CoreBluetooth;


namespace Plugin.BluetoothLE.iOS.Server
{
    public class Device : BluetoothLE.Server.IDevice
    {
        public Device(CBCentral central)
        {
            this.Central = central;
            this.Uuid = new Guid(central.Identifier.ToString());
        }


        public Guid Uuid { get; }
        public CBCentral Central { get; }
        public object Context { get; set; }
    }
}
