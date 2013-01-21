// IMvxServiceConsumer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Interfaces.ServiceProvider
{
    // just a marker interface
    public interface IMvxServiceConsumer
    {
    }

    // just a marker interface
	[Obsolete("Use IMvxServiceConsumer, not IMvxServiceConsumer<T>")]
    public interface IMvxServiceConsumer<TService> : IMvxServiceConsumer
        where TService : class
    {
    }
}