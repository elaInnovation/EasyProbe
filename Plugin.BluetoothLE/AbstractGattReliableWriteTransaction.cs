using System;
using System.Reactive;


namespace Plugin.BluetoothLE
{
    public abstract class AbstractGattReliableWriteTransaction : IGattReliableWriteTransaction
    {
        ~AbstractGattReliableWriteTransaction()
        {
            this.Dispose(false);
        }


        protected virtual void AssertAction()
        {
            if (this.Status != TransactionStatus.Active)
                throw new ArgumentException("Cannot perform action as transaction status is already " + this.Status);
        }


        public TransactionStatus Status { get; protected set; } = TransactionStatus.Active;
        public abstract IObservable<CharacteristicGattResult> Write(IGattCharacteristic characteristic, byte[] value);
        public abstract IObservable<Unit> Commit();
        public abstract void Abort();


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
