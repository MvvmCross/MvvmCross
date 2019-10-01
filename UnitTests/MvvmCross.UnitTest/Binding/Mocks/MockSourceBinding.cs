﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Converters;

namespace MvvmCross.UnitTest.Binding.Mocks
{
    public class MockSourceBinding : IMvxSourceBinding
    {
        public MockSourceBinding()
        {
            SourceType = typeof(object);
        }

        public int DisposeCalled { get; set; }

        public void Dispose()
        {
            DisposeCalled++;
        }

        public Type SourceType { get; set; }

        public List<object> ValuesSet { get; } = new List<object>();

        public void SetValue(object value)
        {
            ValuesSet.Add(value);
        }

        public void FireSourceChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Changed;

        public bool TryGetValueResult { get; set; }
        public object TryGetValueValue { get; set; }

        public object GetValue()
        {
            if (!TryGetValueResult)
                return MvxBindingConstant.UnsetValue;

            return TryGetValueValue;
        }
    }
}
