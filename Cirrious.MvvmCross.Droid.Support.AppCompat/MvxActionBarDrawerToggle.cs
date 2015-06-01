using System;
using Android.App;
using Android.Views;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;

namespace Cirrious.MvvmCross.Droid.AppCompat {
    public class ActionBarDrawerEventArgs : EventArgs {
        public View DrawerView { get; private set; }

        public ActionBarDrawerEventArgs(View drawerView)
        {
            this.DrawerView = drawerView;
        }
    }

    public sealed class ActionBarDrawerSlideEventArgs : ActionBarDrawerEventArgs {
        public float SlideOffset { get; private set; }

        public ActionBarDrawerSlideEventArgs(View drawerView, float slideOffset)
            : base(drawerView)
        {
            this.SlideOffset = slideOffset;
        }
    }

    public sealed class ActionBarDrawerStateChangeEventArgs : EventArgs {
        public int NewState { get; private set; }

        public ActionBarDrawerStateChangeEventArgs(int newState)
        {
            this.NewState = newState;
        }
    }

    public sealed class ActionBarDrawerToggleWrapper : Android.Support.V7.App.ActionBarDrawerToggle {
        public ActionBarDrawerToggleWrapper(Activity activity, DrawerLayout drawerLayout, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes) { }
        public ActionBarDrawerToggleWrapper(Activity activity, DrawerLayout drawerLayout, Toolbar toolbar, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes) { }

        public event EventHandler<ActionBarDrawerEventArgs> DrawerClosed;
        public event EventHandler<ActionBarDrawerEventArgs> DrawerOpened;
        public event EventHandler<ActionBarDrawerSlideEventArgs> DrawerSlide;
        public event EventHandler<ActionBarDrawerStateChangeEventArgs> DrawerStateChanged;

        public override void OnDrawerClosed(View drawerView)
        {
            var handler = DrawerClosed;
            if (handler != null)
                handler(this, new ActionBarDrawerEventArgs(drawerView));

            base.OnDrawerClosed(drawerView);
        }

        public override void OnDrawerOpened(View drawerView)
        {
            var handler = DrawerOpened;
            if (handler != null)
                handler(this, new ActionBarDrawerEventArgs(drawerView));

            base.OnDrawerOpened(drawerView);
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            var handler = DrawerSlide;
            if (handler != null)
                handler(this, new ActionBarDrawerSlideEventArgs(drawerView, slideOffset));

            base.OnDrawerSlide(drawerView, slideOffset);
        }

        public override void OnDrawerStateChanged(int newState)
        {
            var handler = DrawerStateChanged;
            if (handler != null)
                handler(this, new ActionBarDrawerStateChangeEventArgs(newState));

            base.OnDrawerStateChanged(newState);
        }
    }
}