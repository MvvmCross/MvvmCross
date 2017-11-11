using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Test.Mocks.ViewModels
{
    public class SimpleParameterTestViewModel : MvxViewModel<string>
    {
        public string Parameter { get; set; }

        public virtual void Init()
        {
        }

        public override void Prepare(string parameter)
        {
            Parameter = parameter;
        }
    }
}
