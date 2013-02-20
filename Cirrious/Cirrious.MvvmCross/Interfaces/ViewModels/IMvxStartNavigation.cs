﻿// IMvxStartNavigation.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform;

namespace Cirrious.MvvmCross.Interfaces.ViewModels
{
    public interface IMvxStartNavigation
    {
        void Start();
    }

    public interface IMvxNavigationRequestSerializer
    {
        IMvxTextSerializer Serializer { get; }
    }
}