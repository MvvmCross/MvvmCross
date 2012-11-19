using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using CustomerManagement.WindowsPhone.ExtensionMethods;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone.Controls
{
	[ContentProperty("Buttons")]
	public class BindableApplicationBar : ItemsControl, IApplicationBar
	{
		private ApplicationBar _applicationBar;

		public BindableApplicationBar()
		{
			_applicationBar = new ApplicationBar();

		    _applicationBar.StateChanged += (s, e) =>
		                                        {
		                                            var handler = StateChanged;
                                                    if (handler != null)
                                                        handler(this, e);
		                                        };
			this.Loaded += new RoutedEventHandler(BindableApplicationBar_Loaded);
		}

		private void SetApplicationBar(IApplicationBar bar)
		{
			var page = this.GetVisualAncestors().Where(c => c is PhoneApplicationPage).LastOrDefault() as PhoneApplicationPage;
			if (page != null)
				page.ApplicationBar = bar;
			
			if (bar != null)
				page.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(page_BackKeyPress);
			else
				page.BackKeyPress -= page_BackKeyPress;
		}

		void page_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SetApplicationBar(null);
		}

		void BindableApplicationBar_Loaded(object sender, RoutedEventArgs e)
		{
			SetApplicationBar(_applicationBar);
		}

		protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
		    base.OnItemsChanged(e);
		    ResetApplicationBarDisplay();
		}

	    public void ResetApplicationBarDisplay()
	    {
	        _applicationBar.Buttons.Clear();
	        _applicationBar.MenuItems.Clear();

            foreach (BindableApplicationBarIconButton button in Items.Where(ShowItemAs<BindableApplicationBarIconButton>))
	            _applicationBar.Buttons.Add(button.Button);

            foreach (BindableApplicationBarMenuItem item in Items.Where(c => !(c is BindableApplicationBarIconButton) && ShowItemAs<BindableApplicationBarMenuItem>(c)))
	            _applicationBar.MenuItems.Add(item.Item);
	    }

        private static bool ShowItemAs<T>(object item)
            where T : FrameworkElement
        {
            var button = item as T;
            if (button == null)
                return false;

            if (button.Visibility == Visibility.Collapsed)
                return false;

            return true;
        }

	    public static readonly DependencyProperty IsVisibleProperty =
				DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(BindableApplicationBar), new PropertyMetadata(true, OnVisibleChanged));

		private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
				((BindableApplicationBar)d)._applicationBar.IsVisible = (bool)e.NewValue;
		}

		public static readonly DependencyProperty IsMenuEnabledProperty =
			 DependencyProperty.RegisterAttached("IsMenuEnabled", typeof(bool), typeof(BindableApplicationBar), new PropertyMetadata(true, OnEnabledChanged));

		private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
				((BindableApplicationBar)d)._applicationBar.IsMenuEnabled = (bool)e.NewValue;
		}

		public bool IsVisible
		{
			get
			{
				return (bool)GetValue(IsVisibleProperty);
			}
			set
			{
				SetValue(IsVisibleProperty, value);
			}
		}

		public double BarOpacity
		{
			get
			{
				return _applicationBar.Opacity;
			}
			set
			{
				_applicationBar.Opacity = value;
			}
		}

		public bool IsMenuEnabled
		{
			get
			{
				return (bool)GetValue(IsMenuEnabledProperty);
			}
			set
			{
				SetValue(IsMenuEnabledProperty, value);
			}
		}

		public Color BackgroundColor
		{
			get
			{
				return _applicationBar.BackgroundColor;
			}
			set
			{
				_applicationBar.BackgroundColor = value;
			}
		}

		public Color ForegroundColor
		{
			get
			{
				return _applicationBar.ForegroundColor;
			}
			set
			{
				_applicationBar.ForegroundColor = value;
			}
		}

	    public ApplicationBarMode Mode
	    {
	        get { return _applicationBar.Mode; }
            set { _applicationBar.Mode = value; }
	    }

	    public double DefaultSize
	    {
	        get { return _applicationBar.DefaultSize; }
	    }

	    public double MiniSize
	    {
	        get { return _applicationBar.MiniSize; }
	    }

	    public IList Buttons
		{
			get
			{
				return this.Items;
			}

		}

		public IList MenuItems
		{
			get
			{
				return this.Items;
			}
		}

		public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;
	}
}