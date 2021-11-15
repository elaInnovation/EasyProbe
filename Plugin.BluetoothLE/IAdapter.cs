﻿using System;
using System.Collections.Generic;
using Plugin.BluetoothLE.Server;


namespace Plugin.BluetoothLE
{
    public interface IAdapter
    {
        /// <summary>
        /// The detected name of the device
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// Create a BLE server
        /// </summary>
        /// <returns></returns>
        IObservable<IGattServer> CreateGattServer();

        /// <summary>
        /// Use this to advertise your device
        /// </summary>
        /// <value>The advertiser.</value>
        IAdvertiser Advertiser { get; }

        /// <summary>
        /// This readonly property contains a flags enum stating what platform adapter features that are available
        /// </summary>
        AdapterFeatures Features { get; }

        /// <summary>
        /// Get a known device
        /// </summary>
        /// <param name="deviceId">Device identifier.</param>
        IObservable<IDevice> GetKnownDevice(Guid deviceId);

        /// <summary>
        /// Returns current status of adapter (on/off/permission)
        /// </summary>
        AdapterStatus Status { get; }

        /// <summary>
        /// Get current scanning status
        /// </summary>
        bool IsScanning { get; }

        /// <summary>
        /// Stop any current scan - use this if you didn't keep a disposable endpoint for Scan()
        /// </summary>
        void StopScan();

        /// <summary>
        /// Gets a list of connected devices by your app
        /// </summary>
        /// <param name="serviceUuid">(iOS only) Service UUID filter to see devices that were connected outside of application</param>
        /// <returns></returns>
        IObservable<IEnumerable<IDevice>> GetConnectedDevices(Guid? serviceUuid = null);

        /// <summary>
        /// Get the list of paired devices
        /// </summary>
        /// <returns></returns>
        IObservable<IEnumerable<IDevice>> GetPairedDevices();

        /// <summary>
        /// Start scanning for BluetoothLE devices
        /// WARNING: only one scan can be active at a time.  Use IsScanning to check for active scanning
        /// </summary>
        /// <returns></returns>
        IObservable<IScanResult> Scan(ScanConfig config = null);

        /// <summary>
        /// Monitor for status changes with adapter (on/off/permissions)
        /// </summary>
        /// <returns></returns>
        IObservable<AdapterStatus> WhenStatusChanged();

        ///// <summary>
        ///// Monitor for all device status changes
        ///// </summary>
        ///// <returns></returns>
        //IObservable<IDevice> WhenDeviceStatusChanged();

        /// <summary>
        /// Opens the platform settings screen
        /// </summary>
        void OpenSettings();

        /// <summary>
        /// Toggles the bluetooth adapter on/off - returns true if successful
        /// Works only on Android
        /// </summary>
        /// <returns></returns>
        void SetAdapterState(bool enable);

        /// <summary>
        /// iOS ONLY - this is called for WillRestoreState is performed
        /// You must use BleAdapter.Init in your iOS project to set the configuration options
        /// </summary>
        /// <returns></returns>
        IObservable<IDevice> WhenDeviceStateRestored();
    }
}