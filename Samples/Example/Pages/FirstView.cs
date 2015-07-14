using Xamarin.Forms;

namespace Example.Pages
{
    public class FirstPage
        : ContentPage
    {
        public FirstPage()
        {
            Title = "First Page";

            var entryBox = new Entry
            {
                Placeholder = "Who are you?",
                TextColor = Color.Aqua,
                WidthRequest = 30
            };

            var helloResponse = new Label
            {
                Text = string.Empty,
                FontSize = 24
        };

            Content = new StackLayout
            {
                Padding = new Thickness(10),
                Spacing = 10,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new Label
                    {
                        Text = "Enter your nickname in the box below",
                        FontSize = 24
                    },
                    entryBox,
                    helloResponse
                }
            };

            entryBox.SetBinding(Entry.TextProperty, new Binding("YourNickname"));
            helloResponse.SetBinding(Label.TextProperty, new Binding("Hello"));
        }
    }
}
