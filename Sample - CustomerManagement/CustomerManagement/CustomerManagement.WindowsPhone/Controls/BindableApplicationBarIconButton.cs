using System;
using System.Windows;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone.Controls
{
	public class BindableApplicationBarIconButton : BindableApplicationBarMenuItem, IApplicationBarIconButton 
	{
        public static readonly DependencyProperty IconUriProperty = DependencyProperty.RegisterAttached("IconUri", typeof(Uri), typeof(BindableApplicationBarMenuItem), new PropertyMetadata(OnIconUriChanged));

        private static void OnIconUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ((BindableApplicationBarIconButton)d).Button.IconUri = (Uri)e.NewValue;
        }

        public BindableApplicationBarIconButton()
            : base()
		{
		}

		public ApplicationBarIconButton Button
		{
			get
			{
				return (ApplicationBarIconButton)Item;
			}
		}

		protected override IApplicationBarMenuItem CreateItem()
		{
			return new ApplicationBarIconButton();
		}

	    public Uri IconUri
	    {
            get
            {
				return (Uri)GetValue(IconUriProperty);
			}
            set
            {
                SetValue(IconUriProperty, value);
            }
	    }
	}
}