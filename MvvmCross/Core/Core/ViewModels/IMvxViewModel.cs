// IMvxViewModel.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading;
using System.Threading.Tasks;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxViewModel
    {
        void ViewCreated();

        void ViewAppearing();

        void ViewAppeared();

        void ViewDisappearing();

        void ViewDisappeared();

        void ViewDestroy();

        void Init(IMvxBundle parameters);

        void ReloadState(IMvxBundle state);

        void Start();

        void SaveState(IMvxBundle state);

        void Prepare();

        Task Initialize();
    }

    public interface IMvxViewModel<TParameter> : IMvxViewModel
    {
        void Prepare(TParameter parameter);
    }

    //TODO: Can we keep the IMvxViewModel syntax here? Compiler complains
    public interface IMvxViewModelResult<TResult> : IMvxViewModel
    {
        TaskCompletionSource<object> CloseCompletionSource { get; set; }
    }

    public interface IMvxViewModel<TParameter, TResult> : IMvxViewModel<TParameter>, IMvxViewModelResult<TResult>
    {
    }
}