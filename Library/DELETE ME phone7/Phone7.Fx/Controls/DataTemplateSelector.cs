using System.Windows;
using System.Windows.Controls;

namespace Phone7.Fx.Controls
{
    public abstract class DataTemplateSelector : ContentControl
    {
        public virtual DataTemplate SelectTemplate(
            object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(
            object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
}