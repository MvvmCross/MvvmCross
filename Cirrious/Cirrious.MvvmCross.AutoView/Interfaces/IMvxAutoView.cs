// IMvxAutoView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxAutoView
        : IMvxView
          , IMvxConsumer
    {
    }
}