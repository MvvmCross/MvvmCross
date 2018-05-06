<views:MvxWindowsPage
    xmlns:views="using:MvvmCross.Platforms.Uap.Views"
    x:Class="$rootnamespace$.Views.HomeView"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="$rootnamespace$.Views"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d">

    <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
        <StackPanel>
            <TextBox Text="{Binding Text, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
            <Button Content="Reset" Command="{Binding ResetTextCommand}" />
        </StackPanel>
    </Grid>
</views:MvxWindowsPage>
