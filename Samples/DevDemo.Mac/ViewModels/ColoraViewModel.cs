using System;
using DevDemo.Core.Services;
using MonoMac.Foundation;

namespace DevDemo.Mac
{
	public class KVCObject<T> : NSObject
	{
		private readonly T _t;
		private readonly Type _type = typeof(T);


		public KVCObject (T t)
		{
			_t = t;
		}

		public override void SetValueForKey (NSObject value, NSString key)
		{
			var info = _type.GetProperty (key.ToString ());
			info.SetValue (_t, value);
		}

		public override NSObject ValueForKey (NSString key)
		{
			var info = _type.GetProperty (key.ToString ());
			return info.GetValue (_t);
		}
	}
}

