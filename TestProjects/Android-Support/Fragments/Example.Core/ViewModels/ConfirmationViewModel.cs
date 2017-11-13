using System;
using System.Threading.Tasks;
using Example.Core.Model;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Example.Core.ViewModels
{
    public class ConfirmationViewModel : MvxViewModel<ConfirmationConfiguration, bool?>
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public ConfirmationViewModel(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;

            PropertyChanged += ConfirmationViewModel_PropertyChanged;
        }

        public override void Prepare(ConfirmationConfiguration parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            Title = parameter.Title;
            Body = parameter.Body;
            PositiveCommandText = parameter.PositiveCommandText;
            NegativeCommandText = parameter.NegativeCommandText;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await Task.Delay(3000);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _body;
        public string Body
        {
            get => _body;
            set => SetProperty(ref _body, value);
        }

        private string _positiveCommandText;
        public string PositiveCommandText
        {
            get => _positiveCommandText;
            set => SetProperty(ref _positiveCommandText, value);
        }

        private string _negativeCommandText;
        public string NegativeCommandText
        {
            get => _negativeCommandText;
            set => SetProperty(ref _negativeCommandText, value);
        }

        private IMvxAsyncCommand _yesCommand;
        public IMvxAsyncCommand PositiveCommand => _yesCommand ?? (_yesCommand = new MvxAsyncCommand(OnPositiveCommandAsync));

        private Task OnPositiveCommandAsync()
        {
            return _mvxNavigationService.Close(this, true);
        }

        private IMvxAsyncCommand _noCommand;
        public IMvxAsyncCommand NegativeCommand => _noCommand ?? (_noCommand = new MvxAsyncCommand(OnNegativeCommandAsync));

        private Task OnNegativeCommandAsync()
        {
            return _mvxNavigationService.Close(this, false);
        }

        private void ConfirmationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(InitializeTask) && InitializeTask != null)
            {
                InitializeTask.PropertyChanged += InitializeTask_PropertyChanged;
            }
        }

        private void InitializeTask_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(InitializeTask.IsSuccessfullyCompleted))
                Mvx.Trace($"ConfirmationViewModel: Initialize task has finished successfully: {InitializeTask.IsSuccessfullyCompleted}!");
        }
    }
}
