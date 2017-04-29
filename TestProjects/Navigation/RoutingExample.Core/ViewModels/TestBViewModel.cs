using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoutingExample.Core.ViewModels;

[assembly: MvxNavigation(typeof(TestBViewModel), @"mvx://test/\?id=(?<id>[A-Z0-9]{32})$")]

namespace RoutingExample.Core.ViewModels
{
    public class TestBViewModel
        : MvxViewModel
    {
        private string _id;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public void Init(string id)
        {
            _id = id;
        }
    }
}