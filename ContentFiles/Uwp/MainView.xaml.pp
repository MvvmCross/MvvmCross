<views:MvxWindowsPage
    x:Class="$rootnamespace$.Views.MainView"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:views="using:MvvmCross.Uwp.Views"
    mc:Ignorable="d">

    <Grid Background="{ThemeResource ApplicationPageBackgroundThemneBrush}">
        <StackPanel>
            <TextBox Text="{Binding Text, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
            <Button Content="Reset" Command="{x:Bind Vm.ResetTextCommand}" />
        </StackPanel>
    </Grid>
</views:MvxWindowsPage>
