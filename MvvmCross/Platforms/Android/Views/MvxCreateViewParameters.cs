// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxCreateViewParameters
    {
        public MvxCreateViewParameters(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            SavedInstanceState = savedInstanceState;
            Container = container;
            Inflater = inflater;
        }

        public LayoutInflater Inflater { get; private set; }
        public ViewGroup Container { get; private set; }
        public Bundle SavedInstanceState { get; private set; }
    }
}
