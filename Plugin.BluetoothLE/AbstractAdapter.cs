using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Plugin.BluetoothLE.Server;


namespace Plugin.BluetoothLE
{
    public abstract class AbstractAdapter : IAdapter
    {
        public abstract IObservable<IScanResult> Scan(ScanConfig config = null);
        public abstract IObservable<AdapterStatus> WhenStatusChanged();
        public abstract IObservable<IGattServer> CreateGattServer();
        public abstract void StopScan();

        public virtual IAdvertiser Advertiser { get; protected set; }
        public virtual string DeviceName { get; protected set; }
        public virtual AdapterFeatures Features { get; protected set; }
        public virtual AdapterStatus Status { get; protected set; }
        public virtual bool IsScanning { get; protected set; }
        public virtual IObservable<IDevice> GetKnownDevice(Guid deviceId) => throw new NotImplementedException("GetKnownDevice is not supported on this platform");
        public virtual IObservable<IEnumerable<IDevice>> GetConnectedDevices(Guid? serviceUuid = null) => throw new NotImplementedException("GetConnectedDevices is not supported on this platform");
        public virtual IObservable<IEnumerable<IDevice>> GetPairedDevices() => throw new NotImplementedException("GetPairedDevices is not supported on this platform");
        //public virtual IObservable<IDevice> WhenDeviceStatusChanged() => throw new NotImplementedException("WhenDeviceStatusChanged is not supported on this platform");
        public virtual void OpenSettings() => throw new NotImplementedException("OpenSettings is not supported on this platform");
        public virtual void SetAdapterState(bool enable) => throw new NotImplementedException("SetAdapterState is not supported on this platform");
        public virtual IObservable<IDevice> WhenDeviceStateRestored() => Observable.Empty<IDevice>();
    }
}