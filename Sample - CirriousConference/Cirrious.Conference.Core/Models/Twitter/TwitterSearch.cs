using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using Cirrious.CrossCore.Core;

namespace Cirrious.Conference.Core.Models.Twitter
{
    public class TwitterSearch
    {
        public static void StartAsyncSearch(string searchText, Action<IEnumerable<Tweet>> success, Action<Exception> error)
        {
            MvxAsyncDispatcher.BeginAsync(() => DoAsyncSearch(searchText, success, error));
        }

        private static void DoAsyncSearch(string searchText, Action<IEnumerable<Tweet>> success, Action<Exception> error)
        {
            var search = new TwitterSearch(searchText, success, error);
            search.StartSearch();
        }

        private const string TwitterUrl = "http://search.twitter.com/search.atom?rpp=50&&q=";

        private readonly string _searchText;
        private readonly Action<IEnumerable<Tweet>> _success;
        private readonly Action<Exception> _error;

        private TwitterSearch(string searchText, Action<IEnumerable<Tweet>> success, Action<Exception> error)
        {
            _searchText = searchText;
            _success = success;
            _error = error;
        }

        private void StartSearch()
        {
            try
            {
                // perform the search
                string uri = TwitterUrl + _searchText;
                var request = WebRequest.Create(new Uri(uri));
                request.BeginGetResponse(ReadCallback, request);
            }
            catch (Exception exception)
            {
                _error(exception);
            }
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var request = (HttpWebRequest)asynchronousResult.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                using (var streamReader1 = new StreamReader(response.GetResponseStream()))
                {
                    string resultString = streamReader1.ReadToEnd();
                    HandleResponse(resultString);
                }
            }
            catch (Exception exception)
            {
                _error(exception);
            }
        }

        private void HandleResponse(string xml)
        {
            var doc = XDocument.Parse(xml);
            var items = doc.Descendants(AtomConst.Entry)
                .Select(entryElement => new Tweet()
                                            {
                                                Title = entryElement.Descendants(AtomConst.Title).Single().Value,
                                                Id = long.Parse(entryElement.Descendants(AtomConst.ID).Single().Value.Split(':')[2]),
                                                ProfileImageUrl = entryElement.Descendants(AtomConst.Link).Skip(1).First().Attribute("href").Value,
                                                Timestamp = DateTime.Parse(entryElement.Descendants(AtomConst.Published).Single().Value),
                                                Author = entryElement.Descendants(AtomConst.Name).Single().Value
                                            });
            _success(items);
        }

    }
}