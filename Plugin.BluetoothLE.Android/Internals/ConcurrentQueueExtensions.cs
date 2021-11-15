using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Plugin.BluetoothLE.Android.Internals
{
	internal static class ConcurrentQueueExtensions
	{
		public static void Clear<T>(this ConcurrentQueue<T> queue)
		{
			T item;
			while (queue.TryDequeue(out item))
			{
				// do nothing
			}
		}
	}
}
