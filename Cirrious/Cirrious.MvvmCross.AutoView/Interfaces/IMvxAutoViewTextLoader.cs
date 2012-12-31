#region Copyright

// <copyright file="IMvxAutoViewTextLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxAutoViewTextLoader
    {
        bool HasDefinition(Type viewModelType, string key);
        string GetDefinition(Type viewModelType, string key);
    }
}