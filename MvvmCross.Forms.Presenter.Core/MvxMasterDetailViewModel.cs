using MvvmCross.Core.ViewModels;
using System;

namespace MvvmCross.Forms.Presenter.Core
{
    /// <summary>
    /// Base class for IMvxMasterDetailViewModel, extending from MvxViewModel
    /// </summary>
    public class MvxMasterDetailViewModel : MvxViewModel, IMvxMasterDetailViewModel
    {
        public Type RootContentPageViewModelType { get; protected set; }

        public virtual void RootContentPageActivated()
        {

        }
    }

    /// <summary>
    /// Generic typed version of MvxMasterDetailViewModel
    /// </summary>
    /// <typeparam name="TRootContentPageViewModel">ViewModel used for the first ContentPage used in Detail section of the MasterPage</typeparam>
    public class MvxMasterDetailViewModel<TRootContentPageViewModel> : MvxMasterDetailViewModel,
        IMvxMasterDetailViewModel<TRootContentPageViewModel> where TRootContentPageViewModel : IMvxViewModel
    {

        public MvxMasterDetailViewModel()
        {
            RootContentPageViewModelType = typeof(TRootContentPageViewModel);
        }
    }
}
