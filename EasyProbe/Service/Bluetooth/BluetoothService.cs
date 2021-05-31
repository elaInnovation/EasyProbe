using EasyProbe.Models.Bluetooth;
using EasyProbe.Tools;
using ElaTagClassLibrary.ElaTags.Bluetooth.Data;
using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EasyProbe.Service.Bluetooth
{
    public partial class BluetoothService : IBluetoothService
    {
        private string Filter = "PROBE";

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
                if (CheckBluetoothState() == false)
                {
                    if (Device.RuntimePlatform == Device.Android)
                        CrossBleAdapter.Current.SetAdapterState(true);
                    else
                    {
                        CrossBleAdapter.Current.OpenSettings();
                        return;
                    }
                }

                CrossBleAdapter.Current.ScanExtra(new ScanConfig()).Subscribe(scanResult =>
                {
                    if (scanResult.AdvertisementData.LocalName != null)
                    {
                        receiveScanResult(scanResult);
                    }
                });
            }
            catch (Exception)
            {

            }
        }

        public bool CheckBluetoothState()
        {
            try
            {

                if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOff ||
                   CrossBleAdapter.Current.Status == AdapterStatus.Resetting ||
                   CrossBleAdapter.Current.Status == AdapterStatus.Unauthorized ||
                   CrossBleAdapter.Current.Status == AdapterStatus.Unsupported)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void StopScanner()
        {
            CrossBleAdapter.Current.StopScan();
        }

        private void receiveScanResult(IScanResult scanResult)
        {
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
                                TagName = scanResult.Device.Name,
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
    }

}
