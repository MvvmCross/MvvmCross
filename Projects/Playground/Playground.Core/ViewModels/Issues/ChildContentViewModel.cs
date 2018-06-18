using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ChildContentViewModel : MvxViewModel
    {
        public ChildContentViewModel()
        {
        }

        private string _test;

        public string Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }

        public override async Task Initialize()
        {
            //Test = "Bound Text";
            await Task.Yield();
        }
    }
}
