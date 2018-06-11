// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Logging
{
    public interface IMvxLogProvider
    {
        IMvxLog GetLogFor(Type type);

        IMvxLog GetLogFor<T>();

        IMvxLog GetLogFor(string name);

        IDisposable OpenNestedContext(string message);

        IDisposable OpenMappedContext(string key, string value);
    }
}
