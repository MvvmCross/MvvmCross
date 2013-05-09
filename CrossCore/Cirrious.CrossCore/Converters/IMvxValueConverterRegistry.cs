// IMvxValueConverterRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;

namespace Cirrious.CrossCore.Converters
{
    public interface IMvxValueConverterRegistry
    {
        void AddOrOverwrite(string converterName, IMvxValueConverter converter);
        void AddOrOverwriteFrom(Assembly assembly);
    }
}