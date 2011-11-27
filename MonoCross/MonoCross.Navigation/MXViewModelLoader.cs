using System;
using System.Collections.Generic;
using MonoCross.Navigation.ActionResults;

namespace MonoCross.Navigation
{
#warning Why not just make TViewModel dependent on IMXViewModel everywhere?
    public interface IMXViewModelLocator<TViewModel>
    {
        bool TryLoad(Dictionary<string, string> parameters, out TViewModel model);
    }


    public abstract class MXViewModelLocator<TViewModel> : IMXViewModelLocator<TViewModel>
    {
        public virtual bool TryLoad(Dictionary<string, string> parameters, out TViewModel model)
        {
            model = default(TViewModel);
            return false;
        }
    }
}
