using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using BestSellers.Helpers;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers.ViewModels
{
    public class BookListViewModel : BaseViewModel
    {
        const string URL_BOOKLIST = "http://api.nytimes.com/svc/books/v2/lists/{0}.xml?api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

        public void Init(string category = null)
        {
            Category = category ?? "BookList";

            AsyncLoad(Category);
        }

        private string _category;
        public string Category
        {
            get { return _category; }
            set { _category = value; RaisePropertyChanged("CategoryData"); }
        }

        private string _categoryDisplayName;
        public string CategoryDisplayName
        {
            get { return _categoryDisplayName; }
            set { _categoryDisplayName = value; RaisePropertyChanged("CategoryDisplayName"); }
        }

        private List<BookDataViewModel> _list;
        public List<BookDataViewModel> List
        {
            get { return _list; }
            set { _list = value; RaisePropertyChanged("List"); }
        }

        private void AsyncLoad(string category)
        {
            string urlBooks = String.Format(URL_BOOKLIST, category);
            GeneralAsyncLoad(urlBooks, ProcessResponse);
        }

        private void ProcessResponse(Stream stream)
        {
            XElement booksDocument = XElement.Load(stream);

            var books = from item in booksDocument.Descendants("book")
                        select new BookDataViewModel()
                        {
                            CategoryEncoded = Category,
                            Category = item.Element("display_name").Value,
                            BestSellersDate = item.Element("bestsellers_date").Value,
                            Rank = item.Element("rank").Value,
                            RankLastWeek = item.Element("rank_last_week").Value,
                            WeeksOnList = item.Element("weeks_on_list").Value,
                            PublishedDate = item.Element("published_date").Value,

                            Title = item.Descendants("book_detail").Elements("title").First().Value.ToTitleCase(),
                            Contributor = item.Descendants("book_detail").Elements("contributor").First().Value,
                            ContributorNote = item.Descendants("book_detail").Elements("contributor_note").First().Value,
                            Author = item.Descendants("book_detail").Elements("author").First().Value,
                            ISBN13 = item.Descendants("book_detail").Elements("primary_isbn13").First().Value,
                            ISBN10 = item.Descendants("book_detail").Elements("primary_isbn10").First().Value,
                            Publisher = item.Descendants("book_detail").Elements("publisher").First().Value,
                            AgeGroup = item.Descendants("book_detail").Elements("age_group").First().Value,
                            Price = item.Descendants("book_detail").Elements("price").First().Value,
                            Description = item.Descendants("book_detail").Elements("description").First().Value,

                            BookReviewLink = item.Descendants("review").Elements("book_review_link").First().Value,
                            FirstChapterLink = item.Descendants("review").Elements("first_chapter_link").First().Value,
                            SundayReviewLink = item.Descendants("review").Elements("sunday_review_link").First().Value,
                            ArticleChapterLink = item.Descendants("review").Elements("article_chapter_link").First().Value,
                        };

            List = books.ToList();
            if (List.Count > 0)
                CategoryDisplayName = List[0].Category;
        }
    }
}
