namespace $rootnamespace$.Pages

open Xamarin.Forms

type MainPage() as this =
    inherit ContentPage()

    do
        let content = new StackLayout(HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center)

        let label = new Label()
        label.SetBinding(Label.TextProperty, "Hello")

        let entry = new Entry()
        entry.SetBinding(Entry.TextProperty, "Hello")

        content.Children.Add(label)
        content.Children.Add(entry)

        this.Content = content