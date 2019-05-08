using System;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Android.Util;
using Android.Views;
using Android.Support.Design.Widget;
using System.Linq;

namespace MvvmCross.Droid.Support.Design
{
    [Register("mvvmcross.droid.support.design.MvxBottomNavigationView")]
    public class MvxBottomNavigationView : BottomNavigationView, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private readonly Dictionary<IMenuItem, Type> _lookup = new Dictionary<IMenuItem, Type>();

        public MvxBottomNavigationView(Context context) : base(context)
        {
            this.SetOnNavigationItemSelectedListener(this);
        }

        public MvxBottomNavigationView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.SetOnNavigationItemSelectedListener(this);
        }

        public MvxBottomNavigationView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            this.SetOnNavigationItemSelectedListener(this);
        }

        protected MvxBottomNavigationView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public void AddItem(IMenuItem item, Type viewModel)
        {
            _lookup.Add(item, viewModel);

            // The first item is autoselected
            if (_lookup.Count == 1)
            {
                OnNavigationItemSelected(item);
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var viewModelType = this.FindItemByMenuItem(item);

            if (viewModelType != null && HandleNavigate.CanExecute(viewModelType))
            {
                HandleNavigate.Execute(viewModelType);
                return true;
            }

            return false;
        }

        public IMenuItem FindItemByViewModel(Type viewModel)
        {
            return _lookup.FirstOrDefault(i => i.Value == viewModel).Key;
        }

        public Type FindItemByMenuItem(IMenuItem item)
        {
            return _lookup[item];
        }

        public ICommand HandleNavigate { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SetOnNavigationItemSelectedListener(null);
            }

            base.Dispose(disposing);
        }
    }
}
