// SimpleRestTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Net;
using Cirrious.MvvmCross.Plugins.Network.Rest;
using Cirrious.MvvmCross.Plugins.Network.Test.TestClasses.GoogleBooks;
using Cirrious.MvvmCross.Test.Core;
using NUnit.Framework;

namespace MvvmCross.Plugins.Network.Test
{
    [TestFixture]
    public class SimpleRestTest
        : MvxIoCSupportingTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void GetDataFromGoogleBooks()
        {
            ClearAll();

            // not a real test yet....
            var url = BooksService.GetSearchUrl("MonoTouch");

            var json = new Cirrious.MvvmCross.Plugins.Json.MvxJsonConverter();
            var client = new Cirrious.MvvmCross.Plugins.Network.Rest.MvxJsonRestClient
                {
                    JsonConverterProvider = () => json
                };
            var request = new MvxRestRequest(url);
            MvxDecodedRestResponse<BookSearchResult> theResponse = null;
            Exception exception = null;
            client.MakeRequestFor<BookSearchResult>(request,
                                                    (result) => { theResponse = result; },
                                                    (error) => { exception = error; });

            System.Threading.Thread.Sleep(3000);
            Assert.IsNotNull(theResponse);
            Assert.IsNull(exception);
            Assert.IsNotNull(theResponse.Result);
            Assert.AreEqual(HttpStatusCode.OK, theResponse.StatusCode);
            Assert.IsTrue(theResponse.Result.items.Count == 10);
            Assert.IsTrue(theResponse.Result.items[0].ToString().Contains("MonoTouch"));
        }
    }
}