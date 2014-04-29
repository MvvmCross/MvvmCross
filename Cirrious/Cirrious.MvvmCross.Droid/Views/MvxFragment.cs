// MvxFragmentActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
//
// @author: Anass Bouassaba <anass.bouassaba@digitalpatrioten.com>

using System.Collections.Generic;
using Android.Content;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using Android.Views;
using Android.App;

namespace Cirrious.MvvmCross.Droid.Views
{
    public abstract class MvxFragment
        : MvxEventSourceFragment
            , IMvxAndroidView
            , IMvxTextBindingContainer
    {
        Activity _fragmentActivity;
        public Activity FragmentActivity
        {
            get { return _fragmentActivity; }
            set { _fragmentActivity = value; }
        }

        int _layoutId;
        public int LayoutId
        {
            get { return _layoutId; }
            set { _layoutId = value; }
        }

        #region IMvxBindingTextContainer implementation

        Dictionary<Android.Views.View, string> _textBindings;
        public Dictionary<Android.Views.View, string> TextBindings
        {
            get
            {
                if (_textBindings == null)
                    _textBindings = new Dictionary<Android.Views.View, string>();
                return _textBindings;
            }
            set
            {
                _textBindings = value;
            }
        }

        #endregion

        public IDictionary<string, string> ParameterValues { get; set; }

        public MvxFragment(Activity activity, string title = "")
        {
            this.FragmentActivity = activity;

            BindingContext = new MvxAndroidBindingContext(FragmentActivity, this);
            this.AddEventListeners();
        }

        public void Init(Activity activity)
        {
            BindingContext = new MvxAndroidBindingContext(FragmentActivity, this);
            this.AddEventListeners();
        }

        public LayoutInflater LayoutInflater
        {
            get {
                return this.Activity.LayoutInflater;
            }
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        public IMvxBindingContext BindingContext { get; set; }

        public View BindingInflate(int layoutResId)
        {
            var view = this.BindingInflate(layoutResId, null);
            return view;
        }

        protected virtual void OnViewModelSet()
        {
        }
    }
}

