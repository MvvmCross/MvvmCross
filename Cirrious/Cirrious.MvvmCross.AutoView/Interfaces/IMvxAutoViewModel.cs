// IMvxAutoViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxAutoViewModel
    {
        bool SupportsAutoView(string type);

        KeyedDescription GetAutoView(string type);
    }
}