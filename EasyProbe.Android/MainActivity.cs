using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using AndroidX.Core.App;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using System.Collections.Generic;

namespace EasyProbe.Droid
{
    [Activity(Label = "EasyProbe", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        string[] PermissionsArray = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            base.OnCreate(savedInstanceState);
            
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Acr.UserDialogs.UserDialogs.Init(this);
            ActivityCompat.RequestPermissions(this,new String[]{
                Manifest.Permission.BluetoothAdmin, Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.Bluetooth, Manifest.Permission.AccessCoarseLocation}, 0);

            updateNonGrantedPermissions();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void updateNonGrantedPermissions()
        {
            try
            {
                List<string> PermissionList = new List<string>();
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Bluetooth) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.Bluetooth);
                }
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Nfc) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.Nfc);
                }
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.BluetoothAdmin) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.BluetoothAdmin);
                }
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.AccessCoarseLocation) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.AccessCoarseLocation);
                }
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.AccessFineLocation) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.AccessFineLocation);
                }
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.WriteExternalStorage) != (int)Android.Content.PM.Permission.Granted)
                {
                    PermissionList.Add(Manifest.Permission.AccessFineLocation);
                }

                PermissionsArray = new string[PermissionList.Count];
                for (int index = 0; index < PermissionList.Count; index++)
                {
                    PermissionsArray.SetValue(PermissionList[index], index);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}