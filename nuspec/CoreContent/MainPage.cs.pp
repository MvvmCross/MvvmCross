using Xamarin.Forms;

namespace $rootnamespace$.Pages
{
    public class MainPage : ContentPage
    {
        public MainPage()
        { 
            var content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var entry = new Entry();
            entry.SetBinding(Entry.TextProperty, "Text");

            var button = new Button();
            button.Text = "Reset";
            button.SetBinding(Button.CommandProperty, "ResetTextCommand");

            content.Children.Add(entry);
            content.Children.Add(button);

            Content = content;
        }
    }
}
