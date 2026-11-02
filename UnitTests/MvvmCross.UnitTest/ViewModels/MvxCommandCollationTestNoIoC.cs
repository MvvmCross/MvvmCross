// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Commands;

namespace MvvmCross.UnitTest.ViewModels
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxCommandCollationTestNoIoC
    {
        private readonly NavigationTestFixture _fixture;

        public MvxCommandCollationTestNoIoC(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            // ensure singletons are cleared, no IoC please :)
            fixture.Reset();
        }

        [Test]
        public async Task CanConstructCommand()
        {
            var command = new MvxCommand(() => {/* nothing to see here */});

            await Assert.That(command).IsNotNull();
        }

        [Test]
        public async Task CanExecuteCommand()
        {
            var i = 0;
            var command = new MvxCommand(() => { ++i; });

            await Assert.That(command).IsNotNull();

            command.Execute();

            await Assert.That(i).IsEqualTo(1);
        }

        [Test]
        public async Task CanExecuteChanged()
        {
            var canExecute = false;

            var command = new MvxCommand(() => { }, () => canExecute);

            await Assert.That(command.CanExecute()).IsFalse();

            var i = 0;
            command.CanExecuteChanged += (s, e) =>
            {
                i++;
            };

            canExecute = true;
            command.RaiseCanExecuteChanged();

            await Assert.That(command.CanExecute()).IsTrue();
            await Assert.That(i).IsEqualTo(1);
        }
    }
}
