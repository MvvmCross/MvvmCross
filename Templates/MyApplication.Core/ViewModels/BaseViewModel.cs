using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.ViewModels;
using MyApplication.Core.Interfaces.Errors;

namespace MyApplication.Core.ViewModels
{
    public class BaseViewModel 
        : MvxViewModel
    {
        public void ReportError(string error)
        {
            this.GetService<IErrorReporter>().ReportError(error);
        }
    }
}