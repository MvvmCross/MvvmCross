using System;

namespace Cirrious.Conference.Core
{	
	public class LoadingChangedMessage : Cirrious.MvvmCross.Plugins.Messenger.MvxMessage
	{
		public LoadingChangedMessage (object sender)
			: base(sender)
		{
		}
	}
}
