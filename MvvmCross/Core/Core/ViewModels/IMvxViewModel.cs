// IMvxViewModel.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxViewModel
    {
        MvxRequestedBy RequestedBy { get; set; }

		void Appearing();

		void Appeared();

		void Disappearing();

		void Disappeared();

        void Init(IMvxBundle parameters);

        void ReloadState(IMvxBundle state);

        void Start();

        void SaveState(IMvxBundle state);
    }
}