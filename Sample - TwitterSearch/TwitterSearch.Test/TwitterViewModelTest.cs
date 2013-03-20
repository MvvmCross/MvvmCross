using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TwitterSearch.Core.Interfaces;
using TwitterSearch.Core.Models;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.Test
{
    [TestFixture]
    public class TwitterViewModelTest : MvxTest
    {
        [Test]
        public void ConstructionStartsSearch()
        {
            ClearAll();
            CreateMockNavigation();

            var twitterService = new Mock<ITwitterSearchProvider>();
            var searchText = "To search for";
            var viewModel = new TwitterViewModel(twitterService.Object);
            viewModel.Init(searchText);
            Assert.IsTrue(viewModel.IsSearching);
            twitterService.Verify(x => x.StartAsyncSearch(It.Is<string>(s => s == searchText), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()), Times.Once());
        }

        [Test]
        public void ErrorredSearchCausesChangeInIsSearching()
        {
            ClearAll();            
            CreateMockNavigation();

            var twitterService = new Mock<ITwitterSearchProvider>();
            var searchText = "To search for";
            Action<IEnumerable<Tweet>> storedSuccessAction = null;
            Action<Exception> storedErrorAction = null;
            twitterService.Setup(x => x.StartAsyncSearch(It.Is<string>(s => s == searchText), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()))
                          .Callback((string s, Action<IEnumerable<Tweet>> suc, Action<Exception> err) =>
                              {
                                  storedSuccessAction = suc;
                                  storedErrorAction = err;
                              });
            var viewModel = new TwitterViewModel(twitterService.Object);
            viewModel.Init(searchText);
            var exception = new Exception("Just for fun");
            storedErrorAction(exception);
            Assert.IsFalse(viewModel.IsSearching);
        }

        [Test]
        public void SuccessfulSearchCausesChangeInIsSearching()
        {
            ClearAll();
            CreateMockNavigation();

            var twitterService = new Mock<ITwitterSearchProvider>();
            var searchText = "To search for";
            Action<IEnumerable<Tweet>> storedSuccessAction = null;
            Action<Exception> storedErrorAction = null;
            twitterService.Setup(x => x.StartAsyncSearch(It.Is<string>(s => s == searchText), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()))
                          .Callback((string s, Action<IEnumerable<Tweet>> suc, Action<Exception> err) =>
                          {
                              storedSuccessAction = suc;
                              storedErrorAction = err;
                          });
            var viewModel = new TwitterViewModel(twitterService.Object);
            viewModel.Init(searchText);
            var list = new List<Tweet>()
                {
                    new Tweet() {}
                };
            storedSuccessAction(list);
            Assert.IsFalse(viewModel.IsSearching);
        }

        [Test]
        public void SuccessfulSearchCausesListToBeBroadcast()
        {
            ClearAll();
            CreateMockNavigation();
            
            var twitterService = new Mock<ITwitterSearchProvider>();
            var searchText = "To search for";
            Action<IEnumerable<Tweet>> storedSuccessAction = null;
            Action<Exception> storedErrorAction = null;
            twitterService.Setup(x => x.StartAsyncSearch(It.Is<string>(s => s == searchText), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()))
                          .Callback((string s, Action<IEnumerable<Tweet>> suc, Action<Exception> err) =>
                          {
                              storedSuccessAction = suc;
                              storedErrorAction = err;
                          });
            var viewModel = new TwitterViewModel(twitterService.Object);
            viewModel.Init(searchText);
            var list = new List<Tweet>()
                {
                    new Tweet() {}
                };
            storedSuccessAction(list);
            Assert.IsNotNull(viewModel.Tweets);
            Assert.AreEqual(1, viewModel.Tweets.Count());
        }
    }
}