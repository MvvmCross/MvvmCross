#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXDefaultControllerConvention.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

#region using

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.MonoCross.Extensions.Interfaces.Conventions;

#endregion

namespace Cirrious.MonoCross.Extensions.Conventions.Default
{
    public class MXDefaultControllerConvention : IMXControllerConvention
    {
        private readonly IEnumerable<MethodInfo> _actions;
        private readonly string _controllerName;
        private readonly string _defaultActionName;

        public MXDefaultControllerConvention(string controllerName, string defaultActionName,
                                             IEnumerable<MethodInfo> actions)
        {
            _controllerName = controllerName;
            _defaultActionName = defaultActionName;
            _actions = actions;
        }

        public string DefaultActionName
        {
            get { return _defaultActionName; }
        }

        public IEnumerable<IMXActionConvention> ActionConventions
        {
            get
            {
                return from action in _actions
                       select new MXActionConvention()
                                  {
                                      ConventionName = action.Name,
                                      Parameters = from parameter in action.GetParameters()
                                                   select new MXParameterConvention
                                                              {
                                                                  ConventionName = parameter.Name,
                                                                  Type = parameter.ParameterType,
                                                                  IsOptional = parameter.IsOptional,
                                                                  DefaultValue = parameter.DefaultValue,
                                                                  // warning - be very very careful using DefaultValues on WP7 or on 
                                                              } as IMXParameterConvention
                                  } as IMXActionConvention;
            }
        }

        public virtual string ConventionName
        {
            get
            {
                // default convention name for "XYZController" is "XYZ"
                if (_controllerName.EndsWith(MXConventionConstants.ControllerClassSuffix))
                    return _controllerName.Substring(0,
                                                     _controllerName.Length -
                                                     MXConventionConstants.ControllerClassSuffix.Length);
                return _controllerName;
            }
        }

#warning Another $%#^ing static - need to find a way of getting this into an interface and getting it shared :/
        public static string CreateControllerConventionName(string input)
        {
            // default convention name for "XYZController" is "XYZ"
            if (input.EndsWith(MXConventionConstants.ControllerClassSuffix) &&
                input.Length > MXConventionConstants.ControllerClassSuffix.Length)
                return input.Substring(0, input.Length - MXConventionConstants.ControllerClassSuffix.Length);
            return input;
        }
    }
}