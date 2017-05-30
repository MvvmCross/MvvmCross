// IMvxViewModel.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading.Tasks;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxViewModel
    {
        void Appearing();

        void Appeared();

        void Disappearing();

        void Disappeared();

        void Init(IMvxBundle parameters);

        void ReloadState(IMvxBundle state);

        void Start();

        void Destroy ();

        void SaveState(IMvxBundle state);

        Task Initialize();
    }

    public interface IMvxViewModel<TParameter> : IMvxViewModel where TParameter : class
    {
        Task Initialize(TParameter parameter);
    }

    //TODO: Can we keep the IMvxViewModel syntax here? Compiler complains
    public interface IMvxViewModelResult<TResult> : IMvxViewModel where TResult : class
    {
        void SetClose(TaskCompletionSource<TResult> tcs);
        Task<bool> Close(TResult result);
    }

    public interface IMvxViewModel<TParameter, TResult> : IMvxViewModel<TParameter>, IMvxViewModelResult<TResult> where TParameter : class where TResult : class
    {
    }
}