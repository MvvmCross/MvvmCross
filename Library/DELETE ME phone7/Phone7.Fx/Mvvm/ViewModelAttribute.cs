using System;

namespace Phone7.Fx.Mvvm
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ViewModelAttribute : Attribute
    {
        public Type ViewType { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelAttribute"/> class.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        public ViewModelAttribute(Type viewType)
        {
            ViewType = viewType;
        }
    }
}