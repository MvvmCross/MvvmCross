using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Playground.Core.Models;
using Playground.Core.Services;
using Playground.Core.ViewModels;
using Playground.Core.ViewModels.Bindings;
using Playground.Core.ViewModels.Location;
using Playground.Core.ViewModels.Navigation;
using Playground.Core.ViewModels.Samples;

namespace Playground.Wpf.UI.ViewModels
{
    public class RootViewModel : Core.ViewModels.RootViewModel
    {
        public RootViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService, IMvxViewModelLoader mvxViewModelLoader)
               : base(logProvider, navigationService, mvxViewModelLoader)
        { }
    }
}
