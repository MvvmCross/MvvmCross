// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Converters;
using Xamarin.Forms;

namespace MvvmCross.Forms.Converters
{
    public class MvxFormsValueConverter : MvxValueConverter, IValueConverter
    {
    }

    public class MvxFormsValueConverter<TFrom> : MvxValueConverter<TFrom>, IValueConverter
    {
    }

    public class MvxFormsValueConverter<TFrom, TTo> : MvxValueConverter<TFrom, TTo>, IValueConverter
    {
    }
}
