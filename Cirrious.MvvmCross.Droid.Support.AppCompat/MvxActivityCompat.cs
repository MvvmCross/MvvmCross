// MvxActivityCompat.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Java.Lang;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Support.AppCompat
{
    [Register("cirrious.mvvmcross.droid.support.appcompat.MvxActivityCompat")]
    public class MvxActivityCompat : MvxAppCompatActivity
    {
        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            View view = MvxAppCompatActivityHelper.OnCreateView(parent, name, context, attrs);
            return view ?? base.OnCreateView(parent, name, context, attrs);
        }
    }

    public abstract class MvxActivityCompat<TViewModel>
        : MvxActivityCompat
    , IMvxAndroidView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}