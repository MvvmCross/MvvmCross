// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net;
using System.Threading.Tasks;
using MvvmCross.Plugin.Json;
using MvvmCross.Plugin.Network.Rest;
using MvvmCross.Plugins.Network.UnitTest.TestClasses.GoogleBooks;
using Xunit;

namespace MvvmCross.Plugins.Network.UnitTest
{

    [Collection("Rest")]
    public class SimpleRestTest
    {
        [Fact]
        public async Task GetDataFromGoogleBooks()
        {
            // not a real test yet....
            var url = BooksService.GetSearchUrl("MonoTouch");

            var json = new MvxJsonConverter();
            var client = new MvxJsonRestClient
            {
                JsonConverterProvider = () => json
            };
            var request = new MvxRestRequest(url);
            MvxDecodedRestResponse<BookSearchResult> theResponse = null;
            Exception exception = null;
            theResponse = await client.MakeRequestForAsync<BookSearchResult>(request);

            Assert.NotNull(theResponse);
            Assert.Null(exception);
            Assert.NotNull(theResponse.Result);
            Assert.Equal(HttpStatusCode.OK, theResponse.StatusCode);
            Assert.True(theResponse.Result.items.Count == 10);
            Assert.Contains("MonoTouch", theResponse.Result.items[0].ToString());
        }
    }
}
