// IMvxServiceProducer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Interfaces.ServiceProvider
{
    // just a marker interface - use the Generic form wherever possible
    public interface IMvxServiceProducer
    {
    }

    // just a marker interface
	[Obsolete("Use IMvxServiceProducer, not IMvxServiceProducer<T>")]
    public interface IMvxServiceProducer<TInterfaceType>
    {
    }
}