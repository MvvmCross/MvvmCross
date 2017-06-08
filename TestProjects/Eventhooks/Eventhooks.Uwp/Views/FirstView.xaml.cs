using Eventhooks.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventhooks.Uwp.Views
{
    [MvxViewFor(typeof(FirstViewModel))]
    public sealed partial class FirstView : MvxWindowsPage
    {
        public FirstView()
        {
            InitializeComponent();
        }

        public FirstViewModel FirstViewModel
        {
            get => (FirstViewModel)ViewModel;
        }
    }
}
