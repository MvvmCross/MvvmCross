using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;

namespace Phone7.Fx.Mvvm
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.DataContext = ViewModelMappingManager.CreateViewModelInstance(AssociatedObject);
        }
    }
}