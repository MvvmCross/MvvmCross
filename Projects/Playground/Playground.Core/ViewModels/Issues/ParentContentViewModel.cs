using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ParentContentViewModel : MvxViewModel
    {
        private ChildContentViewModel _childViewModel1;
        public ChildContentViewModel ChildViewModel1
        {
            get => _childViewModel1;
            set
            {
                SetProperty(ref _childViewModel1, value);
            }
        }
        private ChildContentViewModel _childBindingContext2;
        public ChildContentViewModel ChildBindingContext2
        {
            get => _childBindingContext2;
            set
            {
                SetProperty(ref _childBindingContext2, value);
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
        public IMvxCommand ChangeButtonCmd1 => new MvxCommand(() => ChildViewModel1.Test = (ChildViewModel1.Test == "Bound Text 1" ? "Bound Text 2" : "Bound Text 1"));
        public IMvxCommand ToggleChild1EnabledCmd => new MvxCommand(() => ChildViewModelEnabled = !ChildViewModelEnabled);

        public IMvxCommand ChangeButtonCmd2 => new MvxCommand(() => ChildBindingContext2.Test = (ChildBindingContext2.Test == "Bound Text 1" ? "Bound Text 2" : "Bound Text 1"));

        public override void Prepare()
        {
            var vm = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>().LoadViewModel(MvxViewModelRequest<ChildContentViewModel>.GetDefaultRequest(), null) as ChildContentViewModel;
            vm.Test = "Child 1";
            ChildViewModel1 = vm;
            var bc = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>()
                    .LoadViewModel(MvxViewModelRequest<ChildContentViewModel>.GetDefaultRequest(), null) as
                ChildContentViewModel;
            bc.Test = "Child 2";
            ChildBindingContext2 = bc;
        }
    }
}
