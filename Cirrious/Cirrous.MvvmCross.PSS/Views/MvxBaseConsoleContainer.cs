using Cirrious.MvvmCross.Pss.Interfaces;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Pss.Views
{
    public abstract class MvxBasePssContainer 
        : MvxViewsContainer
        , IMvxPssNavigation
    {
        public abstract void Navigate(MvxShowViewModelRequest request);
        public abstract void GoBack();
        public abstract void RemoveBackEntry();
        public abstract bool CanGoBack();
    }
}