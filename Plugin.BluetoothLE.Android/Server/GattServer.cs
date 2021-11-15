﻿using Plugin.BluetoothL.AndroidE.Server;
using Plugin.BluetoothLE.Android.Server.Internals;
using Plugin.BluetoothLE.Server;
using System;
using System.Linq;


namespace Plugin.BluetoothLE.Android.Server
{
    public class GattServer : AbstractGattServer
    {
        readonly GattContext context = new GattContext();

        public override BluetoothLE.Server.IGattService CreateService(Guid uuid, bool primary) => new GattService(this.context, this, uuid, primary);


        protected override void AddNative(BluetoothLE.Server.IGattService service)
        {
            var native = service as IDroidGattService;
            if (native == null)
                throw new ArgumentException("Service does not inherit IDroidGattService");

            if (native.Characteristics.Count == 0)
                throw new ArgumentException("No characteristics are assigned to this service");

            native
                .Characteristics
                .Cast<IDroidGattCharacteristic>()
                .Select(character =>
                {
                    character
                        .Descriptors
                        .Cast<IDroidGattDescriptor>()
                        .Select(y => y.Native)
                        .ToList()
                        .ForEach(character.Native.Descriptors.Add);

                    return native.Native;
                });

            this.context.Server.AddService(native.Native);
        }


        protected override void RemoveNative(BluetoothLE.Server.IGattService service) => this.context.Server.Services.Remove(((IDroidGattService) service).Native);
        protected override void ClearNative() => this.context.Server.ClearServices();


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.context.Server.Close();
        }
    }
}