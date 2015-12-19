// MvxViewToViewModelNameMapping.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    public class MvxViewToViewModelNameMapping
        : IMvxNameMapping
    {
        public string ViewModelPostfix { get; set; }

        public MvxViewToViewModelNameMapping()
        {
            this.ViewModelPostfix = "ViewModel";
        }

        public virtual string Map(string inputName)
        {
            return inputName + this.ViewModelPostfix;
        }
    }
}