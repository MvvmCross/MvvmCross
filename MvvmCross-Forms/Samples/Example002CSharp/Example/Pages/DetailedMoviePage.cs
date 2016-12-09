using Example.Converters;
using Xamarin.Forms;

namespace Example.Pages
{
    public class DetailedMoviePage
        : ContentPage
    {
        public DetailedMoviePage()
        {
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            var innerStack = new StackLayout
            {
                Spacing = 10,
                Orientation = StackOrientation.Vertical
            };

            var scrollView = new ScrollView {Content = innerStack};

            var poster = new Image
            {
                VerticalOptions = LayoutOptions.Fill,
                HeightRequest = 200
            };

            var title = new Label
            {
                Font = Font.SystemFontOfSize(NamedSize.Large),
                HorizontalOptions = LayoutOptions.Center
            };

            var tagLine = new Label
            {
                HorizontalOptions = LayoutOptions.Center
            };

            var runtimeLabel = new Label
            {
                Text = "Runtime: ",
                HorizontalOptions = LayoutOptions.Start
            };

            var runtime = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            var runtimeStack = new StackLayout
            {
                Children =
                {
                    runtimeLabel,
                    runtime
                }
            };

            var scoreLabel = new Label
            {
                Text = "Score: ",
                HorizontalOptions = LayoutOptions.Start
            };

            var score = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            var scoreStack = new StackLayout
            {
                Children =
                {
                    scoreLabel,
                    score
                }
            };

            var synopsis = new Label
            {
                HorizontalOptions = LayoutOptions.Center
            };

            var showOnImdb = new Button
            {
                Text = "Show on IMDB",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            innerStack.Children.Add(poster);
            innerStack.Children.Add(title);
            innerStack.Children.Add(tagLine);
            innerStack.Children.Add(runtimeStack);
            innerStack.Children.Add(scoreStack);
            innerStack.Children.Add(synopsis);
            innerStack.Children.Add(showOnImdb);

            poster.SetBinding(Image.SourceProperty, new Binding("PosterUrl"));
            title.SetBinding(Label.TextProperty, new Binding("Title"));
            score.SetBinding(Label.TextProperty, new Binding("Score", stringFormat: "{0} / 10"));
            runtime.SetBinding(Label.TextProperty, new Binding("Runtime", BindingMode.Default, new MinutesToHoursMinutesValueConverter()));
            tagLine.SetBinding(Label.TextProperty, new Binding("TagLine"));
            synopsis.SetBinding(Label.TextProperty, new Binding("Synopsis"));
            showOnImdb.SetBinding(Button.CommandProperty, new Binding("ShowOnImdbCommand"));
            SetBinding(TitleProperty, new Binding("Title"));

            Content = scrollView;
        }
    }
}
