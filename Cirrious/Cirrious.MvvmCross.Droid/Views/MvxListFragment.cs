using System;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxListResources
    {
        public int Container;
        public int List;
        public int Item;
    }

    public class MvxListFragment : MvxFragment
    {
        private ViewGroup _layout;

        MvxListView _listView;
        public MvxListView ListView
        {
            get { return _listView; }
            set { _listView = value; }
        }                        

        private MvxListResources _listResources;
        public MvxListResources ListResources
        {
            get { return _listResources; }
            set { _listResources = value; }
        }

        public Dictionary<int, string> _listItemBindings;
        public Dictionary<int, string> ListItemBindings
        {
            get
            {
                if (_listItemBindings == null)
                {
                    _listItemBindings = new Dictionary<int, string>();
                }
                return _listItemBindings;
            }
            set { _listItemBindings = value; }
        }

        public MvxListFragment(Activity activity, MvxListResources listResources = null, Dictionary<int, string> listItemBindings = null) : base(activity) 
        {
            if (listResources != null)
            {
                _listResources = listResources;
            }

            _listResources = listResources;
            if (listItemBindings != null)
            {
                _listItemBindings = listItemBindings;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _layout = (ViewGroup)BindingInflate(ListResources.Container);
            ListView = _layout.FindViewById<MvxListView>(ListResources.List);
            ListView.ItemTemplateId = _listResources.Item;

            IMvxBindingDescriptionContainer bindingDescriptionContainer = Activity as IMvxBindingDescriptionContainer;
            if (bindingDescriptionContainer != null)
            {
                foreach (KeyValuePair<int, string> entry in ListItemBindings)
                {
                    if (!bindingDescriptionContainer.BindingDescriptions.ContainsKey(entry.Key))
                    {
                        bindingDescriptionContainer.BindingDescriptions.Add(entry.Key, entry.Value);
                    }
                }
            }

            return _layout;
        }
    }
}

