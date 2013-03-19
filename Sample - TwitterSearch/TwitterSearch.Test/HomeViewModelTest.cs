using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;
using TwitterSearch.Core.ViewModels;
using TwitterSearch.Test.Mocks;

namespace TwitterSearch.Test
{
    [TestFixture]
    public class HomeViewModelTest : MvxTest
    {
        [Test]
        public void GoCausesNoNavigationForBannedWord()
        {
            var mockNavigation = CreateMockNavigation();
            var viewModel = new HomeViewModel();
            var searchTerm = "javascript";
            viewModel.SearchText = searchTerm;
            viewModel.SearchCommand.Execute(null);
            Assert.AreEqual(0, mockNavigation.NavigateRequests.Count);
        }

        [Test]
        public void GoCausesNoNavigationForEmptySearch()
        {
            var mockNavigation = CreateMockNavigation();
            var viewModel = new HomeViewModel();
            var searchTerm = "";
            viewModel.SearchText = searchTerm;
            viewModel.SearchCommand.Execute(null);
            Assert.AreEqual(0, mockNavigation.NavigateRequests.Count);
        }

        [Test]
        public void GoCausesNavigationForNonEmptySearch()
        {
            var mockNavigation = new MockMvxViewDispatcher();
            var mockNavigationProvider = new MockMvxViewDispatcherProvider();
            mockNavigationProvider.ViewDispatcher = mockNavigation;
            Ioc.RegisterSingleton<IMvxViewDispatcherProvider>(mockNavigationProvider);

            var viewModel = new HomeViewModel();
            var searchTerm = "Test Search Term";
            viewModel.SearchText = searchTerm;
            viewModel.SearchCommand.Execute(null);
            Assert.AreEqual(1, mockNavigation.NavigateRequests.Count);
        }

        [Test]
        public void RandomChangesTheSearchTerm()
        {
            var viewModel = new HomeViewModel();
            var searchTerm = "Test Search Term";
            viewModel.SearchText = searchTerm;
            viewModel.PickRandomCommand.Execute(null);
            Assert.AreNotEqual(searchTerm, viewModel.SearchText);
        }
    }
}
