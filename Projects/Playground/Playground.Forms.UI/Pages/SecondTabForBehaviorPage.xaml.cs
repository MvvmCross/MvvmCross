using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Playground.Core.ViewModels.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation]
    public partial class SecondTabForBehaviorPage : MvxContentPage<SecondTabForBehaviorViewModel>
    {
        public SecondTabForBehaviorPage()
        {
            InitializeComponent();
        }
    }
}
