// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxSimpleLayoutInflaterHolder : IMvxLayoutInflaterHolder
    {
        public MvxSimpleLayoutInflaterHolder(LayoutInflater layoutInflater)
        {
            LayoutInflater = layoutInflater;
        }

        public LayoutInflater LayoutInflater { get; private set; }
    }
}
