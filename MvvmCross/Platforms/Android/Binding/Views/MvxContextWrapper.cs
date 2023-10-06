// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using Object = Java.Lang.Object;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    [Register("mvvmcross.platforms.android.binding.views.MvxContextWrapper")]
    public class MvxContextWrapper : ContextWrapper
    {
        private LayoutInflater _inflater;
        private readonly IMvxBindingContextOwner _bindingContextOwner;

        public static ContextWrapper Wrap(Context @base, IMvxBindingContextOwner bindingContextOwner)
        {
            return new MvxContextWrapper(@base, bindingContextOwner);
        }

        protected MvxContextWrapper(Context context, IMvxBindingContextOwner bindingContextOwner)
            : base(context)
        {
            if (bindingContextOwner == null)
                throw new InvalidOperationException("Wrapper can only be set on IMvxBindingContextOwner");

            _bindingContextOwner = bindingContextOwner;
        }

        public override Object GetSystemService(string name)
        {
            if (string.Equals(name, LayoutInflaterService, StringComparison.InvariantCulture))
            {
                return _inflater ??=
                    new MvxLayoutInflater(LayoutInflater.From(BaseContext), this, null, false);
            }

            return base.GetSystemService(name);
        }
    }
}
