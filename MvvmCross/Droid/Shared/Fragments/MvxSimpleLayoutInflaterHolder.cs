// MvxSimpleLayoutInflater.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using MvvmCross.Binding.Droid.Views;

namespace MvvmCross.Droid.Shared.Fragments
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