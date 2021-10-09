﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.DrawerLayout.Widget;

namespace MvvmCross.Platforms.Android.Views.AppCompat
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

    [Register("mvvmcross.platforms.android.views.appcompat.MvxActionBarDrawerToggle")]
    public sealed class MvxActionBarDrawerToggle : ActionBarDrawerToggle
    {
        public MvxActionBarDrawerToggle(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
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
