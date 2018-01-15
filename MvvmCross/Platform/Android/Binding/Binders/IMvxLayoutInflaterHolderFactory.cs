﻿// IMvxLayoutInflaterHolderFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding.Droid.Binders
{
    public interface IMvxLayoutInflaterHolderFactory : IMvxLayoutInflaterFactory
    {
        IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings { get; }

        View BindCreatedView(View view, Context context, IAttributeSet attrs);
    }
}