<views:MvxWpfView
    x:Class="$rootnamespace$.Views.HomeView"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:views="clr-namespace:MvvmCross.Platforms.Wpf.Views;assembly=MvvmCross.Platforms.Wpf"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:local="clr-namespace:$rootnamespace$.Views"
    mc:Ignorable="d"
    d:DesignHeight="300" d:DesignWidth="300">
    <Grid>
        <StackPanel>
            <TextBox Text="{Binding Text, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
            <Button Content="Reset" Command="{Binding ResetTextCommand}" />
        </StackPanel>
    </Grid>
</views:MvxWpfView>