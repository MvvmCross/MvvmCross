// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Page1ViewModel : MvxNavigationViewModel
    {
        public MvxCommand<int> HeaderTappedCommand { get; }

        public Page1ViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            HeaderTappedCommand = new MvxCommand<int>(DoHeaderTappedCommand);

            var random = new Random();
            var sections = new List<SectionViewModel>();
            for (var section = 0; section < 5; section++)
            {
                var itemsCount = random.Next(0, 10);
                var items = new List<SectionItemViewModel>();
                for (var item = 0; item < itemsCount; item++)
                {
                    items.Add(new SectionItemViewModel
                    {
                        Title = $"Item {item}"
                    });
                }
                sections.Add(new SectionViewModel(items, section % 2 == 0)
                {
                    Title = $"Section {section}",
                    On = section % 2 != 0
                });
            }

            Sections = sections;
        }

        public List<SectionViewModel> Sections { get; }

        private void DoHeaderTappedCommand(int index)
        {
            System.Diagnostics.Debug.WriteLine($"Header {index} tapped");
        }

        public class SectionViewModel : MvxNotifyPropertyChanged, IEnumerable<SectionItemViewModel>
        {
            private List<SectionItemViewModel> _items;
            private string _title;
            private bool _on;

            public bool ShowsControl { get; }

            public string Title
            {
                get => _title;
                set => SetProperty(ref _title, value);
            }

            public bool On
            {
                get => _on;
                set => SetProperty(ref _on, value);
            }

            public SectionViewModel(IEnumerable<SectionItemViewModel> items, bool showsControl)
            {
                _items = new List<SectionItemViewModel>(items);
                ShowsControl = showsControl;
            }

            public IEnumerator<SectionItemViewModel> GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class SectionItemViewModel : MvxNotifyPropertyChanged
        {
            private string _title;

            public string Title
            {
                get => _title;
                set => SetProperty(ref _title, value);
            }
        }
    }
}
