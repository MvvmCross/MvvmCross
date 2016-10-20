using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System;

    using Android.Support.V7.Widget;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Droid.Target;

    public class MvxAppCompatSearchViewQueryTextTargetBinding : MvxAndroidTargetBinding
    {
        private IDisposable _subscription;

        public MvxAppCompatSearchViewQueryTextTargetBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWayToSource;

        protected SearchView SearchView => (SearchView)this.Target;

        public override void SubscribeToEvents()
        {
            _subscription = SearchView.WeakSubscribe<SearchView, SearchView.QueryTextChangeEventArgs>(
                nameof(SearchView.QueryTextChange),
                HandleQueryTextChanged);
        }

        protected override void SetValueImpl(object target, object value)
        {
        }

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
            var target = this.Target as SearchView;

            if (target == null)
            {
                return;
            }

            var value = target.Query;
            this.FireValueChanged(value);
        }
    }
}