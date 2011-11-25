using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using MonoCross.Navigation;
using BestSellers;
using System.Globalization;
using System.Net;
using System.IO;

namespace BestSellers.Controllers
{
    class BookListController : MXController<BookList>
    {
        const string URL_BOOKLIST = "http://api.nytimes.com/svc/books/v2/lists/{0}.xml?api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

		public static string CapitalizeString(string stringValue)
		{
            return stringValue;
			//return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stringValue.ToLower());
		}

        public override string Load(Dictionary<string,string> parameters)
        {
            string category = parameters.ContainsKey("Category") ? parameters["Category"] : "BookList";

            Model = new BookList();
            Model.Category = category;

            string urlBooks = String.Format(URL_BOOKLIST, category);
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(urlBooks);
                var response = request.GetResponse();
                var stream = response.GetResponseStream();

                XElement booksDocument = XElement.Load(stream);

                var books = from item in booksDocument.Descendants("book") select new Book() {
                    CategoryEncoded = category,
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

                Model.AddRange(books);
                if (Model.Count > 0)
                    Model.CategoryDisplayName = Model[0].Category;
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return ViewPerspective.Read;
        }
    }
}
