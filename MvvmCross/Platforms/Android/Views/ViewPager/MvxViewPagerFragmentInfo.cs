// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace MvvmCross.Platforms.Android.Views.ViewPager
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
