// IMvxAndroidAutoView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces
{
    public interface IMvxAndroidAutoView
        : IMvxAndroidView
          , IMvxAutoView
    {
    }
}