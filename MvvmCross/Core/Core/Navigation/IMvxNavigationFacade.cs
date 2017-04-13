using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Navigation
{
    public interface IMvxNavigationFacade
    {
        Task<MvxViewModelRequest> BuildViewModelRequest(string url, IDictionary<string, string> currentParameters);
    }
}
