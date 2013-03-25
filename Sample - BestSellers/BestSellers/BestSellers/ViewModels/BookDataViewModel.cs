using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers.ViewModels
{
    public class BookDataViewModel
        : BaseViewModel
    {
        public string Category { get; set; }
        public string CategoryEncoded { get; set; }
        public string Title { get; set; }
        public string Contributor { get; set; }
        public string Author { get; set; }
        public string Rank { get; set; }
        public string BestSellersDate { get; set; }
        public string PublishedDate { get; set; }
        public string WeeksOnList { get; set; }
        public string RankLastWeek { get; set; }
        public string Description { get; set; }
        public string ContributorNote { get; set; }
        public string Price { get; set; }
        public string AgeGroup { get; set; }
        public string Publisher { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public string BookReviewLink { get; set; }
        public string FirstChapterLink { get; set; }
        public string SundayReviewLink { get; set; }
        public string ArticleChapterLink { get; set; }

        public string ISBN
        {
            get
            {
                long isbn;
                if (Int64.TryParse(ISBN10, out isbn))
                    return ISBN10;
                else
                    return ISBN13;
            }
        }

        public string AmazonImageUrl
        {
            get { return string.Format("http://images.amazon.com/images/P/{0}.01.ZTZZZZZZ.jpg", ISBN10); }
        }

        public override string ToString()
        {
            return Title + "(" + ISBN + ")";
        }

        public ICommand ViewDetailCommand
        {
            get { return new MvxCommand(() => ShowViewModel<BookViewModel>(new { category= CategoryEncoded, book=ISBN })); }
        }
    }
}