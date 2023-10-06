// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Object = Java.Lang.Object;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
#nullable enable
    public static class MvxLayoutInflaterCompat
    {
        internal class FactoryWrapper : Object, LayoutInflater.IFactory
        {
            protected readonly IMvxLayoutInflaterFactory DelegateFactory;

            [Preserve(Conditional = true)]
#pragma warning disable 8618
            public FactoryWrapper(IntPtr handle, JniHandleOwnership ownership)
#pragma warning restore 8618
                : base(handle, ownership)
            {
            }

            public FactoryWrapper(IMvxLayoutInflaterFactory delegateFactory)
            {
                DelegateFactory = delegateFactory;
            }

            public View? OnCreateView(string name, Context context, IAttributeSet attrs)
            {
                return DelegateFactory.OnCreateView(null, name, context, attrs);
            }
        }

        internal class FactoryWrapper2 : FactoryWrapper, LayoutInflater.IFactory2
        {
            [Preserve(Conditional = true)]
            public FactoryWrapper2(IntPtr handle, JniHandleOwnership ownership)
                : base(handle, ownership)
            {
            }

            public FactoryWrapper2(IMvxLayoutInflaterFactory delegateFactory)
                : base(delegateFactory)
            {
            }

            public View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
            {
                return DelegateFactory.OnCreateView(parent, name, context, attrs);
            }
        }

        public static void SetFactory(LayoutInflater layoutInflater, IMvxLayoutInflaterFactory? factory)
        {
            layoutInflater.Factory2 = factory != null ? new FactoryWrapper2(factory) : null;
        }
    }
#nullable restore
}
