using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwitterSearch.Core.Models;
using TwitterSearch.Core.ViewModels;
using TwitterSearch.Test.Mocks;

namespace TwitterSearch.Test
{
    [TestClass]
    public class TwitterViewModelTests
    {
        [TestMethod]
        public void TwitterSearch_Constructor_Requests_A_TwitterSearch()
        {
            using (var setup = new MockSetup())
            {
                setup.Initialize();

                string search = null;
                Action<IEnumerable<Tweet>> success = null;
                Action<Exception> error = null;
                setup
                    .Twitter
                    .Setup(x => x.StartAsyncSearch(It.IsAny<string>(), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()))
                    .Callback<string,Action<IEnumerable<Tweet>>,Action<Exception>>((s,s2,e) =>
                                                                                       {
                                                                                           search = s;
                                                                                           success = s2;
                                                                                           error = e;
                                                                                       });
                var viewModel = new TwitterViewModel("mySearchTerm");
                setup
                    .Twitter
                    .Verify(x => x.StartAsyncSearch(It.IsAny<string>(), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()), Times.Once());
                Assert.AreEqual("mySearchTerm",search);
                Assert.IsNotNull(success);
                Assert.IsNotNull(error);
            }
        }

        [TestMethod]
        public void TwitterSearch_Constructor_Sets_IsSearching_True()
        {
            using (var setup = new MockSetup())
            {
                setup.Initialize();

                var viewModel = new TwitterViewModel("mySearchTerm");
                Assert.IsTrue(viewModel.IsSearching);
            }
        }

        [TestMethod]
        public void TwitterSearch_Success_Callback_Clears_IsSearching()
        {
            using (var setup = new MockSetup())
            {
                setup.Initialize();

                string search = null;
                Action<IEnumerable<Tweet>> success = null;
                Action<Exception> error = null;
                setup
                    .Twitter
                    .Setup(x => x.StartAsyncSearch(It.IsAny<string>(), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()))
                    .Callback<string, Action<IEnumerable<Tweet>>, Action<Exception>>((s, s2, e) =>
                                                                                         {
                                                                                             search = s;
                                                                                             success = s2;
                                                                                             error = e;
                                                                                         });
                var viewModel = new TwitterViewModel("mySearchTerm");

                var calledChanges = new List<string>();
                viewModel.PropertyChanged += (s, a) => calledChanges.Add(a.PropertyName);


                var result = new [] { new Tweet(), new Tweet() };
                success(result);
                Assert.AreEqual(2, calledChanges.Count);
                CollectionAssert.Contains(calledChanges, "IsSearching");
                Assert.IsFalse(viewModel.IsSearching);
            }
        }

        [TestMethod]
        public void TwitterSearch_Success_Callback_Sets_Tweets()
        {
            using (var setup = new MockSetup())
            {
                setup.Initialize();

                string search = null;
                Action<IEnumerable<Tweet>> success = null;
                Action<Exception> error = null;
                setup
                    .Twitter
                    .Setup(x => x.StartAsyncSearch(It.IsAny<string>(), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()))
                    .Callback<string, Action<IEnumerable<Tweet>>, Action<Exception>>((s, s2, e) =>
                                                                                         {
                                                                                             search = s;
                                                                                             success = s2;
                                                                                             error = e;
                                                                                         });
                var viewModel = new TwitterViewModel("mySearchTerm");

                var calledChanges = new List<string>();
                viewModel.PropertyChanged += (s, a) => calledChanges.Add(a.PropertyName);


                var result = new[] { new Tweet(), new Tweet() };
                success(result);
                Assert.AreEqual(2, calledChanges.Count);
                CollectionAssert.Contains(calledChanges, "Tweets");

                Assert.AreEqual(2, viewModel.Tweets.Count());
                CollectionAssert.Contains(viewModel.Tweets.ToList(), result[0]);
                CollectionAssert.Contains(viewModel.Tweets.ToList(), result[1]);
            }
        }

        [TestMethod]
        public void TwitterSearch_Error_Callback_Clears_IsSearching()
        {
            using (var setup = new MockSetup())
            {
                setup.Initialize();

                string search = null;
                Action<IEnumerable<Tweet>> success = null;
                Action<Exception> error = null;
                setup
                    .Twitter
                    .Setup(x => x.StartAsyncSearch(It.IsAny<string>(), It.IsAny<Action<IEnumerable<Tweet>>>(), It.IsAny<Action<Exception>>()))
                    .Callback<string, Action<IEnumerable<Tweet>>, Action<Exception>>((s, s2, e) =>
                                                                                         {
                                                                                             search = s;
                                                                                             success = s2;
                                                                                             error = e;
                                                                                         });
                var viewModel = new TwitterViewModel("mySearchTerm");

                var calledChanges = new List<string>();
                viewModel.PropertyChanged += (s, a) => calledChanges.Add(a.PropertyName);

                error(new Exception("Test exception"));
                Assert.AreEqual(1, calledChanges.Count);
                CollectionAssert.Contains(calledChanges, "IsSearching");
                Assert.IsFalse(viewModel.IsSearching);
                Assert.IsNull(viewModel.Tweets);

            }
        }
    }
}