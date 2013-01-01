﻿// IMvxStartNavigation.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Interfaces.ViewModels
{
    public interface IMvxStartNavigation
    {
        void Start();
#warning Not sure ApplicationCanOpenBookmarks should be in Mvx Library!
        bool ApplicationCanOpenBookmarks { get; }
    }
}