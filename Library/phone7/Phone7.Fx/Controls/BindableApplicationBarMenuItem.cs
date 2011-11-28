using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Shell;

namespace Phone7.Fx.Controls
{
    public class BindableApplicationBarMenuItem : FrameworkElement, IApplicationBarMenuItem
    {

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(BindableApplicationBarMenuItem), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(BindableApplicationBarMenuItem), null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public static readonly DependencyProperty CommandParameterValueProperty =
            DependencyProperty.RegisterAttached("CommandParameterValue", typeof(object), typeof(BindableApplicationBarMenuItem), null);

        public object CommandParameterValue
        {
            get { return GetValue(CommandParameterValueProperty); }
            set { SetValue(CommandParameterValueProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(BindableApplicationBarMenuItem), new PropertyMetadata(true, OnEnabledChanged));

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableApplicationBarMenuItem)d).MenuItem.IsEnabled = (bool)e.NewValue;
            }
        }

        public new static readonly DependencyProperty VisibilityProperty =
   DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(BindableApplicationBarMenuItem), new PropertyMetadata(OnVisibilityChanged));

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                var button = ((BindableApplicationBarMenuItem)d);
                BindableApplicationBar bar = button.Parent as BindableApplicationBar;

                bar.Invalidate();
            }
        }

        public new Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(BindableApplicationBarMenuItem), new PropertyMetadata(OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableApplicationBarMenuItem)d).MenuItem.Text = e.NewValue.ToString();
            }
        }

        public ApplicationBarMenuItem MenuItem { get; set; }

        public BindableApplicationBarMenuItem()
        {
            MenuItem = new ApplicationBarMenuItem();
            MenuItem.Text = "Text";
            MenuItem.Click += ApplicationBarMenuItemClick;
        }

        void ApplicationBarMenuItemClick(object sender, EventArgs e)
        {
            if (Command != null && CommandParameter != null)
                Command.Execute(CommandParameter);
            else if (Command != null)
                Command.Execute(CommandParameterValue);
            if (Click != null)
                Click(this, e);
        }

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public event EventHandler Click;


    }
}