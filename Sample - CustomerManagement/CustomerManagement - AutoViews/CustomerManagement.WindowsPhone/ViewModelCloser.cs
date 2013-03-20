using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using CustomerManagement.AutoViews.Core.Interfaces;
using Microsoft.Phone.Controls;

namespace CustomerManagement.AutoViews.WindowsPhone
{
    public class ViewModelCloser : IViewModelCloser
    {
        private readonly PhoneApplicationFrame _frame;

        public ViewModelCloser(PhoneApplicationFrame frame)
        {
            _frame = frame;
        }

        public void RequestClose(IMvxViewModel viewModel)
        {
            var topPage = _frame.Content;
            var view = topPage as IMvxView;

            if (view == null)
            {
                MvxTrace.Trace("request close ignored for {0} - no current view", viewModel.GetType().Name);
                return;
            }

            if (view.ViewModel != viewModel)
            {
                MvxTrace.Trace("request close ignored for {0} - current view is registered for a different viewmodel of type {1}", viewModel.GetType().Name, view.ViewModel.GetType().Name);
                return;
            }

            MvxTrace.Trace("request close for {0} - will close current page {1}", viewModel.GetType().Name, view.GetType().Name);
            _frame.GoBack();
        }
    }
}