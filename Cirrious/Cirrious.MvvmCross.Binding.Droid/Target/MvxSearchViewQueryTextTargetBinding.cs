using System;
using Android.Widget;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxSearchViewQueryTextTargetBinding : MvxAndroidTargetBinding
    {
        public MvxSearchViewQueryTextTargetBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType
        {
            get { return typeof(string); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWayToSource; }
        }

        protected SearchView SearchView
        {
            get { return (SearchView)Target; }
        }

        public override void SubscribeToEvents()
        {
            SearchView.QueryTextChange += HandleQueryTextChanged;
        }

        protected override void SetValueImpl(object target, object value)
        {
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = Target as SearchView;
                if (target != null)
                {
                    target.QueryTextChange -= HandleQueryTextChanged;
                }
            }

            base.Dispose(isDisposing);
        }

        private void HandleQueryTextChanged(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            var target = Target as SearchView;

            if (target == null)
            {
                return;
            }

            var value = target.Query;
            FireValueChanged(value);
        }
    }
}