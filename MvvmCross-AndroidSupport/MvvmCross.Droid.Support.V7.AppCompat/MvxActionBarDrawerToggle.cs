// MvxActionBarDrawerToggle.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public class ActionBarDrawerEventArgs : EventArgs
    {
        public ActionBarDrawerEventArgs(View drawerView)
        {
            DrawerView = drawerView;
        }

        public View DrawerView { get; }
    }

    public sealed class ActionBarDrawerSlideEventArgs : ActionBarDrawerEventArgs
    {
        public ActionBarDrawerSlideEventArgs(View drawerView, float slideOffset)
            : base(drawerView)
        {
            SlideOffset = slideOffset;
        }

        public float SlideOffset { get; }
    }

    public sealed class ActionBarDrawerStateChangeEventArgs : EventArgs
    {
        public ActionBarDrawerStateChangeEventArgs(int newState)
        {
            NewState = newState;
        }

        public int NewState { get; }
    }

    [Register("mvvmcross.droid.support.v7.appcompat.MvxActionBarDrawerToggle")]
    public sealed class MvxActionBarDrawerToggle : ActionBarDrawerToggle
    {
        public MvxActionBarDrawerToggle(IntPtr ptr, JniHandleOwnership ownership) : base(ptr, ownership)
        {
        }

        public MvxActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, int openDrawerContentDescRes,
            int closeDrawerContentDescRes)
            : base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes)
        {
        }

        public MvxActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, Toolbar toolbar,
            int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes)
        {
        }

        public event EventHandler<ActionBarDrawerEventArgs> DrawerClosed;

        public event EventHandler<ActionBarDrawerEventArgs> DrawerOpened;

        public event EventHandler<ActionBarDrawerSlideEventArgs> DrawerSlide;

        public event EventHandler<ActionBarDrawerStateChangeEventArgs> DrawerStateChanged;

        public override void OnDrawerClosed(View drawerView)
        {
            DrawerClosed?.Invoke(this, new ActionBarDrawerEventArgs(drawerView));

            base.OnDrawerClosed(drawerView);
        }

        public override void OnDrawerOpened(View drawerView)
        {
            DrawerOpened?.Invoke(this, new ActionBarDrawerEventArgs(drawerView));

            base.OnDrawerOpened(drawerView);
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            DrawerSlide?.Invoke(this, new ActionBarDrawerSlideEventArgs(drawerView, slideOffset));

            base.OnDrawerSlide(drawerView, slideOffset);
        }

        public override void OnDrawerStateChanged(int newState)
        {
            DrawerStateChanged?.Invoke(this, new ActionBarDrawerStateChangeEventArgs(newState));

            base.OnDrawerStateChanged(newState);
        }
    }
}