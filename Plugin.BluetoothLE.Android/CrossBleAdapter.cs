using System;
using Android.App;
using Android.OS;

[assembly: UsesPermission("android.permission.BLUETOOTH")]
[assembly: UsesPermission("android.permission.BLUETOOTH_ADMIN")]
[assembly: UsesPermission("android.permission.ACCESS_COARSE_LOCATION")]

namespace Plugin.BluetoothLE.Android
{
    public static class CrossBleAdapterAndroid
    {
        static IAdapterScanner scanner;
        public static IAdapterScanner AdapterScanner
        {
            get
            {
                if (current == null)
                    throw new ArgumentException("[Plugin.BluetoothLE] No platform plugin found.  Did you install the nuget package in your app project as well?");

                return scanner;
            }
            set => scanner = value;
        }


        static IAdapter current;
        public static IAdapter Current
        {
            get
            {
                if (current == null)
                    throw new ArgumentException("[Plugin.BluetoothLE] No platform plugin found.  Did you install the nuget package in your app project as well?");

                return current;
            }
            set => current = value;
        }


        static AndroidConfig config;
        public static AndroidConfig AndroidConfiguration
        {
            get
            {
                config = config ?? new AndroidConfig();
                return config;
            }
        }

        static CrossBleAdapterAndroid()
        {
            Current = new Adapter();
            AdapterScanner = new AdapterScanner();

            AndroidConfiguration.IsMainThreadSuggested = Build.VERSION.SdkInt < BuildVersionCodes.Kitkat || (
                Build.VERSION.SdkInt < BuildVersionCodes.N &&
                Build.Manufacturer.Equals("samsung", StringComparison.CurrentCultureIgnoreCase)
            );

            AndroidConfiguration.IsServerSupported = Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop;
            AndroidConfiguration.UseNewScanner = Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop;
            AndroidConfiguration.ShouldInvokeOnMainThread = true;
        }
    }
}
