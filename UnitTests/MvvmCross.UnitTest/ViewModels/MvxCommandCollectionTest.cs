// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Windows.Input;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.UnitTest.Mocks.Dispatchers;

namespace MvvmCross.UnitTest.ViewModels
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxCommandCollectionTest
    {
        private readonly NavigationTestFixture _fixture;

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

        [Test]
        public async Task Test_Conventional_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            await Assert.That(myCommand).IsNotNull();
            await CheckCounts(testObject);
            myCommand.Execute();
            await CheckCounts(testObject, 1, 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            await CheckCounts(testObject, 4, 4);
        }

        [Test]
        public async Task Test_Conventional_Command_CanExecute()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            await Assert.That(myCommand).IsNotNull();
            await CheckCounts(testObject);
            var result = myCommand.CanExecute();
            await CheckCounts(testObject, countCanExecuteMyCalled: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            await CheckCounts(testObject, countCanExecuteMyCalled: 4);
        }

        [Test]
        public async Task Test_Conventional_Parameter_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["MyEx"];
            await Assert.That(myCommand).IsNotNull();
            await CheckCounts(testObject);
            myCommand.Execute();
            await CheckCounts(testObject, countMyExCalled: 1, countCanExecuteMyExCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            await CheckCounts(testObject, countMyExCalled: 4, countCanExecuteMyExCalled: 4);
        }

        [Test]
        public async Task Test_Conventional_Parameter_Command_CanExecute()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["MyEx"];
            await Assert.That(myCommand).IsNotNull();
            await CheckCounts(testObject);
            var result = myCommand.CanExecute();
            await CheckCounts(testObject, countCanExecuteMyExCalled: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            await CheckCounts(testObject, countCanExecuteMyExCalled: 4);
        }

        [Test]
        public async Task Test_IntReturning_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["AnIntReturning"];
            await Assert.That(myCommand).IsNotNull();
            await CheckCounts(testObject);
            myCommand.Execute();
            await CheckCounts(testObject, countIntReturningCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            await CheckCounts(testObject, countIntReturningCalled: 4);
        }

        [Test]
        public async Task Test_Attribute1_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr"];
            await Assert.That(myCommand).IsNotNull();
            await CheckCounts(testObject);
            myCommand.Execute();
            await CheckCounts(testObject, countAttributedCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            await CheckCounts(testObject, countAttributedCalled: 4);
        }

        [Test]
        public async Task Test_Attribute2_Command()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr2"];
            await Assert.That(myCommand).IsNotNull();
            await CheckCounts(testObject);
            myCommand.Execute();
            await CheckCounts(testObject, countAttributed2Called: 1, countCanExecuteAttributed2Called: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            await CheckCounts(testObject, countAttributed2Called: 4, countCanExecuteAttributed2Called: 4);
        }

        [Test]
        public async Task Test_Attribute2_Command_CanExecute()
        {
            _fixture.ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr2"];
            await Assert.That(myCommand).IsNotNull();
            await CheckCounts(testObject);
            var result = myCommand.CanExecute();
            await CheckCounts(testObject, countCanExecuteAttributed2Called: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            await CheckCounts(testObject, countCanExecuteAttributed2Called: 4);
        }

        [Test]
        public async Task Test_PropertyChanged_Raises_CanExecuteChange()
        {
            _fixture.ClearAll();

            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            await Assert.That(myCommand).IsNotNull();
            var countMy = 0;
            myCommand.CanExecuteChanged += (sender, args) => countMy++;

            var myExCommand = collection["MyEx"];
            await Assert.That(myExCommand).IsNotNull();
            var countMyEx = 0;
            myExCommand.CanExecuteChanged += (sender, args) => countMyEx++;

            var calledByAttrCommand = collection["CalledByAttr"];
            await Assert.That(calledByAttrCommand).IsNotNull();
            var countAttr = 0;
            calledByAttrCommand.CanExecuteChanged += (sender, args) => countAttr++;

            var calledByAttr2Command = collection["CalledByAttr2"];
            await Assert.That(calledByAttr2Command).IsNotNull();
            var countAttr2 = 0;
            calledByAttr2Command.CanExecuteChanged += (sender, args) => countAttr2++;

            await CheckCounts(testObject);

            testObject.RaisePropertyChanged("CanExecuteAttributed2");
            await Assert.That(countMy).IsEqualTo(0);
            await Assert.That(countMyEx).IsEqualTo(0);
            await Assert.That(countAttr).IsEqualTo(0);
            await Assert.That(countAttr2).IsEqualTo(1);

            testObject.RaisePropertyChanged("CanExecuteAttributed2");
            await Assert.That(countMy).IsEqualTo(0);
            await Assert.That(countMyEx).IsEqualTo(0);
            await Assert.That(countAttr).IsEqualTo(0);
            await Assert.That(countAttr2).IsEqualTo(2);

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            await Assert.That(countMy).IsEqualTo(0);
            await Assert.That(countMyEx).IsEqualTo(0);
            await Assert.That(countAttr).IsEqualTo(0);
            await Assert.That(countAttr2).IsEqualTo(2);

            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            await Assert.That(countMy).IsEqualTo(3);
            await Assert.That(countMyEx).IsEqualTo(0);
            await Assert.That(countAttr).IsEqualTo(0);
            await Assert.That(countAttr2).IsEqualTo(2);

            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            await Assert.That(countMy).IsEqualTo(5);
            await Assert.That(countMyEx).IsEqualTo(6);
            await Assert.That(countAttr).IsEqualTo(0);
            await Assert.That(countAttr2).IsEqualTo(2);
        }

        [Test]
        public async Task Test_PropertyChanged_Raises_Multiple_CanExecuteChange()
        {
            _fixture.ClearAll();

            var dispatcher = new InlineMockMainThreadDispatcher();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var testObject = new SharedCommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var calledByAttrCommand = collection["CalledByAttr"];
            await Assert.That(calledByAttrCommand).IsNotNull();
            var countAttr = 0;
            calledByAttrCommand.CanExecuteChanged += (sender, args) => countAttr++;

            var calledByAttr2Command = collection["CalledByAttr2"];
            await Assert.That(calledByAttr2Command).IsNotNull();
            var countAttr2 = 0;
            calledByAttr2Command.CanExecuteChanged += (sender, args) => countAttr2++;

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            await Assert.That(countAttr).IsEqualTo(1);
            await Assert.That(countAttr2).IsEqualTo(1);

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            await Assert.That(countAttr).IsEqualTo(2);
            await Assert.That(countAttr2).IsEqualTo(2);
        }

        private async Task CheckCounts(
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
            await Assert.That(testObject.CountMyCommandCalled).IsEqualTo(countMyCalled);
            await Assert.That(testObject.CountCanExecuteMyCommandCalled).IsEqualTo(countCanExecuteMyCalled);
            await Assert.That(testObject.CountMyExCommandCalled).IsEqualTo(countMyExCalled);
            await Assert.That(testObject.CountCanExecuteMyExCommandCalled).IsEqualTo(countCanExecuteMyExCalled);
            await Assert.That(testObject.CountNotACmdCalled).IsEqualTo(countNotACalled);
            await Assert.That(testObject.CountAttributedCalled).IsEqualTo(countAttributedCalled);
            await Assert.That(testObject.CountAttributed2Called).IsEqualTo(countAttributed2Called);
            await Assert.That(testObject.CountCanExecuteAttributed2Called).IsEqualTo(countCanExecuteAttributed2Called);
            await Assert.That(testObject.CountAnIntReturningCalled).IsEqualTo(countIntReturningCalled);
        }
    }
}
