using System;
using Android.App;
using BestSellers;
using MonoCross.Droid;
using MonoDroid.Dialog;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "Book View")]
    public class BookView : MXDialogActivityView<Book>
    {
        public override void Render()
        {
            Android.Util.Log.Debug("BookView: Render", "Description: " + Model.Description);

            this.Root = new RootElement("Number " + Model.Rank)
            {
                new Section("Number " + Model.Rank + " - " + Model.Title.ToTitleCase())
                {
                    new StringElement("Rank Last Week", Model.RankLastWeek),
                    new StringElement("Weeks on List", Model.WeeksOnList),
                    new StringElement("Price", String.Format("${0}", Model.Price)),
                    new StringElement("ISBN", Model.ISBN10),
                    new StringElement("Description", Model.Description, Resource.Layout.dialog_labelfieldbelow),
                }
            };
        }
    }
}