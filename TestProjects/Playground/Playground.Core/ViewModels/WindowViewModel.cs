using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using Playground.Core.Models;

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

        private Modes _mode = Modes.Blue;
        public Modes Mode {
            get { return _mode; }
            set {
                if (value == _mode) return;
                _mode = value;
                RaisePropertyChanged(() => Mode);
            }
        }

        private bool _isItem1 = true;
        public bool IsItem1 {
            get { return _isItem1; }
            set { 
                if (value == _isItem1) return;
                _isItem1 = value;
                RaisePropertyChanged(() => IsItem1);
            }
        }

        private bool _isItem2 = false;
        public bool IsItem2
        {
            get { return _isItem2; }
            set
            {
                if (value == _isItem2) return;
                _isItem2 = value;
                RaisePropertyChanged(() => IsItem2);
            }
        }

        private bool _isItem3 = false;
        public bool IsItem3
        {
            get { return _isItem3; }
            set
            {
                if (value == _isItem3) return;
                _isItem3 = value;
                RaisePropertyChanged(() => IsItem3);
            }
        }

        private bool _isItemSetting = true;
        public bool IsItemSetting
        {
            get { return _isItemSetting; }
            set
            {
                if (value == _isItemSetting) return;
                _isItemSetting = value;
                RaisePropertyChanged(() => IsItemSetting);
            }
        }

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

            ToggleSettingCommand = new MvxAsyncCommand(async () => 
            {
                await Task.Run(() =>
                {
                    IsItemSetting = !IsItemSetting;
                });
            });
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }
        public IMvxAsyncCommand<int> ShowWindowChildCommand { get; private set; }

        public IMvxAsyncCommand ToggleSettingCommand { get; private set; }    
    }
}
