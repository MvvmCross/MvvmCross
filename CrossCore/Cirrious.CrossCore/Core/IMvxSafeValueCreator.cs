using System;
using System.Globalization;

namespace Cirrious.CrossCore.Core
{
    public interface IMvxSafeValueCreator
    {
        bool ConvertToBooleanCore(object result);

        object MakeSafeValueCore(Type propertyType, object value);
    }
}
