using System;
using System.Collections.Generic;
using System.Net;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.Json;

namespace DroidAutoComplete.Books
{
    public class BooksJsonService
    {
        public void StartSearchAsync(string whatFor, Action<BookSearchResult> success, Action<Exception> error)
        {
            try
            {
                var webClient = new WebClient();
                webClient.DownloadStringCompleted += 
                    (s, e) =>
                        {
                            if (e.Error != null)
                            {
                                error(e.Error);
                                return;
                            }

                            BookSearchResult result;
                            try
                            {
						result = Mvx.Resolve<IMvxJsonConverter>().DeserializeObject<BookSearchResult>(e.Result);
                            }
                            catch (Exception exception)
                            {
                                error(exception);
                                return;
                                throw;
                            }

                            success(result);
                        };
                string address = string.Format("https://www.googleapis.com/books/v1/volumes?q={0}",
                                               Uri.EscapeDataString(whatFor)); 
                webClient.DownloadStringAsync(new Uri(address));
            }
            catch (Exception exception)
            {
                error(exception);
            }
        }

        #region Json Classes

        public class BookSearchItem : Java.Lang.Object
        {
            public string kind { get; set; }
            public string id { get; set; }
            public VolumeInfo volumeInfo { get; set; }

            public override string ToString()
            {
                return volumeInfo.title;
            }
        }

        public class VolumeInfo
        {
            public string title { get; set; }
            public List<string> authors { get; set; }
            public string authorSummary
            {
                get { return authors == null ? "-" : string.Join(", ", authors); }
            }
            public string publisher { get; set; }
            public string publishedDate { get; set; }
            public string descrption { get; set; }
            public int pageCount { get; set; }
            public double averageRating { get; set; }
            public int ratingsCount { get; set; }
            public ImageLinks imageLinks { get; set; }
            public string language { get; set; }
            public string previewLink { get; set; }
            public string infoLink { get; set; }
            public string canonicalVolumeLink { get; set; }
        }

        public class BookSearchResult
        {
            public List<BookSearchItem> items { get; set; }
        }

        public class ImageLinks
        {
            public string smallThumbnail { get; set; }
            public string thumbnail { get; set; }
        }

        #endregion
    }
}