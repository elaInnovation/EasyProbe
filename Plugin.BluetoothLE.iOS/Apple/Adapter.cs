using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CoreBluetooth;
using Foundation;
using Plugin.BluetoothLE.iOS.Server;
using Plugin.BluetoothLE.Server;
using UIKit;

namespace Plugin.BluetoothLE.iOS
{
    public class Adapter : AbstractAdapter
    {
        readonly AdapterContext context;


        public override string DeviceName => "Default Bluetooth Device";
        public override bool IsScanning => this.context.Manager.IsScanning;


        public override AdapterStatus Status
        {
            get
            {
                switch (this.context.Manager.State)
                {
                    case CBCentralManagerState.PoweredOff:
                        return AdapterStatus.PoweredOff;

                    case CBCentralManagerState.PoweredOn:
                        return AdapterStatus.PoweredOn;

                    case CBCentralManagerState.Resetting:
                        return AdapterStatus.Resetting;

                    case CBCentralManagerState.Unauthorized:
                        return AdapterStatus.Unauthorized;

                    case CBCentralManagerState.Unsupported:
                        return AdapterStatus.Unsupported;

                    case CBCentralManagerState.Unknown:
                    default:
                        return AdapterStatus.Unknown;
                }
            }
        }


        public override IObservable<IDevice> GetKnownDevice(Guid deviceId)
        {
            var peripheral = this.context.Manager.RetrievePeripheralsWithIdentifiers(deviceId.ToNSUuid()).FirstOrDefault();
            if (peripheral == null)
                return Observable.Return<IDevice>(null);

            var device = this.context.GetDevice(peripheral);
            return Observable.Return(device);
        }


        public override IObservable<IEnumerable<IDevice>> GetPairedDevices() => Observable.Return(new IDevice[0]);

        public override IObservable<IEnumerable<IDevice>> GetConnectedDevices(Guid? serviceUuid = null)
        {
            if (serviceUuid == null)
                return Observable.Return(this.context.GetConnectedDevices().ToList());

            var list = new List<IDevice>();
            var peripherals = this.context.Manager.RetrieveConnectedPeripherals(serviceUuid.Value.ToCBUuid());
            foreach (var peripheral in peripherals)
            {
                var dev = this.context.GetDevice(peripheral);
                list.Add(dev);
            }
            return Observable.Return(list);
        }


        public override IObservable<AdapterStatus> WhenStatusChanged() => this.context
            .StateUpdated
            .Select(_ => this.Status)
            .StartWith(this.Status);


        public override IObservable<IScanResult> Scan(ScanConfig config)
        {
            config = config ?? new ScanConfig();

            if (this.Status != AdapterStatus.PoweredOn)
                throw new ArgumentException("Your adapter status is " + this.Status);

            if (this.IsScanning)
                throw new ArgumentException("There is already an existing scan");

            if (config.ScanType == BleScanType.Background && (config.ServiceUuids == null || config.ServiceUuids.Count == 0))
                throw new ArgumentException("Background scan type set but not ServiceUUID");

            return Observable.Create<IScanResult>(ob =>
            {
                this.context.Clear();
                var scan = this.context
                    .ScanResultReceived
                    .AsObservable()
                    .Subscribe(ob.OnNext);

                if (config.ServiceUuids == null || config.ServiceUuids.Count == 0)
                {
                    this.context.Manager.ScanForPeripherals(null, new PeripheralScanningOptions { AllowDuplicatesKey = true });
                }
                else
                {
                    var uuids = config.ServiceUuids.Select(o => o.ToCBUuid()).ToArray();
                    if (config.ScanType == BleScanType.Background)
                    {
                        this.context.Manager.ScanForPeripherals(uuids);
                    }
                    else
                    {
                        this.context.Manager.ScanForPeripherals(uuids, new PeripheralScanningOptions { AllowDuplicatesKey = true });
                    }
                }

                return () =>
                {
                    this.context.Manager.StopScan();
                    scan.Dispose();
                };
            });
        }


        public override void StopScan() => this.context.Manager.StopScan();


        //IObservable<IDevice> deviceStatusOb;
        //public override IObservable<IDevice> WhenDeviceStatusChanged()
        //{
        //    this.deviceStatusOb = this.deviceStatusOb ??
        //        this.context
        //            .PeripheralConnected
        //            .Select(x => this.context.GetDevice(x))
        //            .Merge(this.context
        //                .PeripheralDisconnected
        //                .Select(x => this.context.GetDevice(x))
        //            )
        //            .Publish()
        //            .RefCount();

        //    return this.deviceStatusOb;
        //}


        public override IObservable<IDevice> WhenDeviceStateRestored() =>
            this.context
                .WhenWillRestoreState
                .AsObservable();


        public Adapter(BleAdapterConfiguration config = null)
        {
            this.context = new AdapterContext(config);
            this.Advertiser = new Advertiser(this.context.PeripheralManager);
        }


        public override AdapterFeatures Features
        {
            get
            {
                var v8or9 = UIDevice.CurrentDevice.CheckSystemVersion(8, 0) && !UIDevice.CurrentDevice.CheckSystemVersion(10, 0);
                return v8or9
                    ? AdapterFeatures.AllServer | AdapterFeatures.OpenSettings
                    : AdapterFeatures.AllServer;
            }
        }


        public override IObservable<IGattServer> CreateGattServer() => Observable.FromAsync(async ct =>
        {
            var cb = this.context.PeripheralManager;
            if (cb.State != CBPeripheralManagerState.PoweredOn)
            {
                await Task.Delay(3000).ConfigureAwait(false);
                if (cb.State != CBPeripheralManagerState.PoweredOn)
                    throw new BleException("Invalid Adapter State - " + cb.State);
            }

            return new GattServer(cb);
        });

        public override void OpenSettings()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                //var workSpaceClassId = Class.GetHandle("LSApplicationWorkspace");
                //if (workSpaceClassId != IntPtr.Zero)
                //{
                //    var workSpaceClass = new NSObject(workSpaceClassId);
                //    var workSpaceInstance = workSpaceClass.PerformSelector(new Selector("defaultWorkspace"));

                //    var selector = new Selector("openSensitiveURL:withOptions:");
                //    if (workSpaceInstance.RespondsToSelector(selector))
                //    {
                //        workSpaceInstance.PerformSelector(selector, new NSUrl("Prefs:root=Bluetooth"));
                //    }
                //}
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                OpenUrl("prefs:root=Bluetooth");
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                OpenUrl("prefs:root=General&path=Bluetooth");
            }
        }


        static void OpenUrl(string url)
        {
            var nsurl = new NSUrl(url);
            if (UIApplication.SharedApplication.CanOpenUrl(nsurl))
                UIApplication.SharedApplication.OpenUrl(nsurl);
        }
    }
}