// MvxConsoleDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        public IMvxViewDispatcher Dispatcher
        {
            get { return new MvxConsoleDispatcher(); }
        }
    }
}