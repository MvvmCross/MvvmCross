using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System;

namespace Phone7.Fx.Controls
{
    public class WatermarkedTextBox : Control
    {
        // Fields
        private bool _hasFocus;
        private bool _hasText;
        private TextBox _text;
        public static readonly DependencyProperty InputScopeProperty = DependencyProperty.Register("InputScope", typeof(InputScope), typeof(WatermarkedTextBox), new PropertyMetadata(null));
       
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(WatermarkedTextBox), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(WatermarkedTextBox), new PropertyMetadata((TextWrapping)1));
        public static readonly DependencyProperty WatermarkBrushProperty = DependencyProperty.Register("WatermarkBrush", typeof(Brush), typeof(WatermarkedTextBox), new PropertyMetadata(null));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkedTextBox), new PropertyMetadata(null));

        // Events
        public event TextChangedEventHandler TextChanged;

        // Methods
        public WatermarkedTextBox()
        {
            base.DefaultStyleKey = typeof(WatermarkedTextBox);
        }

        public override void OnApplyTemplate()
        {
            if (this._text != null)
            {
                this._text.GotFocus -= new RoutedEventHandler(this.OnGotFocus);
                this._text.LostFocus -= new RoutedEventHandler(this.OnLostFocus);
                this._text.TextChanged -= new TextChangedEventHandler(this.OnTextChanged);
            }
            base.OnApplyTemplate();
            this._text = base.GetTemplateChild("_text") as TextBox;
            if (this._text != null)
            {
                this._text.GotFocus += new RoutedEventHandler(this.OnGotFocus);
                this._text.LostFocus += new RoutedEventHandler(this.OnLostFocus);
                this._text.TextChanged += new TextChangedEventHandler(this.OnTextChanged);
            }
            this.UpdateVisualStates(false);
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            this._hasFocus = true;
            this.UpdateVisualStates(true);
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            this._hasFocus = false;
            this.UpdateVisualStates(true);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this._hasText = !string.IsNullOrEmpty(this._text.Text);
            this.UpdateVisualStates(true);
            if (this.Text != this._text.Text)
            {
                this.Text = this._text.Text;
            }
            TextChangedEventHandler textChanged = this.TextChanged;
            if (textChanged != null)
            {
                textChanged.Invoke(this._text, e);
            }
        }

        private void UpdateVisualStates(bool useTransitions)
        {
            VisualStateManager.GoToState(this, this._hasText ? "Normal" : "Watermarked", useTransitions);
            VisualStateManager.GoToState(this, this._hasFocus ? "Focused" : "Unfocused", useTransitions);
        }

        // Properties
        public InputScope InputScope
        {
            get
            {
                return (InputScope)base.GetValue(InputScopeProperty);
            }
            set
            {
                base.SetValue(InputScopeProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return (base.GetValue(TextProperty) as string);
            }
            set
            {
                base.SetValue(TextProperty, value);
            }
        }

        public TextWrapping TextWrapping
        {
            get
            {
                return (TextWrapping)base.GetValue(TextWrappingProperty);
            }
            set
            {
                base.SetValue(TextWrappingProperty, value);
            }
        }

        public string Watermark
        {
            get
            {
                return (base.GetValue(WatermarkProperty) as string);
            }
            set
            {
                base.SetValue(WatermarkProperty, value);
            }
        }

        public Brush WatermarkBrush
        {
            get
            {
                return (Brush)base.GetValue(WatermarkBrushProperty);
            }
            set
            {
                base.SetValue(WatermarkBrushProperty, value);
            }
        }
    }


}