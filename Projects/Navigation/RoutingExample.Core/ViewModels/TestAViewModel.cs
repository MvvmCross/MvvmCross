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
            async () => await Mvx.Resolve<IMvxNavigationService>().Navigate<TestBViewModel, User, User>(new User($"To B from {GetHashCode()}", "Something")));

        public void Init()
        {
        }

        public override void Prepare(User parameter)
        {
            var test = parameter;
        }
    }
}
