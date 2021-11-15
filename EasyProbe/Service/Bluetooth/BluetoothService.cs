using EasyProbe.Models.Bluetooth;
using EasyProbe.Tools;
using ElaTagClassLibrary.ElaTags.Bluetooth.Data;
using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Android;
using Plugin.BluetoothLE.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EasyProbe.Service.Bluetooth
{
    public partial class BluetoothService : IBluetoothService
    {
        private string Filter = "PROBE";
        IDisposable adapter = null;
        private bool IsScanning = false;

        private Dictionary<String, TagBluetooth> devices = new Dictionary<string, TagBluetooth>();
        public TagBluetooth CurrentTag { get; set; }
        public String CurrentTemp { get; set; }

        public void SetFilter(string filter)
        {
            this.Filter = filter;
        }

        public void StartScanner()
        {
 
                try
                {
                    if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                    {
                        if (CrossBleAdapterAndroid.Current.Status == AdapterStatus.PoweredOff ||
                                CrossBleAdapterAndroid.Current.Status == AdapterStatus.Resetting ||
                                CrossBleAdapterAndroid.Current.Status == AdapterStatus.Unauthorized ||
                                CrossBleAdapterAndroid.Current.Status == AdapterStatus.Unsupported)
                        {
                            CrossBleAdapterAndroid.Current.SetAdapterState(true);
                        }

                        adapter = CrossBleAdapterAndroid.Current.Scan().Subscribe(scanResult =>
                        {
                            receiveScanResult(scanResult);
                            IsScanning = true;
                        });
                    }
                    else
                    {
                        if (CrossBleAdapteriOS.Current.Status == AdapterStatus.PoweredOff ||
                           CrossBleAdapteriOS.Current.Status == AdapterStatus.Resetting ||
                           CrossBleAdapteriOS.Current.Status == AdapterStatus.Unauthorized ||
                           CrossBleAdapteriOS.Current.Status == AdapterStatus.Unsupported)
                        {
                            if (CrossBleAdapteriOS.Current.CanOpenSettings())
                                CrossBleAdapteriOS.Current.OpenSettings();
                        }
                        else
                        {
                            adapter = CrossBleAdapteriOS.Current.Scan().Subscribe(scanResult =>
                            {
                                receiveScanResult(scanResult);
                                IsScanning = true;
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    
                }

        }

        public void StopScanner()
        {
            adapter.Dispose();
            IsScanning = false;
        }

        private void receiveScanResult(IScanResult scanResult)
        {

            if(scanResult.AdvertisementData.LocalName == null) { return; }

            if (scanResult.AdvertisementData.LocalName.Equals("")) { return; }

            if (this.Filter == "" || scanResult.AdvertisementData.LocalName.ToUpper().Contains(this.Filter.ToUpper()))
            {
                if (scanResult.AdvertisementData.ServiceData != null)
                {
                    foreach (byte[] data in scanResult.AdvertisementData.ServiceData)
                    {
                        var raw = new AdvertisingValue();
                        raw.setRawData(data);
                        if (raw.isThermometer())
                        {
                            TagBluetooth tag = new TagBluetooth()
                            {
                                RawData = scanResult,
                                Temperature = Converter.StringToCelcius(raw.StrDataValue),
                                Farenheit = Converter.CelciusToFaren(raw.StrDataValue),
                                TagName = scanResult.AdvertisementData.LocalName,
                                Rssi = scanResult.Rssi
                            };
                            this.CurrentTag = tag;
                            if (this.devices.ContainsKey(tag.TagName))
                                this.devices[tag.TagName] = tag;
                            else
                                this.devices.Add(tag.TagName, tag);
                            MessagingCenter.Send<App, TagBluetooth>((App)Application.Current, MessengerKeys.UPDATE_TAG, this.CurrentTag);
                        }
                       
                    }
                }
            }
        }

        public TagBluetooth GetLastItem(string tagName)
        {
            if (tagName != String.Empty || tagName != "")
                return this.devices[tagName];
            else
                return null;
        }

        public object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public bool CheckBluetoothState()
        {
            throw new NotImplementedException();
        }
    }

}
