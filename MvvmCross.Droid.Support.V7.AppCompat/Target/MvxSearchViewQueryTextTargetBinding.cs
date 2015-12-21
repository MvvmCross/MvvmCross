using Android.Support.V7.Widget;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Target;
using System;

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    public class MvxSearchViewQueryTextTargetBinding : MvxAndroidTargetBinding
    {
        public MvxSearchViewQueryTextTargetBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWayToSource;

        protected SearchView SearchView => (SearchView)Target;

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