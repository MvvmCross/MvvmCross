#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXConventionConstants.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

namespace Cirrious.MonoCross.Extensions.Conventions
{
#warning This class has two separate uses at present - all these values should probably be configurable (IoC/Service style) - when there's time...
    public static class MXConventionConstants
    {
        public const string NullParameterValue = "___n_u_l_l___";
        public const string UrlPathSeparator = "/";
        public const string DefaultAction = "Index";
        public const string ActionParameterKeyName = "Action";
        public const string ControllerParameterKeyName = "Controller";
        public const string ControllerClassSuffix = "Controller";
    }
}