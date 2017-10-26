﻿using System;

namespace MvvmCross.Platform.Logging.LogProviders
{
    internal class DisposableAction : IDisposable
    {
        private readonly Action _onDispose;

        public DisposableAction(Action onDispose = null)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            if (_onDispose == null) return;
            _onDispose();
        }
    }
}
