using MvvmCross.ViewModels;
using Playground.Core.Models;

namespace Playground.Core.ViewModels.Navigation
{
    public class SecondTabForBehaviorViewModel : MvxViewModel, IMvxChangeTabAware<SampleModel>
    {
        private SampleModel _sampleModel;
        public string Message
        {
            get => _sampleModel.Message;
            set
            {
                _sampleModel.Message = value;
                RaisePropertyChanged();
            }
        }
        public decimal Value
        {
            get => _sampleModel.Value;
            set
            {
                _sampleModel.Value = value;
                RaisePropertyChanged();
            }
        }

        public SecondTabForBehaviorViewModel()
        {
            _sampleModel = new SampleModel();
        }

        public SampleModel OnNavigatedFrom() => new SampleModel
        {
            Message = "Changed Values",
            Value = -10
        };

        public void OnNavigatedTo(SampleModel parameter)
        {
            if (parameter != null)
            {
                _sampleModel = parameter;
                RaiseAllPropertiesChanged();
            }
        }
    }
}
