using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterDetailExample.Core.ViewModels
{
    public class RootContentViewModel : MvxViewModel
    {
        string _message;
        public string Message { get { return _message; } set { SetProperty(ref _message, value); } }

        public RootContentViewModel()
        {
            Message = "Página inicio principal";
        }
    }
}
