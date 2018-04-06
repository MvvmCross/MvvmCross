using System;
using System.Windows;
using System.Windows.Controls;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Wpf.Presenters
{
    public class MultiRegionWpfViewPresenter : MvxWpfViewPresenter
    {
        private readonly ContentControl _contentControl;

        public MultiRegionWpfViewPresenter(ContentControl contentControl)
            : base(contentControl)
        {
            _contentControl = contentControl;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var viewType = GetViewType(request);

            if (viewType.HasRegionAttribute())
            {
                var loader = Mvx.Resolve<IMvxWpfViewLoader>();
                var view = loader.CreateView(request);

                var region = viewType.GetRegionName();
                var containerView = GetChild<Frame>(_contentControl, region);

                if (containerView != null)
                {
                    containerView.Navigate(view);
                    return;
                }
            }

            base.Show(request);
        }

        private static Type GetViewType(MvxViewModelRequest request)
        {
            var viewFinder = Mvx.Resolve<IMvxViewsContainer>();
            return viewFinder.GetViewType(request.ViewModelType);
        }

        private static T GetChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            return LogicalTreeHelper.FindLogicalNode(parent, childName) as T;
        }
    }
}
