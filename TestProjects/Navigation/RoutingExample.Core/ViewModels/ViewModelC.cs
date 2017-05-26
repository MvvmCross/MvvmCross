using System;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class ViewModelC : MvxViewModel<Tuple<string, int>,string>
    {
        private string _navigatedTo;

        public string Title => "View C";

        public string NavigatedTo
        {
            get => _navigatedTo;
            set => SetProperty(ref _navigatedTo, value);
        }

        public MvxCommand CloseCommand => new MvxCommand(async () =>
        {
            await Close(Title);
        });

        public override Task Initialize(Tuple<string, int> parameter)
        {
            NavigatedTo = $"Navigated to from {parameter.Item1} {parameter.Item2} times";

            return Task.FromResult(true);
        }
    }
}
