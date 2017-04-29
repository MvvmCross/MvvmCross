// IMvxViewModelInitializer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading.Tasks;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxViewModelInitializer<TInit> : IMvxViewModel
    {
        Task Init(string parameter);
    }
}