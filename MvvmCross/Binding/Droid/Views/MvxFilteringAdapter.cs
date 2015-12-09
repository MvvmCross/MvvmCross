// MvxFilteringAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;
    using System.Threading;

    using Android.App;
    using Android.Content;
    using Android.Runtime;
    using Android.Widget;

    using Java.Lang;

    using MvvmCross.Platform.Platform;

    public class MvxFilteringAdapter
        : MvxAdapter, IFilterable
    {
        private class MyFilter : Filter
        {
            private readonly MvxFilteringAdapter _owner;

            public MyFilter(MvxFilteringAdapter owner)
            {
                this._owner = owner;
            }

            #region Overrides of Filter

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var stringConstraint = constraint == null ? string.Empty : constraint.ToString();

                var count = this._owner.SetConstraintAndWaitForDataChange(stringConstraint);

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

            #endregion Overrides of Filter
        }

        private int SetConstraintAndWaitForDataChange(string newConstraint)
        {
            MvxTrace.Trace("Wait starting for {0}", newConstraint);
            this._dataChangedEvent.Reset();
            this.PartialText = newConstraint;
            this._dataChangedEvent.WaitOne();
            MvxTrace.Trace("Wait finished with {1} items for {0}", newConstraint, this.Count);
            return this.Count;
        }

        private string _partialText;

        public event EventHandler PartialTextChanged;

        public string PartialText
        {
            get { return this._partialText; }
            private set
            {
                this._partialText = value;
                this.FireConstraintChanged();
            }
        }

        private void FireConstraintChanged()
        {
            var activity = this.Context as Activity;

            activity?.RunOnUiThread(() =>
            {
                var handler = this.PartialTextChanged;
                handler?.Invoke(this, EventArgs.Empty);
            });
        }

        private readonly ManualResetEvent _dataChangedEvent = new ManualResetEvent(false);

        public override void NotifyDataSetChanged()
        {
            this._dataChangedEvent.Set();
            base.NotifyDataSetChanged();
        }

        public MvxFilteringAdapter(Context context) : base(context)
        {
            this.ReturnSingleObjectFromGetItem = true;
            this.Filter = new MyFilter(this);
        }

        protected MvxFilteringAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public bool ReturnSingleObjectFromGetItem { get; set; }

        private MvxReplaceableJavaContainer _javaContainer;

        public override Java.Lang.Object GetItem(int position)
        {
            // for autocomplete views we need to return something other than null here
            // - see @JonPryor's answer in http://stackoverflow.com/questions/13842864/why-does-the-gref-go-too-high-when-i-put-a-mvxbindablespinner-in-a-mvxbindableli/13995199#comment19319057_13995199
            // - and see problem report in https://github.com/slodge/MvvmCross/issues/145
            // - obviously this solution is not good for general Java code!
            if (this.ReturnSingleObjectFromGetItem)
            {
                if (this._javaContainer == null)
                    this._javaContainer = new MvxReplaceableJavaContainer();
                this._javaContainer.Object = this.GetRawItem(position);
                return this._javaContainer;
            }

            return base.GetItem(position);
        }

        #region Implementation of IFilterable

        public Filter Filter { get; set; }

        #endregion Implementation of IFilterable
    }
}