using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace Phone7.Fx.Controls.JumpList
{
    public partial class JumpListBox : UserControl
    {
        public event EventHandler SelectedItemChanged;

        public static readonly DependencyProperty IndexedMemberProperty =
           DependencyProperty.Register("IndexedMember", typeof(string), typeof(JumpListBox), null);

        public static readonly DependencyProperty ImageMemberProperty =
           DependencyProperty.Register("ImageMember", typeof(string), typeof(JumpListBox), null);

        public static readonly DependencyProperty IsPumpedProperty =
            DependencyProperty.Register("DoPump", typeof(bool), typeof(JumpListBox), new PropertyMetadata(true));



        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(JumpListBox), new PropertyMetadata(OnItemsSourceChanged));


        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(JumpListBox), new PropertyMetadata(OnSelectedItemChanged));



        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(JumpListBox), new PropertyMetadata(OnItemTemplateChanged));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public bool IsPumped
        {
            get { return (bool)GetValue(IsPumpedProperty); }
            set { SetValue(IsPumpedProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Name of the property to be used as index
        /// </summary>
        public string IndexedMember
        {
            get { return (string)GetValue(IndexedMemberProperty); }
            set { SetValue(IndexedMemberProperty, value); }
        }


        public string ImageMember
        {
            get { return (string)GetValue(ImageMemberProperty); }
            set { SetValue(ImageMemberProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
                if (SelectedItemChanged != null)
                {
                    SelectedItemChanged(this, EventArgs.Empty);
                }
            }
        }


        public ListBox InternalListBox
        {
            get
            {
                return mainListBox;
            }
        }

        private readonly JumpListSelector _jumpListSelector;

        private static List<AlphabetizedItemContainer> _orderedCollection;

        public JumpListBox()
        {
            InitializeComponent();

            _jumpListSelector = new JumpListSelector();
            _jumpListSelector.LetterClicked += new EventHandler<GEventArgs<object>>(_jumpListSelector_LetterClicked);

            this.Loaded += JumpListBox_Loaded;

            TiltEffect.SetIsTiltEnabled(this, true);
        }

        void _jumpListSelector_LetterClicked(object sender, GEventArgs<object> e)
        {

            Dispatcher.BeginInvoke(() =>
                                       {
                                           ScrollViewer scrollViewer = VisualTreeHelper.GetChild(mainListBox, 0) as ScrollViewer;
                                           if (scrollViewer != null)
                                               scrollViewer.ScrollToVerticalOffset(_orderedCollection.IndexOf(e.Data as AlphabetizedItemContainer));
                                       });
        }

        void JumpListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (_jumpListSelector.Parent != null)
                return;

            FrameworkElement root = Parent as FrameworkElement;
            Grid lastGrid = null;

            while (root.Parent != null)
            {
                Grid tempGrid = root as Grid;

                if (tempGrid != null)
                    lastGrid = tempGrid;

                if (root.Parent is PhoneApplicationPage)
                {
                    break;
                }
                root = root.Parent as FrameworkElement;
            }

            if (lastGrid.RowDefinitions.Count > 0)
                Grid.SetRowSpan(_jumpListSelector, lastGrid.RowDefinitions.Count);
            if (lastGrid.ColumnDefinitions.Count > 0)
                Grid.SetColumnSpan(_jumpListSelector, lastGrid.ColumnDefinitions.Count);
            lastGrid.Children.Add(_jumpListSelector);
        }


        private static void OnItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            JumpListBox jumpListBox = sender as JumpListBox;
            if (jumpListBox == null)
                throw new ArgumentException("The sender must be a JumpListBox");

            IEnumerable items = e.NewValue as IEnumerable;

            var collection = new AlphabetizedCollection(jumpListBox.IndexedMember, jumpListBox.ImageMember);
            collection.AddRange(items);

            _orderedCollection = collection.OrderBy(c => c.FirstLetter).ToList();

            jumpListBox.mainListBox.DataContext = null;
            jumpListBox._jumpListSelector.ItemsSource = collection;

            if (jumpListBox.IsPumped)
            {
                PumpList<AlphabetizedItemContainer> pumpList =
                    new PumpList<AlphabetizedItemContainer>(_orderedCollection);
                jumpListBox.mainListBox.ItemsSource = pumpList.Items;

                pumpList.StartPump();
            }
            else
                jumpListBox.mainListBox.ItemsSource = _orderedCollection;

        }



        private static void OnItemTemplateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            JumpListBox jumpListBox = sender as JumpListBox;
            if (jumpListBox == null)
                throw new ArgumentException("The sender must be a JumpListBox");
            DataTemplate template = e.NewValue as DataTemplate;
            if (template == null)
                throw new ArgumentException("The template must be a DataTemplate");

            jumpListBox.mainListBox.ItemTemplate = template;
        }



        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            JumpListBox jumpListBox = sender as JumpListBox;
            if (jumpListBox == null)
                throw new ArgumentException("The sender must be a JumpListBox");

            if (e.NewValue == null)
                jumpListBox.mainListBox.SelectedItem = null;
        }
        private void mainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainListBox.SelectedItem == null)
                return;

            AlphabetizedItemContainer container = mainListBox.SelectedItem as AlphabetizedItemContainer;

            if (container == null)
                return;
            if (container.ShowGroup)
                return;

            SelectedItem = container.Item;
        }

        private void ButtonLetterClick(object sender, RoutedEventArgs e)
        {
            _jumpListSelector.Show();
        }
    }
}
