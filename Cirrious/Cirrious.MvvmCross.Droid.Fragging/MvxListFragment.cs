using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public abstract class MvxListFragment
        : MvxEventSourceListFragment
          , IMvxAndroidFragmentView
    {
        protected MvxListFragment()
        {
            this.AddEventListeners();
        }

        public IMvxBindingContext BindingContext { get; set; }

        public object DataContext { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }
    }
}