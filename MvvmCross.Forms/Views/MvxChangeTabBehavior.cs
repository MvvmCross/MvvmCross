using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxChangeTabBehavior<TParameter> : Behavior<MvxTabbedPage>
    {
        public MvxTabbedPage AssociatedObject { get; private set; }
        private IMvxChangeTabAware<TParameter> _oldViewModel;

        protected override void OnAttachedTo(MvxTabbedPage bindable)
        {
            bindable.CurrentPageChanged += Bindable_CurrentPageChanged;
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
        }
        protected override void OnDetachingFrom(MvxTabbedPage bindable)
        {
            bindable.CurrentPageChanged -= Bindable_CurrentPageChanged;
            base.OnDetachingFrom(bindable);
            AssociatedObject = null;
        }

        private void Bindable_CurrentPageChanged(object sender, EventArgs e)
        {
            if (AssociatedObject == null)
                return;

            if (AssociatedObject.CurrentPage is IMvxPage p)
            {
                var viewModel = p.ViewModel as IMvxChangeTabAware<TParameter>;
                viewModel?.OnNavigatedTo(_oldViewModel != null ? _oldViewModel.OnNavigatedFrom() : default);
                _oldViewModel = viewModel;
            }
        }
    }
}
