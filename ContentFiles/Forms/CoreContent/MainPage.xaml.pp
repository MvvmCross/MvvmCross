<?xml version="1.0" encoding="UTF-8"?>
<d:MvxContentPage x:TypeArguments="viewModels:MainViewModel"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:$rootnamespace$.Core.Pages"
    x:Class="$rootnamespace$.Core.Pages.MainPage"
    xmlns:viewModels="clr-namespace:$rootnamespace$.Core.ViewModels;assembly=$rootnamespace$.Core"
    xmlns:d="clr-namespace:MvvmCross.Forms.Views;assembly=MvvmCross.Forms">
	<ContentPage.Content>
        <StackLayout>
        <Button Text="Show text" Command="{Binding ShowTextCommand}" />
        <Label Text="{Binding Text}" />
        </StackLayout>
	</ContentPage.Content>
</d:MvxContentPage>
