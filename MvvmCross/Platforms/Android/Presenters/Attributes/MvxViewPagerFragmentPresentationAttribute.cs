// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Platforms.Android.Presenters.Attributes;

#nullable enable
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MvxViewPagerFragmentPresentationAttribute : MvxFragmentPresentationAttribute
{
    public MvxViewPagerFragmentPresentationAttribute()
    {
    }

    public MvxViewPagerFragmentPresentationAttribute(
        string title,
        int viewPagerResourceId,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type? activityHostViewModelType = null,
        bool addToBackStack = false,
        Type? fragmentHostViewType = null,
        bool isCacheableFragment = false,
        string? tag = null)
        : base(
              activityHostViewModelType,
              int.MinValue,
              addToBackStack,
              int.MinValue,
              int.MinValue,
              int.MinValue,
              int.MinValue,
              int.MinValue,
              fragmentHostViewType,
              isCacheableFragment,
              tag)
    {
        Title = title;
        ViewPagerResourceId = viewPagerResourceId;
    }

    public MvxViewPagerFragmentPresentationAttribute(
        string title,
        string viewPagerResourceName,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type? activityHostViewModelType = null,
        bool addToBackStack = false,
        Type? fragmentHostViewType = null,
        bool isCacheableFragment = false,
        string? tag = null)
        : base(
              activityHostViewModelType,
              null,
              addToBackStack,
              null,
              null,
              null,
              null,
              null,
              fragmentHostViewType,
              isCacheableFragment,
              tag)
    {
        Title = title;

        if (!string.IsNullOrEmpty(viewPagerResourceName) &&
            Mvx.IoCProvider?.TryResolve(out IMvxAndroidGlobals globals) == true &&
            globals.ApplicationContext.Resources != null)
        {
            ViewPagerResourceId = globals.ApplicationContext.Resources.GetIdentifier(
                viewPagerResourceName, "id", globals.ApplicationContext.PackageName);
        }
        else
        {
            ViewPagerResourceId = global::Android.Resource.Id.Content;
        }
    }

    /// <summary>
    /// The title for the ViewPager. Also used as Title for TabLayout when using MvxTabLayoutPresentationAttribute
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The resource used to get the ViewPager from the view
    /// </summary>
    public int ViewPagerResourceId { get; set; }
}
