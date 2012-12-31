#region Copyright

// <copyright file="IMvxResourceObjectLoaderConfiguration.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

namespace Cirrious.MvvmCross.Plugins.ResourceLoader
{
    public interface IMvxResourceObjectLoaderConfiguration<T>
        where T : IMvxResourceObject
    {
        void SetRootLocation(string location);
        void SetRootLocation(string namespaceKey, string typeKey, string location);
    }
}