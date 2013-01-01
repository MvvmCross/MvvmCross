// MvxBaseConsoleContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Console.Views
{
    public abstract class MvxBaseConsoleContainer
        : MvxViewsContainer
          , IMvxConsoleNavigation
    {
        public abstract void Navigate(MvxShowViewModelRequest request);
        public abstract void GoBack();
        public abstract void RemoveBackEntry();
        public abstract bool CanGoBack();
    }
}