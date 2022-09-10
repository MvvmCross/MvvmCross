// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.UnitTest.Binding.Mocks
{
    public class MockTargetBinding : IMvxTargetBinding
    {
        public MockTargetBinding()
        {
            TargetValueType = typeof(object);
        }

        public int DisposeCalled { get; set; }

        public void Dispose()
        {
            DisposeCalled++;
        }

        public int SubscribeToEventsCalled { get; set; }

        public void SubscribeToEvents()
        {
            SubscribeToEventsCalled++;
        }

        public Type TargetValueType { get; set; }
        public MvxBindingMode DefaultMode { get; set; }

        public List<object> Values { get; } = new List<object>();

        public void SetValue(object value)
        {
            Values.Add(value);
        }

        public void FireValueChanged(MvxTargetChangedEventArgs args)
        {
            ValueChanged?.Invoke(this, args);
        }

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;
    }
}
