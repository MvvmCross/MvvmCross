// IMvxBaseAndroidView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Droid.Interfaces
{
    public interface IMvxBaseAndroidView
        : IMvxView
    {
    }

    public interface IMvxViewModelInjectedAndroidView
        : IMvxBaseAndroidView
    {    
    }

    public interface IMvxIntentCreatedAndroidView
        : IMvxBaseAndroidView
    {        
    }
}
