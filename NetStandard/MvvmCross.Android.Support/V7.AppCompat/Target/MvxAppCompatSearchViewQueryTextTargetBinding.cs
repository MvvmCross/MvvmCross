using System;
using Android.Support.V7.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Platform.Droid.WeakSubscription;

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    public class MvxAppCompatSearchViewQueryTextTargetBinding 
        : MvxAndroidTargetBinding
    {
        private IDisposable _subscription;

        public MvxAppCompatSearchViewQueryTextTargetBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected SearchView SearchView => (SearchView)Target;

        public override void SubscribeToEvents()
        {
            _subscription = SearchView.WeakSubscribe<SearchView, SearchView.QueryTextChangeEventArgs>(
                nameof(SearchView.QueryTextChange),
                HandleQueryTextChanged);
        }

        protected override void SetValueImpl(object target, object value) =>
            ((SearchView)target).SetQuery((string)value, true);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }

            base.Dispose(isDisposing);
        }

        private void HandleQueryTextChanged(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (Target is SearchView searchView)
            {
                var value = searchView.Query;
                FireValueChanged(value);
            }
        }
    }
}