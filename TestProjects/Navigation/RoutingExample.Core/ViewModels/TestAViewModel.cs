using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoutingExample.Core.ViewModels;

[assembly: MvxNavigation(typeof(TestAViewModel), @"mvx://test/a")]
[assembly: MvxNavigation(typeof(TestAViewModel), @"https?://mvvmcross.com/blog")]
namespace RoutingExample.Core.ViewModels
{
    public class TestAViewModel
        : MvxViewModel
    {

        public TestAViewModel()
        {
            
        }

        public void Init()
        {
            
        }
    }
}
