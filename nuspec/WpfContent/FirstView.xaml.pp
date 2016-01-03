<views:MvxWpfView 
             x:Class="$rootnamespace$.Views.FirstView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:views="clr-namespace:MvvmCross.Wpf.Views;assembly=MvvmCross.Wpf"
             mc:Ignorable="d" 
             d:DesignHeight="300" d:DesignWidth="300">
    <Grid>
        <StackPanel>
            <TextBox Text="{Binding Hello, Mode=TwoWay}" />
            <TextBox Text="{Binding Hello, Mode=TwoWay}" />
        </StackPanel>
    </Grid>
</views:MvxWpfView>
