using System;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Android.Util;
using Android.Views;
using Android.Support.Design.Widget;

namespace MvvmCross.Droid.Support.Design
{
    [Register("mvvmcross.droid.support.design.MvxBottomNavigationView")]
    public class MvxBottomNavigationView : BottomNavigationView, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        public List<MvxMenuItem> ItemsSource = new List<MvxMenuItem>();

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
            ItemsSource.Add(new MvxMenuItem(item, viewModel));

            // The first item is autoselected
            if (ItemsSource.Count == 1)
            {
                OnNavigationItemSelected(item);
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var menuItem = this.FindItemByMenuItem(item);

            if (menuItem != null && HandleNavigate.CanExecute(menuItem.ViewModel))
            {
                HandleNavigate.Execute(menuItem.ViewModel);
                return true;
            }

            return false;
        }

        public MvxMenuItem FindItemByViewModel(Type viewModel)
        {
            return ItemsSource.Find(i => i.ViewModel == viewModel);
        }

        public int FindPositionByViewModel(Type viewModel)
        {
            return ItemsSource.FindIndex(i => i.ViewModel == viewModel);
        }

        public MvxMenuItem FindItemByMenuItem(IMenuItem item)
        {
            return ItemsSource.Find(i => i.MenuItem == item);
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

    public class MvxMenuItem
    {
        public IMenuItem MenuItem { get; set; }
        public Type ViewModel { get; set; }

        public MvxMenuItem(IMenuItem menuItem, Type viewModel)
        {
            this.MenuItem = menuItem;
            this.ViewModel = viewModel;
        }
    }
}
