﻿using System;
using System.IO;


namespace Plugin.BluetoothLE
{
    public interface IGattCharacteristic
    {
        IGattService Service { get; }
        Guid Uuid { get; }
        string Description { get; }
        bool IsNotifying { get; }
        CharacteristicProperties Properties { get; }
        byte[] Value { get; }

        /// <summary>
        /// Enable notifications (or indications if available)
        /// </summary>
        /// <param name="useIndicationIfAvailable">If true and indication is available, it will be used</param>
        /// <returns></returns>
        IObservable<CharacteristicGattResult> EnableNotifications(bool useIndicationIfAvailable = false);

        /// <summary>
        /// Disable notifications
        /// </summary>
        IObservable<CharacteristicGattResult> DisableNotifications();

        /// <summary>
        /// This will only monitor any notifications to the characteristic if it is hooked.  It will not (un)subscribe them.  Use SubscribeToNotifications
        /// </summary>
        /// <returns></returns>
        IObservable<CharacteristicGattResult> WhenNotificationReceived();

        /// <summary>
        /// Discovers descriptors for this characteristic
        /// </summary>
        /// <returns></returns>
        IObservable<IGattDescriptor> DiscoverDescriptors();

        /// <summary>
        /// This will fire and forget a write
        /// </summary>
        /// <param name="value"></param>
        IObservable<CharacteristicGattResult> WriteWithoutResponse(byte[] value);

        /// <summary>
        /// Writes the value to the remote characteristic
        /// </summary>
        /// <param name="value">The bytes to send</param>
        IObservable<CharacteristicGattResult> Write(byte[] value);

        /// <summary>
        /// Read characteristic remote value
        /// </summary>
        /// <returns></returns>
        IObservable<CharacteristicGattResult> Read();
    }
}
