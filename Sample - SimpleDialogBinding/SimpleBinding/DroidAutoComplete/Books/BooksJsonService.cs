using System;
using System.Net;
using Newtonsoft.Json;

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
                                result = JsonConvert.DeserializeObject<BookSearchResult>(e.Result);
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
    }
}