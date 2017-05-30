using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Forms.ViewModels
{
    /// <summary>
    /// Extends IMvxViewModel interface to have track of the ViewModel used in Detail as root.
    /// Also, it exposes a method to be  launched when this root page is activated due to navigation popping in Detail
    /// 
    /// </summary>
    interface IMvxMasterDetailViewModel : IMvxViewModel
    {     
        void RootContentPageActivated();

        Type RootContentPageViewModelType { get; }
    }

    /// <summary>
    /// Generic version of IMvxViewModel interface    
    /// </summary>
    /// <typeparam name="TRootContentPageViewModel">ViewModel used for the first ContentPage used in Detail section of the MasterPage</typeparam>
    interface IMvxMasterDetailViewModel<TRootContentPageViewModel> : IMvxMasterDetailViewModel
        where TRootContentPageViewModel : IMvxViewModel
    {
        
    }
}
