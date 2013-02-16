using System;

namespace Cirrious.Conference.Core
{
	
	public class LoadingFinishedMessage : Cirrious.MvvmCross.Plugins.Messenger.BaseMessage
	{
		public LoadingFinishedMessage (object sender)
			: base(sender)
		{
		}
	}
}
