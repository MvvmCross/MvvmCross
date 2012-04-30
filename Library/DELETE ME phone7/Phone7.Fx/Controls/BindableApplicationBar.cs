using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Phone7.Fx.Controls
{
    [ContentProperty("Buttons")]
    public class BindableApplicationBar : ItemsControl, IApplicationBar
    {
        // ApplicationBar wrappé
        private readonly ApplicationBar _applicationBar;

        public BindableApplicationBar()
        {
            _applicationBar = new ApplicationBar();
            this.Loaded += BindableApplicationBar_Loaded;
        }

        void BindableApplicationBar_Loaded(object sender, RoutedEventArgs e)
        {
            // AU chargement du control, on recherche la page ou est situé le control pour lui associé notre applicationbar
            var page =
                this.GetVisualAncestors().Where(c => c is PhoneApplicationPage).FirstOrDefault() as PhoneApplicationPage;
            if (page != null) page.ApplicationBar = _applicationBar;
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            Invalidate();
        }

        public void Invalidate()
        {
            // On efface tout, pour être sur...
            _applicationBar.Buttons.Clear();
            _applicationBar.MenuItems.Clear();
            // On récupère tous les boutons défini dans les items de l'ItemsControl
            foreach (BindableApplicationBarIconButton button in Items.Where(c => c is BindableApplicationBarIconButton && ((BindableApplicationBarIconButton)c).Visibility == Visibility.Visible))
            {
                // on les affectes a l'application bar
                _applicationBar.Buttons.Add(button.Button);
            }
            foreach (BindableApplicationBarMenuItem button in Items.Where(c => c is BindableApplicationBarMenuItem && ((BindableApplicationBarMenuItem)c).Visibility == Visibility.Visible))
            {
                _applicationBar.MenuItems.Add(button.MenuItem);
            }
        }

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(BindableApplicationBar), new PropertyMetadata(true, OnVisibleChanged));
        
        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableApplicationBar)d)._applicationBar.IsVisible = (bool)e.NewValue;
            }
        }

#if MANGO

        public static readonly DependencyProperty ModeProperty =
          DependencyProperty.RegisterAttached("Mode", typeof(ApplicationBarMode), typeof(BindableApplicationBar), new PropertyMetadata(ApplicationBarMode.Default, OnModeChanged));

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableApplicationBar)d)._applicationBar.Mode = (ApplicationBarMode)e.NewValue;
            }
        }

        public ApplicationBarMode Mode
        {
            get { return (ApplicationBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

            public double DefaultSize
        {
            get { return _applicationBar.DefaultSize; }
        }

        public double MiniSize
        {
            get { return _applicationBar.MiniSize; }
        }

#endif

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public double BarOpacity
        {
            get { return _applicationBar.Opacity; }
            set { _applicationBar.Opacity = value; }
        }

        public bool IsMenuEnabled
        {
            get { return _applicationBar.IsMenuEnabled; }
            set { _applicationBar.IsMenuEnabled = true; }
        }

        public Color BackgroundColor
        {
            get { return _applicationBar.BackgroundColor; }
            set { _applicationBar.BackgroundColor = value; }
        }

        public Color ForegroundColor
        {
            get { return _applicationBar.ForegroundColor; }
            set { _applicationBar.ForegroundColor = value; }
        }

        public IList Buttons
        {
            get { return this.Items; }

        }

        public IList MenuItems
        {
            get { return this.Items; }
        }

        public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;
    }
}