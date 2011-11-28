using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Phone7.Fx.Controls.JumpList
{
    public partial class JumpListSelector : UserControl
    {
        public event EventHandler<GEventArgs<object>> LetterClicked;

        public class LetterVisibiliy
        {
            public char Letter { get; set; }
            public bool Enable { get; set; }
          
        }

        public JumpListSelector()
        {
            InitializeComponent();

        }



        private void LetterClick(object sender, RoutedEventArgs e)
        {
            hideLetters.Completed += (ss, ee) =>
                                         {
                                             lettersGrid.Visibility = Visibility.Collapsed;
                                             var data = _itemsSource.First(c => c.FirstLetter.ToString() == ((Button)sender).Content.ToString());
                                             if (LetterClicked != null)
                                                 LetterClicked(this, new GEventArgs<object> { Data = data });
                                         };
            hideLetters.Begin();
        }

        internal void Show()
        {

            lettersGrid.Visibility = Visibility.Visible;

            if (_warmUp)
            {
                _warmUp = false;
                Tools.PreLaunchStoryboard(lettersGrid, showLetters, 0.1, "Opacity", 0, 0.01);
            }
            else
            {
                showLetters.Begin();
            }
        }



        private AlphabetizedCollection _itemsSource;
        private bool _warmUp = true;

        public AlphabetizedCollection ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                _itemsSource = value;
                List<LetterVisibiliy> list = new List<LetterVisibiliy>();
                foreach (char c in "#abcdefghijklmnopqrstuvwxyz")
                {
                    list.Add(new LetterVisibiliy { Letter = c, Enable = false });
                }
                var val = value.Where(c => c.ShowGroup).Select(c => new { Letter = c.FirstLetter, Ctx = c.Item });
                foreach (var item in list.Where(item => val.Any(c => c.Letter == item.Letter)))
                {
                    item.Enable = true;
                }
                this.DataContext = list;
            }
        }
    }

    internal static class Tools
    {
        public static void AddNewAnimation(Storyboard sb, DependencyObject source, string property, double from, double to, double transitionSpeed)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = TimeSpan.FromSeconds(transitionSpeed);

            Storyboard.SetTarget(animation, source);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            sb.Children.Add(animation);
        }

        public static void PreLaunchStoryboard(FrameworkElement root, Storyboard sb, double delay, string property, double from, double to)
        {
            Storyboard preSB = new Storyboard();
            AddNewAnimation(preSB, root, property, from, to, delay);

            preSB.Completed += (s, e) => sb.Begin();
            preSB.Begin();
        }
    }
}
