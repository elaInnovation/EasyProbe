package crc64b7fa605b87944894;


public class LollipopScanCallback
	extends android.bluetooth.le.ScanCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onScanResult:(ILandroid/bluetooth/le/ScanResult;)V:GetOnScanResult_ILandroid_bluetooth_le_ScanResult_Handler\n" +
			"n_onBatchScanResults:(Ljava/util/List;)V:GetOnBatchScanResults_Ljava_util_List_Handler\n" +
			"";
		mono.android.Runtime.register ("Plugin.BluetoothLE.Internals.LollipopScanCallback, Plugin.BluetoothLE", LollipopScanCallback.class, __md_methods);
	}


	public LollipopScanCallback ()
	{
		super ();
		if (getClass () == LollipopScanCallback.class)
			mono.android.TypeManager.Activate ("Plugin.BluetoothLE.Internals.LollipopScanCallback, Plugin.BluetoothLE", "", this, new java.lang.Object[] {  });
	}


	public void onScanResult (int p0, android.bluetooth.le.ScanResult p1)
	{
		n_onScanResult (p0, p1);
	}

	private native void n_onScanResult (int p0, android.bluetooth.le.ScanResult p1);


	public void onBatchScanResults (java.util.List p0)
	{
		n_onBatchScanResults (p0);
	}

	private native void n_onBatchScanResults (java.util.List p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
