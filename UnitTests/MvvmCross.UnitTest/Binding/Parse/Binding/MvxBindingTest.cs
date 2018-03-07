// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

﻿using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.UnitTest.Binding.Parse.Binding
{
    public abstract class MvxBindingTest : IClassFixture<MvxTestFixture>
    {
        public MvxBindingTest(MvxTestFixture fixture)
        {
        }

        protected void AssertAreEquivalent(MvxSerializableBindingSpecification expected,
                                         MvxSerializableBindingSpecification actual)
        {
            Assert.Equal(expected.Count, actual.Count);
            foreach (var kvp in expected)
            {
                Assert.True(actual.ContainsKey(kvp.Key));
                AssertAreEquivalent(kvp.Value, actual[kvp.Key]);
            }
        }

        protected void AssertAreEquivalent(MvxSerializableBindingDescription expected,
                                         MvxSerializableBindingDescription actual)
        {
            Assert.Equal(expected.Converter, actual.Converter);
            Assert.Equal(expected.ConverterParameter, actual.ConverterParameter);
            Assert.Equal(expected.FallbackValue, actual.FallbackValue);
            Assert.Equal(expected.Mode, actual.Mode);
            Assert.Equal(expected.Path, actual.Path);
            Assert.Equal(expected.Function, actual.Function);
            Assert.Equal(expected.Literal, actual.Literal);
            if (expected.Sources == null)
                Assert.Null(actual.Sources);
            else
            {
                Assert.Equal(expected.Sources.Count, actual.Sources.Count);
                for (var i = 0; i < expected.Sources.Count; i++)
                    AssertAreEquivalent(expected.Sources[i], actual.Sources[i]);
            }
        }
    }
}
