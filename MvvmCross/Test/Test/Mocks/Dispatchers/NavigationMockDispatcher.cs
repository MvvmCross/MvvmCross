using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;

namespace MvvmCross.Test.Mocks.Dispatchers
{
    public class NavigationMockDispatcher : MvxMainThreadDispatcher
      , IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();
        public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint>();

        public virtual bool RequestMainThreadAction(Action action)
        {
            action();
            return true;
        }

        public virtual bool ShowViewModel(MvxViewModelRequest request)
        {
            var sb = new StringBuilder();

            if (request.ParameterValues != null)
            {
                foreach (var pair in request.ParameterValues)
                {
                    sb.Append(string.Format("{{ {0}={1} }},", pair.Key, pair.Value));
                }
            }

            if (sb.Length == 0)
            {
                Debug.WriteLine("ShowViewModel: '{0}' without parameters", request.ViewModelType.Name, string.Empty);
            }
            else
            {
                Debug.WriteLine("ShowViewModel: '{0}' with parameters: {1}", request.ViewModelType.Name, sb);
            }
            
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
