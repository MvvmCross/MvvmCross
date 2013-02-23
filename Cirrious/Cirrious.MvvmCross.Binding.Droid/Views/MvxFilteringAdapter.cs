// MvxFilteringAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Widget;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Platform.Diagnostics;
using Java.Lang;

namespace Cirrious.MvvmCross.Binding.Droid.Views
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

        public MvxFilteringAdapter(Context context) : base(context)
        {
            Filter = new MyFilter(this);
        }

        #region Implementation of IFilterable

        public Filter Filter { get; set; }

        #endregion
    }
}