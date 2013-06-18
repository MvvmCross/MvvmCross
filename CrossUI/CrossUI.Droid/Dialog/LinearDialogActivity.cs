// DialogActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CrossUI.Droid.Dialog.Elements;
using Java.Lang;

namespace CrossUI.Droid.Dialog
{
    public class CustomDataSetObserver: DataSetObserver
    {

        public event EventHandler Changed;
        public event EventHandler Invalidated;

        public override void OnChanged()
        {
            base.OnChanged();
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
        public override void OnInvalidated()
        {
            base.OnInvalidated();
            if (Invalidated != null)
                Invalidated(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// DialogActivity based on a linear view, this will solve all edittext related focus problems when using elements 
    /// suggestions at http://stackoverflow.com/questions/2679948/focusable-edittext-inside-listview doesn't help for example
    /// </summary>
    public class LinearDialogActivity : Activity
    {
        public RootElement Root
        {
            get { return _dialogAdapter == null ? null : _dialogAdapter.Root; }
            set
            {
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter != null)
                {
                    _dialogAdapter.DeregisterListView();
                    _dialogAdapter.UnregisterDataSetObserver(_observer);
                }

                _dialogAdapter = new DialogAdapter(this, value);
                if (_observer == null)
                {
                    _observer = new CustomDataSetObserver();
                    _observer.Changed += ObserverOnChanged;
                    _observer.Invalidated += ObserverOnChanged;
                }
                _dialogAdapter.RegisterDataSetObserver(_observer);
                
                if (_list != null)
                    AddViews();
            }
        }

        private void ObserverOnChanged(object sender, EventArgs eventArgs)
        {
            AddViews();
        }

        public override void OnContentChanged()
        {
            base.OnContentChanged();
            _list = FindViewById<ViewGroup>(Android.Resource.Id.List);

            if (_list == null || _list is ListView)
            {
                throw new RuntimeException("Your content must have a ViewGroup whose id attribute is Android.Resource.Id.List and is not of type ListView");
            }
            if (_dialogAdapter != null)
                AddViews();
        }


        private int _TAG_INDEX = 82171829;
        public void AddViews()
        {
            if (_dialogAdapter == null || _list == null)
                return;
            _list.RemoveAllViews();
            for (var i = 0; i < _dialogAdapter.Count; i++)
            {
                var view = _dialogAdapter.GetView(i, null, _list);
                view.SetTag(_TAG_INDEX, i);
                view.Click -= ListView_ItemClick;
                view.LongClick -= ListView_ItemLongClick;
                view.Click += ListView_ItemClick;
                view.LongClick +=ListView_ItemLongClick;

                view.Focusable = true;
                view.FocusableInTouchMode = true;
                view.Clickable = true;
                view.LongClickable = true;

                _list.AddView(view);
            }
        }

        public void ListView_ItemClick(object sender, EventArgs eventArgs)
        {
            var position = (int) ((View) sender).GetTag(_TAG_INDEX);
            var elem = _dialogAdapter.ElementAtIndex(position);
            if (elem == null) return;
            elem.Selected();
            if (elem.Click != null)
                elem.Click(this, EventArgs.Empty);
        }

        public void ListView_ItemLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            var position = (int)((View)sender).GetTag(_TAG_INDEX);
            var elem = _dialogAdapter.ElementAtIndex(position);
            if (elem == null) return;
            if (elem.LongClick != null)
                elem.LongClick(this, EventArgs.Empty);
        }

        private DialogAdapter _dialogAdapter;
        private CustomDataSetObserver _observer;
        private ViewGroup _list;

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
            if (ValueChanged != null)
                ValueChanged(sender, args);
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