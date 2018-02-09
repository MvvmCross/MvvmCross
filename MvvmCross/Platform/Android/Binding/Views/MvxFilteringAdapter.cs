﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Java.Lang;
using MvvmCross.Logging;
using MvvmCross.Platform.Android.Base;
using MvvmCross.Platform.Android.Binding.BindingContext;
using Object = Java.Lang.Object;
using String = Java.Lang.String;

namespace MvvmCross.Platform.Android.Binding.Views
{
    public class MvxFilteringAdapter
        : MvxAdapter, IFilterable
    {
        private class MyFilter : Filter
        {
            private readonly MvxFilteringAdapter _owner;

            public MyFilter(MvxFilteringAdapter owner)
            {
                _owner = owner;
            }

            #region Overrides of Filter

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var stringConstraint = constraint == null ? string.Empty : constraint.ToString();

                var count = _owner.SetConstraintAndWaitForDataChange(stringConstraint);

                return new FilterResults
                {
                    Count = count
                };
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                // force a refresh
                _owner.NotifyDataSetInvalidated();
            }

            public override ICharSequence ConvertResultToStringFormatted(Object resultValue)
            {
                var ourContainer = resultValue as MvxJavaContainer;
                if (ourContainer == null)
                {
                    return base.ConvertResultToStringFormatted(resultValue);
                }

                return new String(ourContainer.Object.ToString());
            }

            #endregion Overrides of Filter
        }

        private int SetConstraintAndWaitForDataChange(string newConstraint)
        {
            if (PartialText == newConstraint)
            {
                return Count;
            }
            
            MvxLog.Instance.Trace("Wait starting for {0}", newConstraint);
            _dataChangedEvent.Reset();
            PartialText = newConstraint;
            _dataChangedEvent.WaitOne();
            MvxLog.Instance.Trace("Wait finished with {1} items for {0}", newConstraint, Count);
            return Count;
        }

        private string _partialText;

        public event EventHandler PartialTextChanged;

        public string PartialText
        {
            get
            {
                return _partialText;
            }
            private set
            {
                _partialText = value;
                FireConstraintChanged();
            }
        }

        private void FireConstraintChanged()
        {
            var activity = Context as Activity;

            activity?.RunOnUiThread(() =>
            {
                PartialTextChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        private readonly ManualResetEvent _dataChangedEvent = new ManualResetEvent(false);

        public override void NotifyDataSetChanged()
        {
            _dataChangedEvent.Set();
            base.NotifyDataSetChanged();
        }

        public MvxFilteringAdapter(Context context) : this(context, MvxAndroidBindingContextHelpers.Current())
        {
        }

        public MvxFilteringAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
            ReturnSingleObjectFromGetItem = true;
            Filter = new MyFilter(this);
        }
        
        protected MvxFilteringAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public bool ReturnSingleObjectFromGetItem { get; set; }

        private MvxReplaceableJavaContainer _javaContainer;

        public override Object GetItem(int position)
        {
            // for autocomplete views we need to return something other than null here
            // - see @JonPryor's answer in http://stackoverflow.com/questions/13842864/why-does-the-gref-go-too-high-when-i-put-a-mvxbindablespinner-in-a-mvxbindableli/13995199#comment19319057_13995199
            // - and see problem report in https://github.com/slodge/MvvmCross/issues/145
            // - obviously this solution is not good for general Java code!
            if (ReturnSingleObjectFromGetItem)
            {
                if (_javaContainer == null)
                    _javaContainer = new MvxReplaceableJavaContainer();
                _javaContainer.Object = GetRawItem(position);
                return _javaContainer;
            }

            return base.GetItem(position);
        }

        #region Implementation of IFilterable

        public Filter Filter { get; set; }

        #endregion Implementation of IFilterable
    }
}
