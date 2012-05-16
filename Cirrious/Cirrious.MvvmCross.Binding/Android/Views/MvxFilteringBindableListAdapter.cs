#region Copyright
// <copyright file="MvxFilteringBindableListAdapter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Widget;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Java.Lang;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxFilteringBindableListAdapter
        : MvxBindableListAdapter, IFilterable
    { 
        private class MyFilter : Filter
        {
            private readonly MvxFilteringBindableListAdapter _owner;

            public MyFilter(MvxFilteringBindableListAdapter owner)
            {
                _owner = owner;
            }

            #region Overrides of Filter

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var stringConstraint = constraint == null ? string.Empty : constraint.ToString();

                var count = _owner.SetConstraintAndWaitForDataChange(stringConstraint);

                return new FilterResults()
                           {
                               Count = count
                           };
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                // force a refresh
                this._owner.NotifyDataSetInvalidated();
            }

            public override ICharSequence ConvertResultToStringFormatted(Java.Lang.Object resultValue)
            {
                var ourContainer = resultValue as MvxJavaContainer;
                if (ourContainer == null)
                {
                    return base.ConvertResultToStringFormatted(resultValue);
                }

                return new Java.Lang.String(ourContainer.Object.ToString());
            }

            #endregion
        }

        private int SetConstraintAndWaitForDataChange(string newConstraint)
        {
            MvxTrace.Trace("Wait starting for {0}", newConstraint);
            _dataChangedEvent.Reset();
            this.PartialText = newConstraint;
            _dataChangedEvent.WaitOne();
            MvxTrace.Trace("Wait finished with {1} items for {0}", newConstraint, Count);
            return Count;
        }

        private string _partialText;

        public event EventHandler PartialTextChanged;

        public string PartialText
        {
            get { return _partialText; }
            private set
            {
                _partialText = value; 
                FireConstraintChanged();
            }
        }

        private void FireConstraintChanged()
        {
            var activity = Context as Activity;

            if (activity == null)
                return;

            activity.RunOnUiThread(() =>
                                       {
                                           var handler = PartialTextChanged;
                                           if (handler != null)
                                               handler(this, EventArgs.Empty);
                                       });
        }

        private readonly ManualResetEvent _dataChangedEvent = new ManualResetEvent(false);

        public override void NotifyDataSetChanged()
        {
            _dataChangedEvent.Set();
            base.NotifyDataSetChanged();
        }

        public MvxFilteringBindableListAdapter(Context context) : base(context)
        {
            Filter = new MyFilter(this);
        }

        #region Implementation of IFilterable

        public Filter Filter { get; set; }

        #endregion
    }
}