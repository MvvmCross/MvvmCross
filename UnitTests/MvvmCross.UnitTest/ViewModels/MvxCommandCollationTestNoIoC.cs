// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Commands;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxCommandCollationTestNoIoC
    {
        private NavigationTestFixture _fixture;

        public MvxCommandCollationTestNoIoC(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            // ensure singletons are cleared, no IoC please :)
            fixture.Reset();
        }

        [Fact]
        public void CanConstructCommand()
        {
            var command = new MvxCommand(() => {/* nothing to see here */});

            Assert.NotNull(command);
        }

        [Fact]
        public void CanExecuteCommand()
        {
            var i = 0;
            var command = new MvxCommand(() => { ++i; });

            Assert.NotNull(command);

            command.Execute();

            Assert.Equal(1, i);
        }

        [Fact]
        public void CanExecuteChanged()
        {
            var canExecute = false;

            var command = new MvxCommand(() => { }, () => canExecute);

            Assert.False(command.CanExecute());

            var i = 0;
            command.CanExecuteChanged += (s, e) =>
            {
                i++;
            };

            canExecute = true;
            command.RaiseCanExecuteChanged();

            Assert.True(command.CanExecute());
            Assert.Equal(1, i);
        }
    }
}
