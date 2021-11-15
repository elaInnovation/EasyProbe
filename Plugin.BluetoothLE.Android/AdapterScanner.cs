using System;
using System.Reactive.Linq;


namespace Plugin.BluetoothLE.Android
{
    public class AdapterScanner : IAdapterScanner
    {
        public bool IsSupported => false;
        public IObservable<IAdapter> FindAdapters() => Observable.Return(CrossBleAdapterAndroid.Current);
    }
}