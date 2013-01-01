// IMvxServiceProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Interfaces.ServiceProvider
{
    public interface IMvxServiceProvider
    {
        bool SupportsService<T>() where T : class;
        T GetService<T>() where T : class;
        bool TryGetService<T>(out T service) where T : class;
    }
}