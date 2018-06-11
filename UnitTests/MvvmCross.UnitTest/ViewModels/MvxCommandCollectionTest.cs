// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Windows.Input;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxCommandCollectionTest
    {
        private NavigationTestFixture _fixture;

        public MvxCommandCollectionTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }

        public class CommandTestClass : INotifyPropertyChanged
        {
            public int CountMyCommandCalled { get; set; }

            public void MyCommand()
            {
                CountMyCommandCalled++;
            }

            public int CountCanExecuteMyCommandCalled { get; set; }

            public bool CanExecuteMyCommand
            {
                get
                {
                    CountCanExecuteMyCommandCalled++;
                    return true;
                }
            }

            public int CountMyExCommandCalled { get; set; }

            public void MyExCommand()
            {
                CountMyExCommandCalled++;
            }

            public int CountCanExecuteMyExCommandCalled { get; set; }

            public bool CanExecuteMyExCommand
            {
                get
                {
                    CountCanExecuteMyExCommandCalled++;
                    return true;
                }
            }

            public int CountNotACmdCalled { get; set; }

            public void NotACmd()
            {
                CountNotACmdCalled++;
            }

            public int CountAnIntReturningCalled { get; set; }

            public int AnIntReturningCommand()
            {
                CountAnIntReturningCalled++;
                return 99;
            }

            public int CountAttributedCalled { get; set; }

            [MvxCommand("CalledByAttr")]
            public void AttributedCommand(string ignored)
            {
                CountAttributedCalled++;
            }

            public int CountAttributed2Called { get; set; }

            [MvxCommand("CalledByAttr2", "CanExecuteAttributed2")]
            public void AttributedWithProperty()
            {
                CountAttributed2Called++;
            }

            public int CountCanExecuteAttributed2Called { get; set; }

            public bool CanExecuteAttributed2
            {
                get
                {
                    CountCanExecuteAttributed2Called++;
                    return true;
                }
            }

            public ICommand OldSchoolCommand { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public void RaisePropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class SharedCommandTestClass : INotifyPropertyChanged
        {
            public int CountAttributedCalled { get; set; }

            [MvxCommand("CalledByAttr", "CanExecuteAttributed")]
            public void AttributedCommand(string ignored)
            {
                CountAttributedCalled++;
            }

            public int CountAttributed2Called { get; set; }

            [MvxCommand("CalledByAttr2", "CanExecuteAttributed")]
            public void AttributedWithProperty()
            {
                CountAttributed2Called++;
            }

            public int CountCanExecuteAttributedCalled { get; set; }

            public bool CanExecuteAttributed
            {
                get
                {
                    CountCanExecuteAttributedCalled++;
                    return true;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void RaisePropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Fact]
        public void Test_Conventional_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            Assert.NotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, 1, 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, 4, 4);
        }

        [Fact]
        public void Test_Conventional_Command_CanExecute()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            Assert.NotNull(myCommand);
            CheckCounts(testObject);
            var result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteMyCalled: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteMyCalled: 4);
        }

        [Fact]
        public void Test_Conventional_Parameter_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["MyEx"];
            Assert.NotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, countMyExCalled: 1, countCanExecuteMyExCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, countMyExCalled: 4, countCanExecuteMyExCalled: 4);
        }

        [Fact]
        public void Test_Conventional_Parameter_Command_CanExecute()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["MyEx"];
            Assert.NotNull(myCommand);
            CheckCounts(testObject);
            var result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteMyExCalled: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteMyExCalled: 4);
        }

        [Fact]
        public void Test_IntReturning_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["AnIntReturning"];
            Assert.NotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, countIntReturningCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, countIntReturningCalled: 4);
        }

        [Fact]
        public void Test_Attribute1_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr"];
            Assert.NotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, countAttributedCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, countAttributedCalled: 4);
        }

        [Fact]
        public void Test_Attribute2_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr2"];
            Assert.NotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, countAttributed2Called: 1, countCanExecuteAttributed2Called: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, countAttributed2Called: 4, countCanExecuteAttributed2Called: 4);
        }

        [Fact]
        public void Test_Attribute2_Command_CanExecute()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr2"];
            Assert.NotNull(myCommand);
            CheckCounts(testObject);
            var result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteAttributed2Called: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteAttributed2Called: 4);
        }

        [Fact]
        public void Test_PropertyChanged_Raises_CanExecuteChange()
        {
            _fixture.ClearAll();

            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            Assert.NotNull(myCommand);
            var countMy = 0;
            myCommand.CanExecuteChanged += (sender, args) => countMy++;

            var myExCommand = collection["MyEx"];
            Assert.NotNull(myExCommand);
            var countMyEx = 0;
            myExCommand.CanExecuteChanged += (sender, args) => countMyEx++;

            var calledByAttrCommand = collection["CalledByAttr"];
            Assert.NotNull(calledByAttrCommand);
            var countAttr = 0;
            calledByAttrCommand.CanExecuteChanged += (sender, args) => countAttr++;

            var calledByAttr2Command = collection["CalledByAttr2"];
            Assert.NotNull(calledByAttr2Command);
            var countAttr2 = 0;
            calledByAttr2Command.CanExecuteChanged += (sender, args) => countAttr2++;

            CheckCounts(testObject);

            testObject.RaisePropertyChanged("CanExecuteAttributed2");
            Assert.Equal(0, countMy);
            Assert.Equal(0, countMyEx);
            Assert.Equal(0, countAttr);
            Assert.Equal(1, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteAttributed2");
            Assert.Equal(0, countMy);
            Assert.Equal(0, countMyEx);
            Assert.Equal(0, countAttr);
            Assert.Equal(2, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            Assert.Equal(0, countMy);
            Assert.Equal(0, countMyEx);
            Assert.Equal(0, countAttr);
            Assert.Equal(2, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            Assert.Equal(3, countMy);
            Assert.Equal(0, countMyEx);
            Assert.Equal(0, countAttr);
            Assert.Equal(2, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            Assert.Equal(5, countMy);
            Assert.Equal(6, countMyEx);
            Assert.Equal(0, countAttr);
            Assert.Equal(2, countAttr2);
        }

        [Fact]
        public void Test_PropertyChanged_Raises_Multiple_CanExecuteChange()
        {
            _fixture.ClearAll();

            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var testObject = new SharedCommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var calledByAttrCommand = collection["CalledByAttr"];
            Assert.NotNull(calledByAttrCommand);
            var countAttr = 0;
            calledByAttrCommand.CanExecuteChanged += (sender, args) => countAttr++;

            var calledByAttr2Command = collection["CalledByAttr2"];
            Assert.NotNull(calledByAttr2Command);
            var countAttr2 = 0;
            calledByAttr2Command.CanExecuteChanged += (sender, args) => countAttr2++;

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            Assert.Equal(1, countAttr);
            Assert.Equal(1, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            Assert.Equal(2, countAttr);
            Assert.Equal(2, countAttr2);
        }

        private void CheckCounts(
            CommandTestClass testObject, 
            int countMyCalled = 0, 
            int countCanExecuteMyCalled = 0, 
            int countMyExCalled = 0, 
            int countCanExecuteMyExCalled = 0, 
            int countNotACalled = 0, 
            int countAttributedCalled = 0, 
            int countAttributed2Called = 0, 
            int countCanExecuteAttributed2Called = 0, 
            int countIntReturningCalled = 0)
        {
            Assert.Equal(countMyCalled, testObject.CountMyCommandCalled);
            Assert.Equal(countCanExecuteMyCalled, testObject.CountCanExecuteMyCommandCalled);
            Assert.Equal(countMyExCalled, testObject.CountMyExCommandCalled);
            Assert.Equal(countCanExecuteMyExCalled, testObject.CountCanExecuteMyExCommandCalled);
            Assert.Equal(countNotACalled, testObject.CountNotACmdCalled);
            Assert.Equal(countAttributedCalled, testObject.CountAttributedCalled);
            Assert.Equal(countAttributed2Called, testObject.CountAttributed2Called);
            Assert.Equal(countCanExecuteAttributed2Called, testObject.CountCanExecuteAttributed2Called);
            Assert.Equal(countIntReturningCalled, testObject.CountAnIntReturningCalled);
        }
    }
}
