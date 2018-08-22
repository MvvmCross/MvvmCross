<?xml version="1.0" encoding="utf-8" ?>
<views:MvxContentPage
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:views="clr-namespace:MvvmCross.Forms.Views;assembly=MvvmCross.Forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:$rootnamespace$.Views"
    x:Class="$rootnamespace$.Views.HomeView">
  
    <Grid>
      <StackLayout>
        <Entry Text="{Binding Text, Mode=TwoWay}"></Entry>
        <Button Text="Reset" Command="{Binding ResetTextCommand}"></Button>
      </StackLayout>
    </Grid>
</views:MvxContentPage>
