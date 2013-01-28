// MvxHttpImageView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    [Obsolete("Use MvxImageView instead")]
    public class MvxHttpImageView
        : MvxImageView
    {
        public MvxHttpImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public MvxHttpImageView(Context context)
            : base(context)
        {
        }

        protected MvxHttpImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}