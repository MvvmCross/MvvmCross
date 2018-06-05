using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class CollectionViewModel : MvxViewModel
    {
        public IEnumerable<string> Codes { get; set; } = new List<string>
        {
            "test1",
            "test2",
            "test3"
        };

        public CollectionViewModel()
        {
        }
    }
}
