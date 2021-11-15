using System;
namespace Plugin.BluetoothLE.Server
{
    public class DeviceSubscriptionEvent
    {
        public DeviceSubscriptionEvent(BluetoothLE.Server.IDevice device, bool subscribed)
        {
            this.Device = device;
            this.IsSubscribed = subscribed;
        }


        public bool IsSubscribed { get; }
        public BluetoothLE.Server.IDevice Device { get; }
    }
}
