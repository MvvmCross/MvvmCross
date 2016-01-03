<views:MvxStorePage
    x:Class="$rootnamespace$.Views.FirstView"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:views="using:MvvmCross.WindowsStore.Views"
    mc:Ignorable="d">

    <Grid Background="{StaticResource ApplicationPageBackgroundThemeBrush}">
        <StackPanel>
            <TextBox Text="{Binding Hello, Mode=TwoWay}" />
            <TextBlock Text="{Binding Hello}" />
        </StackPanel>
    </Grid>
</views:MvxStorePage>
