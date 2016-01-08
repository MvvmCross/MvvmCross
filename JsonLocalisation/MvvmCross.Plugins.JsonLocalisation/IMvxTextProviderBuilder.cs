// IMvxTextProviderBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Localization;

namespace MvvmCross.Plugins.JsonLocalisation
{
    public interface IMvxTextProviderBuilder
    {
        IMvxTextProvider TextProvider { get; }

        void LoadResources(string whichLocalisationFolder);
    }
}