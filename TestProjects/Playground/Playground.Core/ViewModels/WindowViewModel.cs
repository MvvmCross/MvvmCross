using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class WindowChildParam
    {
        public int ParentNo { get; set; }
        public int ChildNo { get; set; }
    }

    public class WindowViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private static int _count;

        public string Title => $"No.{Count} Window View";

        public int Count { get; set; }

        public WindowViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            _count++;
            Count = _count;

            ShowWindowChildCommand = new MvxAsyncCommand<int>(async no =>
            {
                await _navigationService.Navigate<WindowChildViewModel, WindowChildParam>(new WindowChildParam
                {
                    ParentNo = Count,
                    ChildNo = no
                });
            });

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }
        public IMvxAsyncCommand<int> ShowWindowChildCommand { get; private set; }
    }
}
