// DialogActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Views;
using Android.Widget;
using CrossUI.Droid.Dialog.Elements;
using System;
using System.Linq;

namespace CrossUI.Droid.Dialog
{
    public class DialogActivity : ListActivity
    {
        public DialogActivity()
        {
        }

        private DialogAdapter _dialogAdapter;
        private View _viewWithLastFocus;
        private bool _contentHasBeenSet = false;

        public override void OnContentChanged()
        {
            if (_contentHasBeenSet && ListView != null)
            {
                ListView.ViewTreeObserver.GlobalFocusChange -= OnViewTreeObserverOnGlobalFocusChange;
                ListView.ViewTreeObserver.GlobalLayout -= OnViewTreeObserverOnGlobalLayout;
            }
            base.OnContentChanged();

            _contentHasBeenSet = true;
            ListView.DescendantFocusability = DescendantFocusability.AfterDescendants;
            ListView.ItemsCanFocus = true;

            ListView.ViewTreeObserver.GlobalFocusChange += OnViewTreeObserverOnGlobalFocusChange;
            ListView.ViewTreeObserver.GlobalLayout += OnViewTreeObserverOnGlobalLayout;
        }

        private void OnViewTreeObserverOnGlobalLayout(object sender, EventArgs args)
        {
            if (_viewWithLastFocus != null)
            {
                _viewWithLastFocus.RequestFocus();
                _viewWithLastFocus.RequestFocusFromTouch();
                _viewWithLastFocus = null;
            }
        }

        private void OnViewTreeObserverOnGlobalFocusChange(object sender, ViewTreeObserver.GlobalFocusChangeEventArgs args)
        {
            if (args.NewFocus == null)
            {
                _viewWithLastFocus = null;
                return;
            }
            if (args.NewFocus != ListView)
            {
                //check if it's one of our's
                var parent = args.NewFocus.Parent;
                while (parent != null && parent != parent.Parent)
                {
                    if (parent == ListView)
                    {
                        _viewWithLastFocus = args.NewFocus;
                        break;
                    }
                    parent = parent.Parent;
                }
            }
        }

        public RootElement Root
        {
            get { return _dialogAdapter?.Root; }
            set
            {
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                _dialogAdapter?.DeregisterListView();

                ListAdapter = _dialogAdapter = new DialogAdapter(this, value, ListView);
            }
        }

        public void HandleValueChangedEvents(EventHandler eventHandler)
        {
            foreach (var element in Root.Sections.SelectMany(section => section))
            {
                if (element is ValueElement)
                    (element as ValueElement).ValueChanged += eventHandler;
            }
        }

        public event EventHandler ValueChanged;

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            ValueChanged?.Invoke(sender, args);
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            return null;
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}