using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.ViewModels;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.Core.ViewModels
{
    public class BaseViewModel 
        : MvxViewModel
    {
        public void ReportError(string error)
        {
            this.Resolve<IErrorReporter>().ReportError(error);
        }
    }
}