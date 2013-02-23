using System;

namespace Cirrious.Conference.Core
{
	public class FavoritesChangedMessage : Cirrious.MvvmCross.Plugins.Messenger.MvxBaseMessage
	{
		public FavoritesChangedMessage (object sender)
			: base(sender)
		{
		}
	}
}

