using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ParentContentViewModel : MvxViewModel
    {
        private ChildContentViewModel _childViewModel;
        public ChildContentViewModel ChildViewModel1
        {
            get => _childViewModel;
            set
            {
                SetProperty(ref _childViewModel, value);
            }
        }
        private bool _childViewModelEnabled;
        public bool ChildViewModelEnabled
        {
            get => _childViewModelEnabled;
            set
            {
                SetProperty(ref _childViewModelEnabled, value);
            }
        }
        public IMvxCommand ChangeButtonCmd => new MvxCommand(() => ChildViewModel1.Test = (ChildViewModel1.Test == "Bound Text 1" ? "Bound Text 2" : "Bound Text 1"));
        public IMvxCommand ToggleChildEnabledCmd => new MvxCommand(() => ChildViewModelEnabled = !ChildViewModelEnabled);

        public override void Prepare()
        {
            ChildViewModel1 = Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(MvxViewModelRequest<ChildViewModel>.GetDefaultRequest(), null) as ChildContentViewModel;
        }
    }
}
