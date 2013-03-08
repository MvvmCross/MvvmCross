using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using BestSellers.Helpers;

namespace BestSellers.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private BookDataViewModel _book;
        public BookDataViewModel Book
        {
            get { return _book; }
            set
            {
                _book = value;
                RaisePropertyChanged("Book");
            }
        }

        public void Init(string category = null, string book = null)
        {
            category = category ?? string.Empty;
            book = book ?? string.Empty;

            AsyncFindBook(category, book);
        }

        private void AsyncFindBook(string category, string bookId)
        {
            string urlBooks =
                "http://api.nytimes.com/svc/books/v2/lists.xml?list={0}&isbn={1}&api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

            urlBooks = String.Format(urlBooks, category.Replace(" ", "-"), bookId);
            GeneralAsyncLoad(urlBooks, ProcessResponse);
        }

        private void ProcessResponse(Stream stream)
        {
            XDocument loaded = null;
            loaded = XDocument.Load(stream);
            var books = from item in loaded.Descendants("book")
                        select new BookDataViewModel()
                                    {
                                        Category = item.Element("display_name").Value,
                                        BestSellersDate = item.Element("bestsellers_date").Value,
                                        Rank = item.Element("rank").Value,
                                        RankLastWeek = item.Element("rank_last_week").Value,
                                        WeeksOnList = item.Element("weeks_on_list").Value,
                                        PublishedDate = item.Element("published_date").Value,

                                        Title =
                                            item.Descendants("book_detail").Elements("title").First().Value.
                                            ToTitleCase(),
                                        Contributor =
                                            item.Descendants("book_detail").Elements("contributor").First().Value,
                                        ContributorNote =
                                            item.Descendants("book_detail").Elements("contributor_note").First().
                                            Value,
                                        Author = item.Descendants("book_detail").Elements("author").First().Value,
                                        ISBN13 =
                                            item.Descendants("book_detail").Elements("primary_isbn13").First().
                                            Value,
                                        ISBN10 =
                                            item.Descendants("book_detail").Elements("primary_isbn10").First().
                                            Value,
                                        Publisher =
                                            item.Descendants("book_detail").Elements("publisher").First().Value,
                                        AgeGroup =
                                            item.Descendants("book_detail").Elements("age_group").First().Value,
                                        Price = item.Descendants("book_detail").Elements("price").First().Value,
                                        Description =
                                            item.Descendants("book_detail").Elements("description").First().Value,

                                        BookReviewLink =
                                            item.Descendants("review").Elements("book_review_link").First().Value,
                                        FirstChapterLink =
                                            item.Descendants("review").Elements("first_chapter_link").First().
                                            Value,
                                        SundayReviewLink =
                                            item.Descendants("review").Elements("sunday_review_link").First().
                                            Value,
                                        ArticleChapterLink =
                                            item.Descendants("review").Elements("article_chapter_link").First().
                                            Value,
                                    };

            Book = books.First();
        }
    }
}