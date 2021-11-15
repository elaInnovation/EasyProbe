using System;
using System.Reactive;
using System.Reactive.Linq;
using Acr.Reactive;
using Plugin.BluetoothLE.Android.Internals;

namespace Plugin.BluetoothLE.Android
{
    public class GattReliableWriteTransaction : AbstractGattReliableWriteTransaction
    {
        readonly DeviceContext context;


        public GattReliableWriteTransaction(DeviceContext context)
        {
            this.context = context;
            this.context.Gatt.BeginReliableWrite();
        }


        public override IObservable<CharacteristicGattResult> Write(IGattCharacteristic characteristic, byte[] value)
        {
            // just write to the standard characteristic write
            this.AssertAction();
            return characteristic.Write(value);
        }


        public override IObservable<Unit> Commit() => this.context.Invoke(Observable.Create<Unit>(ob =>
        {
            this.AssertAction();

            var sub = this.context
                .Callbacks
                .ReliableWriteCompleted
                .Subscribe(args =>
                {
                    if (args.IsSuccessful)
                    {
                        this.Status = TransactionStatus.Committed;
                        ob.Respond(Unit.Default);
                    }
                    else
                    {
                        this.Status = TransactionStatus.Aborted; // TODO: or errored?
                        ob.OnError(new GattReliableWriteTransactionException("Error committing transaction"));
                    }
                });

            if (!this.context.Gatt.ExecuteReliableWrite())
                throw new GattReliableWriteTransactionException("Failed to execute write");

            this.Status = TransactionStatus.Committing;

            return sub;
        }));


        public override void Abort()
        {
            this.AssertAction();
            this.context.Gatt.AbortReliableWrite();
            this.Status = TransactionStatus.Aborted;
        }


        protected override void Dispose(bool disposing)
        {
        }
    }
}