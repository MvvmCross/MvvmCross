using Xamarin.Forms;

namespace $rootnamespace$.Pages
{
    public class FirstPage : ContentPage
    {
        public FirstPage()
        { 
            var content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var label = new Label();
            label.SetBinding(Label.TextProperty, "Hello");

            var entry = new Entry();
            entry.SetBinding(Entry.TextProperty, "Hello");

            content.Children.Add(label);
            content.Children.Add(entry);

            Content = content;
        }
    }
}
