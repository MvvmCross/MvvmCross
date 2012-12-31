#region Copyright

// <copyright file="IMvxTextProviderBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
{
    public interface IMvxTextProviderBuilder
    {
        MvxJsonDictionaryTextProvider TextProvider { get; }
        void LoadResources(string whichLocalisationFolder);
    }
}