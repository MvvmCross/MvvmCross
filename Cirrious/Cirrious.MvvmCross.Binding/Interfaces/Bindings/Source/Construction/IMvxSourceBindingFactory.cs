#region Copyright
// <copyright file="IMvxSourceBindingFactory.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction
{
    public interface IMvxSourceBindingFactory
    {
        IMvxSourceBinding CreateBinding(object source, string combinedPropertyName);
        IMvxSourceBinding CreateBinding(object source, IEnumerable<string> childPropertyNames);
    }
}