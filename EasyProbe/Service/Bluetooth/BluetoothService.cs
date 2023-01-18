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
                if(scanResult.AdvertisementData.LocalName.Contains("TOOL_TAG"))
                {
                    var raw = scanResult.AdvertisementData.RawData;
                    if(raw != null)
                    {
                        // frame counter
                        if (raw[11] == 32)
                        {
                            int bat_lsb = int.Parse(raw[14].ToString());
                            int bat_msb = int.Parse(raw[13].ToString());
                            float battery = Convert.ToInt32("0x"+bat_msb.ToString("X") + bat_lsb.ToString("X"), 16);
                            battery = battery / 1000;

                            int counter = int.Parse(raw[15].ToString());

                            int state = int.Parse(raw[16].ToString());

                            float main_frame_count = Convert.ToInt32("0x" +
                                int.Parse(raw[17].ToString()).ToString("X") +
                                int.Parse(raw[18].ToString()).ToString("X") +
                                int.Parse(raw[19].ToString()).ToString("X") +
                                int.Parse(raw[20].ToString()).ToString("X"),16);

                            float second_frame_count = Convert.ToInt32("0x" +
                                int.Parse(raw[21].ToString()).ToString("X") +
                                int.Parse(raw[22].ToString()).ToString("X") +
                                int.Parse(raw[23].ToString()).ToString("X") +
                                int.Parse(raw[24].ToString()).ToString("X"), 16);

                           
                            float n2T2 = (1 * (((main_frame_count)-67) * 10 - 10 - (second_frame_count/10))) / (10 - 1);

                            TagBluetooth tag = new TagBluetooth()
                            {
                                RawData = scanResult,
                                Battery = battery,
                                Counter = counter,
                                State = state == 0 ? false : true,
                                TimeUsed = n2T2 < 0? 0 : n2T2,
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


                /*
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
                }*/
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
