using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Phone7.Fx.Controls
{
    public class LoadingListBox:ListBox
    {
        public event RoutedEventHandler NeedItems;
        public event RoutedEventHandler OnScroll;

        private ScrollViewer _viewer;
        private ScrollBar _scrollBar;


        public static readonly DependencyProperty LoadingVisibilityProperty =
           DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(LoadingListBox),
           new PropertyMetadata(System.Windows.Visibility.Collapsed));

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public static readonly DependencyProperty IsOnDemandDataLoadedProperty = 
            DependencyProperty.Register(
                                "IsOnDemandDataLoaded",
                                typeof(bool),
                                typeof(LoadingListBox),
                                new PropertyMetadata(IsOnDemandDataLoadedPropertyChanged));


        private static void IsOnDemandDataLoadedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                ((LoadingListBox) d).LoadingVisibility = Visibility.Collapsed;
            }
            else
            {
                ((LoadingListBox)d).LoadingVisibility = Visibility.Visible;
            }
        }

        public bool IsOnDemandDataLoaded
        {
            get { return (bool)GetValue(IsOnDemandDataLoadedProperty); }
            set { SetValue(IsOnDemandDataLoadedProperty, value); }
        }

        public LoadingListBox()
        {
            this.Loaded += LoadingListBox_Loaded;
        }

        void LoadingListBox_Loaded(object sender, RoutedEventArgs e)
        {
             _viewer = this.GetVisualChildren().OfType<ScrollViewer>().FirstOrDefault();

            _scrollBar = _viewer.GetVisualDescendents().OfType<ScrollBar>().First(s => s.Name == "VerticalScrollBar");

            _scrollBar.ValueChanged += _scrollBar_ValueChanged;
           
        }

        void _scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (OnScroll != null)
                OnScroll(this, new RoutedEventArgs());
            if (_viewer.ScrollableHeight <= (_viewer.VerticalOffset + 20))
            {
                if (NeedItems != null && LoadingVisibility != Visibility.Visible)
                {
                    LoadingVisibility = Visibility.Visible;
                    //IsOnDemandDataLoaded = false;
                    
                    NeedItems(this, new RoutedEventArgs());
                }
            }
        }

    

      
    }
}