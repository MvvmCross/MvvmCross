#region Copyright

// <copyright file="MvxSimpleColorConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Globalization;

namespace Cirrious.MvvmCross.Plugins.Color
{
    public class MvxSimpleColorConverter : MvxBaseColorConverter
    {
        protected override MvxColor Convert(object value, object parameter, CultureInfo culture)
        {
            return (MvxColor) value;
        }
    }
}