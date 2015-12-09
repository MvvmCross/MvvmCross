// IMvxUserInterfaceFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Interfaces
{
    using CrossUI.Core.Descriptions;

    public interface IMvxUserInterfaceFactory
    {
        TResult Build<TBuildable, TResult>(IMvxAutoView view, KeyedDescription description);
    }
}