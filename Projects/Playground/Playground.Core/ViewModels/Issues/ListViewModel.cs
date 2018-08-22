// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ListViewModel : MvxViewModel
    {
        public MvxObservableCollection<TestItem> TestItems { get; } = new MvxObservableCollection<TestItem>();
        public IMvxAsyncCommand<TestItem> ItemClickedCommand => new MvxAsyncCommand<TestItem>(ItemClicked);

        public ListViewModel()
        {
            TestItems.Add(new TestItem()
            {
                Title = "Item1"
            });

            TestItems.Add(new TestItem()
            {
                Title = "Item2"
            });
        }

        private async Task ItemClicked(TestItem arg)
        {
            var item = arg;
        }
    }

    public class TestItem
    {
        public string Title { get; set; }
    }
}
