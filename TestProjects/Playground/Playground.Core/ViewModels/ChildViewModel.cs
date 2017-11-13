using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using Playground.Core.Models;

namespace Playground.Core.ViewModels
{
    public class ChildViewModel : MvxViewModel<SampleModel>
    {
        private readonly IMvxNavigationService _navigationService;

        private SampleModel _parameter;

        public ChildViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));

            ShowSecondChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SecondChildViewModel>());

            ShowRootCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<RootViewModel>());
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        public override void Prepare(SampleModel parameter)
        {
            _parameter = parameter;
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            base.SaveStateToBundle(bundle);
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            base.ReloadFromBundle(state);
        }

        public async override System.Threading.Tasks.Task Initialize()
        {
            await base.Initialize();

            await Task.Delay(8500);
        }

        public void Init()
        {
        }

        public override void Start()
        {
            base.Start();
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowSecondChildCommand { get; private set; }

        public IMvxAsyncCommand ShowRootCommand { get; private set; }
    }
}
