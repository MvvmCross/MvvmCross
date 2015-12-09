// IMvxViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxViewModel
    {
        MvxRequestedBy RequestedBy { get; set; }

        void Init(IMvxBundle parameters);

        void ReloadState(IMvxBundle state);

        void Start();

        void SaveState(IMvxBundle state);
    }
}