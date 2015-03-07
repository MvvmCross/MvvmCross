// MvxRadioGroup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using System.Collections;
using System.Collections.Specialized;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxRadioGroup : RadioGroup, IMvxWithChangeAdapter
    {
        public MvxRadioGroup(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxAdapterWithChangedEvent(context))
        {
        }


        public MvxRadioGroup(Context context, IAttributeSet attrs, IMvxAdapterWithChangedEvent adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            if (adapter != null)
            {
                Adapter = adapter;
                Adapter.ItemTemplateId = itemTemplateId;
            }

            this.ChildViewAdded += OnChildViewAdded;
            this.ChildViewRemoved += OnChildViewRemoved;
        }

		protected MvxRadioGroup(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
	    {
	    }

        public void AdapterOnDataSetChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            this.UpdateDataSetFromChange(sender, eventArgs);
        }


        int _generatedId = 1000;


        void OnChildViewAdded(object sender, Android.Views.ViewGroup.ChildViewAddedEventArgs args)
        {
            var li = (args.Child as MvxListItemView);
            if (li != null)
            {
                var radioButton = (li.GetChildAt(0) as RadioButton);
                if (radioButton != null)
                {
                    // radio buttons require an id so that they get un-checked correctly
                    if (radioButton.Id == Android.Views.View.NoId)
                    {
                        _generatedId += 1;
                        int rid = _generatedId;
                        radioButton.Id = rid;
                    }
                    radioButton.CheckedChange += OnRadioButtonCheckedChange;
                }
            }
        }


        private void OnRadioButtonCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs args)
        {
            var radionButton = (sender as RadioButton);
            if (radionButton != null)
            {
                this.Check(radionButton.Id);
            }
        }


        private void OnChildViewRemoved(object sender, ChildViewRemovedEventArgs childViewRemovedEventArgs)
        {
            var boundChild = childViewRemovedEventArgs.Child as IMvxBindingContextOwner;
            if (boundChild != null)
            {
                boundChild.ClearAllBindings();
            }
        }


        private IMvxAdapterWithChangedEvent _adapter;

        public IMvxAdapterWithChangedEvent Adapter
        {
            get { return _adapter; }
            protected set
            {
                var existing = _adapter;
                if (existing == value)
                {
                    return;
                }


                if (existing != null)
                {
                    existing.DataSetChanged -= AdapterOnDataSetChanged;
                    if (value != null)
                    {
                        value.ItemsSource = existing.ItemsSource;
                        value.ItemTemplateId = existing.ItemTemplateId;
                    }
                }


                _adapter = value;


                if (_adapter != null)
                {
                    _adapter.DataSetChanged += AdapterOnDataSetChanged;
                }


                if (_adapter == null)
                {
                    MvxBindingTrace.Warning(
                        "Setting Adapter to null is not recommended - you may lose ItemsSource binding when doing this");
                }
            }
        }


        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return Adapter.ItemsSource; }
            set { Adapter.ItemsSource = value; }
        }


        public int ItemTemplateId
        {
            get { return Adapter.ItemTemplateId; }
            set { Adapter.ItemTemplateId = value; }
        }
    }
}

