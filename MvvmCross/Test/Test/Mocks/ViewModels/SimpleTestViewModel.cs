using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Test.Mocks.ViewModels
{
    public class SimpleTestViewModel : MvxViewModel
    {
        public virtual void Init()
        {
        }

        public virtual void Init(string hello)
        {
        }
    }

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
