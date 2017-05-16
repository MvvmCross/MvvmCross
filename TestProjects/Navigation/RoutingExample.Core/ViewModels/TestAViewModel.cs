using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoutingExample.Core.ViewModels;

[assembly: MvxNavigation(typeof(TestAViewModel), @"mvx://test/a")]
[assembly: MvxNavigation(typeof(TestAViewModel), @"https?://mvvmcross.com/blog")]
namespace RoutingExample.Core.ViewModels
{
    public class TestAViewModel
        : MvxViewModel<User>
    {

        public TestAViewModel()
        {
            
        }

        public IMvxAsyncCommand OpenViewModelBCommand => new MvxAsyncCommand(
            async () => await Mvx.Resolve<IMvxNavigationService>().Navigate<TestBViewModel, User, User>(new User($"To B from {this.GetHashCode()}", "Something")));

        public void Init()
        {
            
        }

        public override async Task Initialize(User parameter)
        {
            var test = parameter;
            await Task.FromResult(true);
        }
    }
}
