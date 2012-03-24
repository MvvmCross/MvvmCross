using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwitterSearch.Core.ViewModels;
using TwitterSearch.Test.Mocks;

namespace TwitterSearch.Test
{
    [TestClass]
    public class HomeViewModelTests
    {
        [TestMethod]
        public void SearchCommand_Calls_RequestNavigate()
        {
            using (var setup = new MockSetup())
            {
                setup.Initialize();
                var requests = new List<MvxShowViewModelRequest>();
                setup.Dispatcher
                    .Setup(x => x.RequestNavigate(It.IsAny<MvxShowViewModelRequest>()))
                    .Callback<MvxShowViewModelRequest>((request) => requests.Add(request));

                var viewModel = new HomeViewModel();
                viewModel.SearchText = "MyTestValue";
                viewModel.SearchCommand.Execute();

                setup.Dispatcher
                    .Verify(x => x.RequestNavigate(It.IsAny<MvxShowViewModelRequest>()), Times.Once());
                Assert.AreEqual(1, requests.Count);
                Assert.AreEqual(typeof(TwitterViewModel), requests[0].ViewModelType);
                Assert.AreEqual(false, requests[0].ClearTop);
                Assert.AreEqual(MvxRequestedBy.UserAction, requests[0].RequestedBy);
                Assert.AreEqual(1, requests[0].ParameterValues.Count);
                Assert.AreEqual("MyTestValue", requests[0].ParameterValues["searchTerm"]);
            }
        }

        [TestMethod]
        public void PickRandom_Changes_SearchText()
        {
            using (var setup = new MockSetup())
            {
                setup.Initialize();

                var viewModel = new HomeViewModel();
                Assert.IsNotNull(viewModel.SearchText);
                var calledProperties = new List<string>();
                viewModel.PropertyChanged += (sender, args) => calledProperties.Add(args.PropertyName);
                Assert.AreEqual(0, calledProperties.Count);
                
                var existingSearchText = viewModel.SearchText;
                viewModel.PickRandomCommand.Execute();
                Assert.AreEqual(1, calledProperties.Count);
                Assert.AreEqual("SearchText", calledProperties[0]);
                Assert.AreNotEqual(existingSearchText, viewModel.SearchText);

                existingSearchText = viewModel.SearchText;
                viewModel.PickRandomCommand.Execute();
                Assert.AreEqual(2, calledProperties.Count);
                Assert.AreEqual("SearchText", calledProperties[0]);
                Assert.AreEqual("SearchText", calledProperties[1]);
                Assert.AreNotEqual(existingSearchText, viewModel.SearchText);
            }
        }

        [TestMethod]
        public void User_Can_Change_SearchText()
        {
            using (var setup = new MockSetup())
            {
                setup.Initialize();

                var viewModel = new HomeViewModel();
                Assert.IsNotNull(viewModel.SearchText);
                viewModel.SearchText = "MyTestString";
                Assert.AreEqual("MyTestString", viewModel.SearchText);
            }
        }
    }
}