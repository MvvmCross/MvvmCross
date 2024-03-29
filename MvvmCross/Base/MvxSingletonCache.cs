// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;

namespace MvvmCross.Base;

public sealed class MvxSingletonCache
    : MvxSingleton<IMvxSingletonCache>, IMvxSingletonCache
{
    private bool _inpcInterceptorResolveAttempted;
    private IMvxInpcInterceptor? _inpcInterceptor;
    private IMvxStringToTypeParser? _parser;
    private IMvxSettings? _settings;

    public static MvxSingletonCache Initialize()
    {
        if (Instance != null)
            throw new MvxException("You should only initialize MvxBindingSingletonCache once");

        return new MvxSingletonCache();
    }

    private MvxSingletonCache()
    {
    }

    public IMvxInpcInterceptor? InpcInterceptor
    {
        get
        {
            if (_inpcInterceptorResolveAttempted)
                return _inpcInterceptor;

            Mvx.IoCProvider?.TryResolve(out _inpcInterceptor);
            _inpcInterceptorResolveAttempted = true;
            return _inpcInterceptor;
        }
    }

    public IMvxStringToTypeParser? Parser
    {
        get
        {
            _parser ??= Mvx.IoCProvider?.Resolve<IMvxStringToTypeParser>();
            return _parser;
        }
    }

    public IMvxSettings? Settings
    {
        get
        {
            _settings ??= Mvx.IoCProvider?.Resolve<IMvxSettings>();
            return _settings;
        }
    }
}
