// MvxActionBarDrawerToggle.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public class ActionBarDrawerEventArgs : EventArgs 
    {
        public View DrawerView { get; private set; }

        public ActionBarDrawerEventArgs(View drawerView)
        {
            DrawerView = drawerView;
        }
    }

    public sealed class ActionBarDrawerSlideEventArgs : ActionBarDrawerEventArgs 
    {
        public float SlideOffset { get; private set; }

        public ActionBarDrawerSlideEventArgs(View drawerView, float slideOffset)
            : base(drawerView)
        {
            SlideOffset = slideOffset;
        }
    }

    public sealed class ActionBarDrawerStateChangeEventArgs : EventArgs 
    {
        public int NewState { get; private set; }

        public ActionBarDrawerStateChangeEventArgs(int newState)
        {
            NewState = newState;
        }
    }

    public sealed class MvxActionBarDrawerToggle : Android.Support.V7.App.ActionBarDrawerToggle 
    {
        public MvxActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes) { }

        public MvxActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, Toolbar toolbar, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes) { }

        public event EventHandler<ActionBarDrawerEventArgs> DrawerClosed;
        public event EventHandler<ActionBarDrawerEventArgs> DrawerOpened;
        public event EventHandler<ActionBarDrawerSlideEventArgs> DrawerSlide;
        public event EventHandler<ActionBarDrawerStateChangeEventArgs> DrawerStateChanged;

        public override void OnDrawerClosed(View drawerView)
        {
            var handler = DrawerClosed;
            handler?.Invoke(this, new ActionBarDrawerEventArgs(drawerView));

            base.OnDrawerClosed(drawerView);
        }

        public override void OnDrawerOpened(View drawerView)
        {
            var handler = DrawerOpened;
            handler?.Invoke(this, new ActionBarDrawerEventArgs(drawerView));

            base.OnDrawerOpened(drawerView);
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            var handler = DrawerSlide;
            handler?.Invoke(this, new ActionBarDrawerSlideEventArgs(drawerView, slideOffset));

            base.OnDrawerSlide(drawerView, slideOffset);
        }

        public override void OnDrawerStateChanged(int newState)
        {
            var handler = DrawerStateChanged;
            handler?.Invoke(this, new ActionBarDrawerStateChangeEventArgs(newState));

            base.OnDrawerStateChanged(newState);
        }
    }
}