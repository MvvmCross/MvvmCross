using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Phone7.Fx.Extensions;
using Phone7.Fx.Ioc;

namespace Phone7.Fx.Mvvm
{
    /// <summary>
    /// 
    /// </summary>
    public static class ViewModelMappingManager
    {
        private static Dictionary<Type, Type> _viewModels = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (_viewModels == null)
            {
                _viewModels = new Dictionary<Type, Type>();
                var asemblies = AppDomain.CurrentDomain.GetManifestIncludedAssemblies();
                foreach (Assembly assembly in asemblies)
                {
                    var viewModels = assembly.GetTypes().Where(c => c.GetCustomAttributes(typeof(ViewModelAttribute), true).Count() > 0);
                    foreach (Type viewModel in viewModels)
                    {
                        var customAttributes = (ViewModelAttribute)viewModel.GetCustomAttributes(typeof(ViewModelAttribute), true).FirstOrDefault();
                        if (customAttributes == null)
                            continue;
                        if (!_viewModels.ContainsKey(customAttributes.ViewType))
                        {
                            _viewModels.Add(customAttributes.ViewType, viewModel);
                           
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Creates the view model instance.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="instance"></param>
        /// <returns></returns>
        internal static object CreateViewModelInstance(object instance)
        {
            Initialize();

            Type viewType = instance.GetType();

            if (!_viewModels.ContainsKey(viewType))
                throw new InvalidOperationException("the associated view model is not found for this view !");

            //var interfaces = viewType.GetInterfaces();
            //if (interfaces.Length > 1)
            //{
            //    if (interfaces[1].FullName == typeof(IView).FullName)
            //    {
            //        Container.Current.RegisterInstance(interfaces[0], instance);
            //    }
            //}
            return ObjectBuilder.CreateObject(_viewModels[viewType]);
            //return Container.Current.Resolve(_viewModels[viewType]); // ObjectBuilder.CreateObject(_viewModels[viewType]);
        }
    }
}