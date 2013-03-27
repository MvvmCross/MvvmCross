// MvxCommandCollectionTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.ViewModels;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test.ViewModels
{
    [TestFixture]
    public class MvxCommandCollectionTest : MvxIoCSupportingTest
    {
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

            public bool CanExecuteAttributed2 { get
            {
                CountCanExecuteAttributed2Called++;
                return true;
            }}

            public ICommand OldSchoolCommand { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public void RaisePropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null) 
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Test]
        public void Test_Conventional_Command()
        {
            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            Assert.IsNotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, countMyCalled:1, countCanExecuteMyCalled:1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, countMyCalled: 4, countCanExecuteMyCalled: 4);
        }

        [Test]
        public void Test_Conventional_Command_CanExecute()
        {
            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["My"];
            Assert.IsNotNull(myCommand);
            CheckCounts(testObject);
            var result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteMyCalled: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteMyCalled: 4);
        }

        [Test]
        public void Test_Conventional_Parameter_Command()
        {
            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["MyEx"];
            Assert.IsNotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, countMyExCalled: 1, countCanExecuteMyExCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, countMyExCalled: 4, countCanExecuteMyExCalled: 4);
        }

        [Test]
        public void Test_Conventional_Parameter_Command_CanExecute()
        {
            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["MyEx"];
            Assert.IsNotNull(myCommand);
            CheckCounts(testObject);
            var result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteMyExCalled: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteMyExCalled: 4);
        }

        [Test]
        public void Test_Attribute1_Command()
        {
            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr"];
            Assert.IsNotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, countAttributedCalled: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, countAttributedCalled: 4);
        }

        [Test]
        public void Test_Attribute2_Command()
        {
            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr2"];
            Assert.IsNotNull(myCommand);
            CheckCounts(testObject);
            myCommand.Execute();
            CheckCounts(testObject, countAttributed2Called: 1, countCanExecuteAttributed2Called: 1);
            myCommand.Execute();
            myCommand.Execute();
            myCommand.Execute();
            CheckCounts(testObject, countAttributed2Called: 4, countCanExecuteAttributed2Called: 4);
        }

        [Test]
        public void Test_Attribute2_Command_CanExecute()
        {
            var testObject = new CommandTestClass();
            var collection = new MvxCommandCollectionBuilder()
                .BuildCollectionFor(testObject);

            var myCommand = collection["CalledByAttr2"];
            Assert.IsNotNull(myCommand);
            CheckCounts(testObject);
            var result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteAttributed2Called: 1);
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            result = myCommand.CanExecute();
            CheckCounts(testObject, countCanExecuteAttributed2Called: 4);
        }

        [Test]
        public void Test_PropertyChanged_Raises_CanExecuteChange()
        {
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

            CheckCounts(testObject);

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

        private void CheckCounts(
            CommandTestClass testObject
            , int countMyCalled = 0
            , int countCanExecuteMyCalled = 0
            , int countMyExCalled = 0
            , int countCanExecuteMyExCalled = 0
            , int countNotACalled = 0
            , int countAttributedCalled = 0
            , int countAttributed2Called = 0
            , int countCanExecuteAttributed2Called = 0)
        {
            Assert.AreEqual(countMyCalled, testObject.CountMyCommandCalled);
            Assert.AreEqual(countCanExecuteMyCalled, testObject.CountCanExecuteMyCommandCalled);
            Assert.AreEqual(countMyExCalled, testObject.CountMyExCommandCalled);
            Assert.AreEqual(countCanExecuteMyExCalled, testObject.CountCanExecuteMyExCommandCalled);
            Assert.AreEqual(countNotACalled, testObject.CountNotACmdCalled);
            Assert.AreEqual(countAttributedCalled, testObject.CountAttributedCalled);
            Assert.AreEqual(countAttributed2Called, testObject.CountAttributed2Called);
            Assert.AreEqual(countCanExecuteAttributed2Called, testObject.CountCanExecuteAttributed2Called);            
        }
    }
}