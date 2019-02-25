// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class WindowChildViewModel : MvxNavigationViewModel<WindowChildParam>
    {
        private WindowChildParam _param;

        public WindowChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }

        public int ParentNo => _param.ParentNo;
        public string Text => $"I'm No.{_param.ChildNo}. My parent is No.{_param.ParentNo}";

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public override void Prepare(WindowChildParam param) => _param = param;
    }
}
