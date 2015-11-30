// MvxCreateViewParameters.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Android.Views;

namespace MvvmCross.Droid.Support.V7.Fragging
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