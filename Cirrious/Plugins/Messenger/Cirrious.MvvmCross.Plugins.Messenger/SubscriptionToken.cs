// BaseSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
	public class SubscriptionToken
	{
		public Guid Id { get; private set; }
		public object[] _dependentObjects;

		public SubscriptionToken (Guid id, params object[] dependentObjects)
		{
			Id = id;
			_dependentObjects = dependentObjects;
		}
	}    
}