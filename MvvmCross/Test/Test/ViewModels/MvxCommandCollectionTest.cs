// MvxCommandCollectionTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Core;
using MvvmCross.Test.Mocks.Dispatchers;

namespace MvvmCross.Test.ViewModels
{
    using System.ComponentModel;
    using System.Windows.Input;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Test.Core;

    using NUnit.Framework;

    [TestFixture]
    public class MvxCommandCollectionTest : MvxIoCSupportingTest
    {
        public class CommandTestClass : INotifyPropertyChanged
        {
            public int CountMyCommandCalled { get; set; }

            public void MyCommand()
            {
                this.CountMyCommandCalled++;
            }

            public int CountCanExecuteMyCommandCalled { get; set; }

            public bool CanExecuteMyCommand
            {
                get
                {
                    this.CountCanExecuteMyCommandCalled++;
                    return true;
                }
            }

            public int CountMyExCommandCalled { get; set; }

            public void MyExCommand()
            {
                this.CountMyExCommandCalled++;
            }

            public int CountCanExecuteMyExCommandCalled { get; set; }

            public bool CanExecuteMyExCommand
            {
                get
                {
                    this.CountCanExecuteMyExCommandCalled++;
                    return true;
                }
            }

            public int CountNotACmdCalled { get; set; }

            public void NotACmd()
            {
                this.CountNotACmdCalled++;
            }

            public int CountAnIntReturningCalled { get; set; }

            public int AnIntReturningCommand()
            {
                this.CountAnIntReturningCalled++;
                return 99;
            }

            public int CountAttributedCalled { get; set; }

            [MvxCommand("CalledByAttr")]
            public void AttributedCommand(string ignored)
            {
                this.CountAttributedCalled++;
            }

            public int CountAttributed2Called { get; set; }

            [MvxCommand("CalledByAttr2", "CanExecuteAttributed2")]
            public void AttributedWithProperty()
            {
                this.CountAttributed2Called++;
            }

            public int CountCanExecuteAttributed2Called { get; set; }

            public bool CanExecuteAttributed2
            {
                get
                {
                    this.CountCanExecuteAttributed2Called++;
                    return true;
                }
            }

            public ICommand OldSchoolCommand { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public void RaisePropertyChanged(string propertyName)
            {
                var handler = this.PropertyChanged;
                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class SharedCommandTestClass : INotifyPropertyChanged
        {
            public int CountAttributedCalled { get; set; }

            [MvxCommand("CalledByAttr", "CanExecuteAttributed")]
            public void AttributedCommand(string ignored)
            {
                this.CountAttributedCalled++;
            }

            public int CountAttributed2Called { get; set; }

            [MvxCommand("CalledByAttr2", "CanExecuteAttributed")]
            public void AttributedWithProperty()
            {
                this.CountAttributed2Called++;
            }

            public int CountCanExecuteAttributedCalled { get; set; }

            public bool CanExecuteAttributed
            {
                get
                {
                    this.CountCanExecuteAttributedCalled++;
                    return true;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void RaisePropertyChanged(string propertyName)
            {
                var handler = this.PropertyChanged;
                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Test]
        public void Test_Conventional_Command()
        {
            ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            Assert.IsNotNull(myCommand);
            this.CheckCounts(testObject);
            myCommand.Execute();
            this.CheckCounts(testObject, countMyCalled: 1, countCanExecuteMyCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            this.CheckCounts(testObject, countMyCalled: 4, countCanExecuteMyCalled: 4);
        }

        [Test]
        public void Test_Conventional_Command_CanExecute()
        {
            ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            Assert.IsNotNull(myCommand);
            this.CheckCounts(testObject);
            var result = myCommand.CanExecute();
            this.CheckCounts(testObject, countCanExecuteMyCalled: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            this.CheckCounts(testObject, countCanExecuteMyCalled: 4);
        }

        [Test]
        public void Test_Conventional_Parameter_Command()
        {
            ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["MyEx"];
            Assert.IsNotNull(myCommand);
            this.CheckCounts(testObject);
            myCommand.Execute();
            this.CheckCounts(testObject, countMyExCalled: 1, countCanExecuteMyExCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            this.CheckCounts(testObject, countMyExCalled: 4, countCanExecuteMyExCalled: 4);
        }

        [Test]
        public void Test_Conventional_Parameter_Command_CanExecute()
        {
            ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["MyEx"];
            Assert.IsNotNull(myCommand);
            this.CheckCounts(testObject);
            var result = myCommand.CanExecute();
            this.CheckCounts(testObject, countCanExecuteMyExCalled: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            this.CheckCounts(testObject, countCanExecuteMyExCalled: 4);
        }

        [Test]
        public void Test_IntReturning_Command()
        {
            ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["AnIntReturning"];
            Assert.IsNotNull(myCommand);
            this.CheckCounts(testObject);
            myCommand.Execute();
            this.CheckCounts(testObject, countIntReturningCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            this.CheckCounts(testObject, countIntReturningCalled: 4);
        }

        [Test]
        public void Test_Attribute1_Command()
        {
            ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr"];
            Assert.IsNotNull(myCommand);
            this.CheckCounts(testObject);
            myCommand.Execute();
            this.CheckCounts(testObject, countAttributedCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            this.CheckCounts(testObject, countAttributedCalled: 4);
        }

        [Test]
        public void Test_Attribute2_Command()
        {
            ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr2"];
            Assert.IsNotNull(myCommand);
            this.CheckCounts(testObject);
            myCommand.Execute();
            this.CheckCounts(testObject, countAttributed2Called: 1, countCanExecuteAttributed2Called: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            this.CheckCounts(testObject, countAttributed2Called: 4, countCanExecuteAttributed2Called: 4);
        }

        [Test]
        public void Test_Attribute2_Command_CanExecute()
        {
            ClearAll();

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr2"];
            Assert.IsNotNull(myCommand);
            this.CheckCounts(testObject);
            var result = myCommand.CanExecute();
            this.CheckCounts(testObject, countCanExecuteAttributed2Called: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            this.CheckCounts(testObject, countCanExecuteAttributed2Called: 4);
        }

        [Test]
        public void Test_PropertyChanged_Raises_CanExecuteChange()
        {
            ClearAll();

            var dispatcher = new InlineMockMainThreadDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            Assert.IsNotNull(myCommand);
            var countMy = 0;
            myCommand.CanExecuteChanged += (sender, args) => countMy++;

            var myExCommand = collection["MyEx"];
            Assert.IsNotNull(myExCommand);
            var countMyEx = 0;
            myExCommand.CanExecuteChanged += (sender, args) => countMyEx++;

            var calledByAttrCommand = collection["CalledByAttr"];
            Assert.IsNotNull(calledByAttrCommand);
            var countAttr = 0;
            calledByAttrCommand.CanExecuteChanged += (sender, args) => countAttr++;

            var calledByAttr2Command = collection["CalledByAttr2"];
            Assert.IsNotNull(calledByAttr2Command);
            var countAttr2 = 0;
            calledByAttr2Command.CanExecuteChanged += (sender, args) => countAttr2++;

            this.CheckCounts(testObject);

            testObject.RaisePropertyChanged("CanExecuteAttributed2");
            Assert.AreEqual(0, countMy);
            Assert.AreEqual(0, countMyEx);
            Assert.AreEqual(0, countAttr);
            Assert.AreEqual(1, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteAttributed2");
            Assert.AreEqual(0, countMy);
            Assert.AreEqual(0, countMyEx);
            Assert.AreEqual(0, countAttr);
            Assert.AreEqual(2, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            Assert.AreEqual(0, countMy);
            Assert.AreEqual(0, countMyEx);
            Assert.AreEqual(0, countAttr);
            Assert.AreEqual(2, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            Assert.AreEqual(3, countMy);
            Assert.AreEqual(0, countMyEx);
            Assert.AreEqual(0, countAttr);
            Assert.AreEqual(2, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            testObject.RaisePropertyChanged("CanExecuteMyExCommand");
            Assert.AreEqual(5, countMy);
            Assert.AreEqual(6, countMyEx);
            Assert.AreEqual(0, countAttr);
            Assert.AreEqual(2, countAttr2);
        }

        [Test]
        public void Test_PropertyChanged_Raises_Multiple_CanExecuteChange()
        {
            ClearAll();

            var dispatcher = new InlineMockMainThreadDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            var testObject = new SharedCommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var calledByAttrCommand = collection["CalledByAttr"];
            Assert.IsNotNull(calledByAttrCommand);
            var countAttr = 0;
            calledByAttrCommand.CanExecuteChanged += (sender, args) => countAttr++;

            var calledByAttr2Command = collection["CalledByAttr2"];
            Assert.IsNotNull(calledByAttr2Command);
            var countAttr2 = 0;
            calledByAttr2Command.CanExecuteChanged += (sender, args) => countAttr2++;

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            Assert.AreEqual(1, countAttr);
            Assert.AreEqual(1, countAttr2);

            testObject.RaisePropertyChanged("CanExecuteAttributed");
            Assert.AreEqual(2, countAttr);
            Assert.AreEqual(2, countAttr2);
        }

        private void CheckCounts(
            CommandTestClass testObject
            , int countMyCalled = 0
            , int countCanExecuteMyCalled = 0
            , int countMyExCalled = 0
            , int countCanExecuteMyExCalled = 0
            , int countNotACalled = 0
            , int countAttributedCalled = 0
            , int countAttributed2Called = 0
            , int countCanExecuteAttributed2Called = 0
            , int countIntReturningCalled = 0)
        {
            Assert.AreEqual(countMyCalled, testObject.CountMyCommandCalled);
            Assert.AreEqual(countCanExecuteMyCalled, testObject.CountCanExecuteMyCommandCalled);
            Assert.AreEqual(countMyExCalled, testObject.CountMyExCommandCalled);
            Assert.AreEqual(countCanExecuteMyExCalled, testObject.CountCanExecuteMyExCommandCalled);
            Assert.AreEqual(countNotACalled, testObject.CountNotACmdCalled);
            Assert.AreEqual(countAttributedCalled, testObject.CountAttributedCalled);
            Assert.AreEqual(countAttributed2Called, testObject.CountAttributed2Called);
            Assert.AreEqual(countCanExecuteAttributed2Called, testObject.CountCanExecuteAttributed2Called);
            Assert.AreEqual(countIntReturningCalled, testObject.CountAnIntReturningCalled);
        }
    }
}