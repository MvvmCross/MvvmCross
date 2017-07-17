using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;

namespace MvvmCross.Test.Mocks.Dispatchers
{
    public class NavigationMockDispatcher 
        : MvxMainThreadDispatcher, IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();
        public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint>();

        public virtual bool RequestMainThreadAction(Action action, 
                                                    bool maskExceptions = true)
        {
            action();
            return true;
        }

        public virtual bool ShowViewModel(MvxViewModelRequest request)
        {
            var debugString = $"ShowViewModel: '{request.ViewModelType.Name}' ";
            if (request.ParameterValues != null)
                debugString += $"with parameters: {string.Join(",", request.ParameterValues.Select(pair => $"{{ {pair.Key}={pair.Value} }}"))}";
            else
                debugString += "without parameters";
            Debug.WriteLine(debugString);

            Requests.Add(request);
            return true;
        }

        public virtual bool ChangePresentation(MvxPresentationHint hint)
        {
            Hints.Add(hint);
            return true;
        }

    }
}
