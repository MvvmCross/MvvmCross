// IMvxAutoViewTextLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Interfaces
{
    using System;

    public interface IMvxAutoViewTextLoader
    {
        bool HasDefinition(Type viewModelType, string key);

        string GetDefinition(Type viewModelType, string key);
    }
}