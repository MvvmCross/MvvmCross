using System.Collections.Generic;

namespace Cirrious.MvvmCross.Views
{
    public class MvxShowViewModelRequest
    {
        public MxvViewModelAction ViewModelAction { get; set; }
        public IDictionary<string, string> ParameterValues { get; set; }

        public MvxShowViewModelRequest()
        {            
        }

        public MvxShowViewModelRequest(MxvViewModelAction viewModelAction, IDictionary<string, string> parameterValues)
        {
            ViewModelAction = viewModelAction;
            ParameterValues = parameterValues;
        }
    }

    public class MvxShowViewModelRequest<TViewModel> : MvxShowViewModelRequest
    {
        public MvxShowViewModelRequest(MxvViewModelAction<TViewModel> viewModelAction, IDictionary<string, string> parameterValues)
            : base(viewModelAction, parameterValues)
        {
        }
    }
}