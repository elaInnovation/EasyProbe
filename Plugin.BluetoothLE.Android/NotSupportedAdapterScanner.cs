using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace Plugin.BluetoothLE.Android
{
    public class NotSupportedAdapterScanner : IAdapterScanner
    {
        public bool IsSupported => false;
        public IObservable<IAdapter> FindAdapters() => Observable.Return(CrossBleAdapterAndroid.Current);
    }
}
