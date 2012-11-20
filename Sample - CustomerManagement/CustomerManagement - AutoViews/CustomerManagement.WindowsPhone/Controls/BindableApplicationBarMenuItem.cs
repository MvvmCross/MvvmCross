using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone.Controls
{
	public class BindableApplicationBarMenuItem : FrameworkElement, IApplicationBarMenuItem
	{
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(BindableApplicationBarMenuItem), new PropertyMetadata(true, OnEnabledChanged));

		public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(BindableApplicationBarMenuItem), new PropertyMetadata(OnTextChanged));

		private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
				((BindableApplicationBarMenuItem)d).Item.IsEnabled = (bool)e.NewValue;
		}

		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
				((BindableApplicationBarMenuItem)d).Item.Text = e.NewValue.ToString();
		}

		public IApplicationBarMenuItem Item
		{
			get;
			set;
		}

		public BindableApplicationBarMenuItem()
		{
			Item = CreateItem();
			if (DesignerProperties.IsInDesignTool)
				Item.Text = "Text";

			Item.Click += (s, e) =>
			{
				if (Click != null)
					Click(s, e);
			};
		}

		protected virtual IApplicationBarMenuItem CreateItem()
		{
			return new ApplicationBarMenuItem();
		}

		public bool IsEnabled
		{
			get
			{
				return (bool)GetValue(IsEnabledProperty);
			}
			set
			{
				SetValue(IsEnabledProperty, value);
			}
		}

		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{                
				SetValue(TextProperty, value);
			}
		}

		public event EventHandler Click;

	}
}