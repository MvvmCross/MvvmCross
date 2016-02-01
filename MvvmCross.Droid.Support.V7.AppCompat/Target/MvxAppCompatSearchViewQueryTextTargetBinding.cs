namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System;

    using Android.Support.V7.Widget;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Droid.Target;

    public class MvxAppCompatSearchViewQueryTextTargetBinding : MvxAndroidTargetBinding
    {
        public MvxAppCompatSearchViewQueryTextTargetBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWayToSource;

        protected SearchView SearchView => (SearchView)this.Target;

        public override void SubscribeToEvents()
        {
            this.SearchView.QueryTextChange += this.HandleQueryTextChanged;
        }

        protected override void SetValueImpl(object target, object value)
        {
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = this.Target as SearchView;
                if (target != null)
                {
                    target.QueryTextChange -= this.HandleQueryTextChanged;
                }
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