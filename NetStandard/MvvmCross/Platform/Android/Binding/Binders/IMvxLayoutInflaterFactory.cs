﻿using Android.Content;
using Android.Util;
using Android.Views;

namespace MvvmCross.Binding.Droid.Binders
{
    public interface IMvxLayoutInflaterFactory
    {
        View OnCreateView(View parent, string name, Context context, IAttributeSet attrs);
    }
}