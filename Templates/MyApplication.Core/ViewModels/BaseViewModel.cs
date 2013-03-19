using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.Core.ViewModels
{
    public class BaseViewModel 
        : MvxViewModel
    {
        public void ReportError(string error)
        {
            Mvx.Resolve<IErrorReporter>().ReportError(error);
        }
    }
}