using Xamarin.Forms;

namespace Example.Pages
{
    public class AboutPage 
        : ContentPage
    {
        public AboutPage()
        {
            Title = "About";

            Content = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "This is an Xamarin.Forms application demonstrating usage with " + "" +
                               "MvvmCross. The Application is made by Tomasz Cielecki and is " +
                               "based on a sample in a private repository, made by the creator " +
                               "of MvvmCross, Stuart Lodge.\n\nYou can fork this sample on " +
                               "GitHub"
                    }
                }
            };
        }
    }
}
