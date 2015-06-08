// IMvxLayoutInflaterFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Cirrious.MvvmCross.Binding.Bindings;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public interface IMvxLayoutInflaterFactory
    {
        IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings { get; }

        View OnCreateView(View parent, string name, Context context, IAttributeSet attrs);
    }
}