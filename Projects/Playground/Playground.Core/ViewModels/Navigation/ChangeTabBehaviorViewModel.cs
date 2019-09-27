using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.Models;

namespace Playground.Core.ViewModels.Navigation
{
    public class ChangeTabBehaviorViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private bool _firstTime = true;

        public ChangeTabBehaviorViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>
            {
                _navigationService.Navigate<FirstTabForBehaviorViewModel>(),
                _navigationService.Navigate<SecondTabForBehaviorViewModel>()
            };
            return Task.WhenAll(tasks);
        }

        public override void ViewAppearing()
        {
            if (_firstTime)
            {
                ShowInitialViewModels();
                _firstTime = false;
            }
        }
    }

    public class FirstTabForBehaviorViewModel : MvxViewModel, IMvxChangeTabAware<SampleModel>
    {
        private SampleModel _sampleModel;
        public SampleModel SampleModel
        {
            get => _sampleModel;
            set => SetProperty(ref _sampleModel, value);
        }

        public FirstTabForBehaviorViewModel()
        {
            SampleModel = new SampleModel();
        }

        public SampleModel OnNavigatedFrom() => SampleModel;

        public void OnNavigatedTo(SampleModel parameter)
        {
            if (parameter != null)
                SampleModel = parameter;
        }
    }

    public class SecondTabForBehaviorViewModel : MvxViewModel, IMvxChangeTabAware<SampleModel>
    {
        private SampleModel _sampleModel;
        public SampleModel SampleModel
        {
            get => _sampleModel;
            set => SetProperty(ref _sampleModel, value);
        }

        public SecondTabForBehaviorViewModel()
        {
            SampleModel = new SampleModel();
        }

        public SampleModel OnNavigatedFrom() => new SampleModel
        {
            Message = "Changed Values",
            Value = -10
        };

        public void OnNavigatedTo(SampleModel parameter)
        {
            if (parameter != null)
                SampleModel = parameter;
        }
    }
}
