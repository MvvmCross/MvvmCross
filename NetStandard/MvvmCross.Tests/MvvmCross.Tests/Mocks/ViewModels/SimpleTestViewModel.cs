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
}
