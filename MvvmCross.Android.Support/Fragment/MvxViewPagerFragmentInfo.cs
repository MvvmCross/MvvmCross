// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Support.V4.App;
using MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V4
{
    public class MvxViewPagerFragmentInfo
    {
        public MvxViewPagerFragmentInfo(string title, string tag, Type fragmentType, MvxViewModelRequest request)
        {
            Title = title;
            Tag = tag;
            FragmentType = fragmentType;
            Request = request;
        }

        public Type FragmentType { get; }

        public string Tag { get; }

        public string Title { get; }

        public MvxViewModelRequest Request { get; }

        public Fragment CachedFragment { get; set; }
    }
}
