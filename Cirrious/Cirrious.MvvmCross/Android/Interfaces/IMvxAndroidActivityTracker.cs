#region Copyright

// <copyright file="IMvxAndroidActivityTracker.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.App;
using Cirrious.MvvmCross.Interfaces.ViewModel;

namespace Cirrious.MvvmCross.Android.Interfaces
{
    public interface IMvxAndroidActivityTracker
    {
        void SetInitialAndroidActivity(Activity activity);
        void OnTopLevelAndroidActivity(Activity activity, IMvxViewModel viewModel);
        void OnSubViewAndroidActivity(Activity activity);
        Activity CurrentTopLevelActivity { get; }
    }
}