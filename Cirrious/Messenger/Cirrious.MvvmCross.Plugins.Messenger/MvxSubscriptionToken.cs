// MvxSubscriptionToken.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    public class MvxSubscriptionToken
    {
        public Guid Id { get; private set; }
        private readonly object[] _dependentObjects;

        public MvxSubscriptionToken(Guid id, params object[] dependentObjects)
        {
            Id = id;
            _dependentObjects = dependentObjects;
        }
    }
}