// MvxBindingActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public partial class MvxBindingActivityView<TViewModel>
        : MvxActivityView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
    }
}