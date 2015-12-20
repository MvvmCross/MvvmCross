// DialogListFragment.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if false
using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using FooBar.Dialog.Droid.Elements;

namespace FooBar.Dialog.Droid
{
    public class DialogListFragment : ListFragment
    {
        public RootElement Root
        {
            get { return _dialogAdapter == null ? null : _dialogAdapter.Root; }
            set
            {
                value.ValueChanged -= HandleValueChangedEvent;
                value.ValueChanged += HandleValueChangedEvent;

                if (_dialogAdapter == null)
                    _dialogAdapter = new DialogAdapter(Activity, value);
                else
                    _dialogAdapter.Root = value;
            }
        }

        public override View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
        {
            ListAdapter = _dialogAdapter;
            return base.OnCreateView(p0, p1, p2);
        }

        public override void OnViewCreated(View p0, Bundle p1)
        {
            if (_dialogAdapter == null) return;
            _dialogAdapter.List = ListView;
            _dialogAdapter.RegisterListView();
            base.OnViewCreated(p0, p1);
        }

        private DialogAdapter _dialogAdapter;

        public event EventHandler ValueChanged;

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        public void ReloadData()
        {
            if (Root == null) return;
            _dialogAdapter.ReloadData();
        }
    }
}
#endif