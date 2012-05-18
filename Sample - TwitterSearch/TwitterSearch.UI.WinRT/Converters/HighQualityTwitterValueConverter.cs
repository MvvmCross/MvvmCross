using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Converters;

namespace TwitterSearch.UI.Win8.Converters
{
    public class HighQualityTwitterValueConverter 
        : MvxBaseValueConverter
    {
        public override object Convert(object value, Type type, object parmeter, CultureInfo cultureInfo)
        {
            return ((string) value).Replace("_normal", string.Empty);
        }
    }
}
