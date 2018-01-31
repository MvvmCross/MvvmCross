﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Test.Mocks.ViewModels
{
    public class SimpleParameterTestViewModel : MvxViewModel<string>
    {
        public string Parameter { get; set; }

        public virtual void Init()
        {
        }

        public override void Prepare(string parameter)
        {
            Parameter = parameter;
        }
    }
}
