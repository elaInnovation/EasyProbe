using System;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Plugin.BluetoothLE.Android.Server.Internals;
using Plugin.BluetoothLE.Server;


namespace Plugin.BluetoothLE.Android.Server
{
    public class Advertiser : AbstractAdvertiser
    {
        BluetoothManager manager;
        AdvertisementCallbacks adCallbacks;


        public override void Start(BluetoothLE.Server.AdvertisementData adData)
        {
            if (!CrossBleAdapterAndroid.AndroidConfiguration.IsServerSupported)
                throw new BleException("BLE Advertiser needs API Level 23+");

            this.manager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
            this.adCallbacks = new AdvertisementCallbacks();

            var settings = new AdvertiseSettings.Builder()
                .SetAdvertiseMode(AdvertiseMode.Balanced)
                .SetConnectable(adData.AndroidIsConnectable);

            var data = new AdvertiseData.Builder()
                .SetIncludeDeviceName(adData.AndroidUseDeviceName)
                .SetIncludeTxPowerLevel(true);

            if (adData.ManufacturerData != null)
                data.AddManufacturerData(adData.ManufacturerData.CompanyId, adData.ManufacturerData.Data);

            foreach (var serviceUuid in adData.ServiceUuids)
                data.AddServiceUuid(serviceUuid.ToParcelUuid());

            this.manager
                .Adapter
                .BluetoothLeAdvertiser
                .StartAdvertising(
                    settings.Build(),
                    data.Build(),
                    this.adCallbacks
                );

            base.Start(adData);
        }


        public override void Stop()
        {
            if (this.manager != null && this.adCallbacks != null)
                this.manager.Adapter.BluetoothLeAdvertiser.StopAdvertising(this.adCallbacks);

            base.Stop();
        }
    }
}
