// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Hosting;

namespace MvvmCross.Platforms.Android.Presenters.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MvxTabLayoutPresentationAttribute : MvxViewPagerFragmentPresentationAttribute
{
    public MvxTabLayoutPresentationAttribute()
    {
    }

    public MvxTabLayoutPresentationAttribute(
        string title,
        int viewPagerResourceId,
        int tabLayoutResourceId,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? activityHostViewModelType = null,
        bool addToBackStack = false,
        Type? fragmentHostViewType = null,
        bool isCacheableFragment = false)
        : base(
              title,
              viewPagerResourceId,
              activityHostViewModelType,
              addToBackStack,
              fragmentHostViewType,
              isCacheableFragment)
    {
        TabLayoutResourceId = tabLayoutResourceId;
    }

    public MvxTabLayoutPresentationAttribute(
        string title,
        string viewPagerResourceName,
        string tabLayoutResourceName,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? activityHostViewModelType = null,
        bool addToBackStack = false,
        Type? fragmentHostViewType = null,
        bool isCacheableFragment = false)
        : base(
              title,
              viewPagerResourceName,
              activityHostViewModelType,
              addToBackStack,
              fragmentHostViewType,
              isCacheableFragment)
    {
        if (!string.IsNullOrEmpty(tabLayoutResourceName))
        {
            IMvxAndroidGlobals? globals = null;
            if ((globals = MvxHost.Current?.Services.GetService<IMvxAndroidGlobals>()) != null && globals.ApplicationContext.Resources != null)
            {
                TabLayoutResourceId = globals.ApplicationContext.Resources.GetIdentifier(
                    tabLayoutResourceName, "id", globals.ApplicationContext.PackageName);
            }
            else
            {
                TabLayoutResourceId = global::Android.Resource.Id.Content;
            }
        }
        else
        {
            TabLayoutResourceId = global::Android.Resource.Id.Content;
        }
    }

    /// <summary>
    ///     The resource used to get the TabLayout from the view
    /// </summary>
    public int TabLayoutResourceId { get; set; }
}
