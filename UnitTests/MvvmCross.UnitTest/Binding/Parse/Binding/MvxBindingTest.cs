// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Tests;

namespace MvvmCross.UnitTest.Binding.Parse.Binding
{
    public abstract class MvxBindingTest
    {
        protected MvxBindingTest(MvxTestFixture fixture)
        {
            fixture.ClearAll();
        }

        protected async Task AssertAreEquivalent(MvxSerializableBindingSpecification expected,
                                         MvxSerializableBindingSpecification actual)
        {
            await Assert.That(actual.Count).IsEqualTo(expected.Count);
            foreach (var kvp in expected)
            {
                await Assert.That(actual.ContainsKey(kvp.Key)).IsTrue();
                await AssertAreEquivalent(kvp.Value, actual[kvp.Key]);
            }
        }

        protected async Task AssertAreEquivalent(MvxSerializableBindingDescription expected,
                                         MvxSerializableBindingDescription actual)
        {
            await Assert.That(actual.Converter).IsEqualTo(expected.Converter);
            await Assert.That(actual.ConverterParameter).IsEqualTo(expected.ConverterParameter);
            await Assert.That(actual.FallbackValue).IsEqualTo(expected.FallbackValue);
            await Assert.That(actual.Mode).IsEqualTo(expected.Mode);
            await Assert.That(actual.Path).IsEqualTo(expected.Path);
            await Assert.That(actual.Function).IsEqualTo(expected.Function);
            await Assert.That(actual.Literal).IsEqualTo(expected.Literal);
            if (expected.Sources == null)
                await Assert.That(actual.Sources).IsNull();
            else
            {
                await Assert.That(actual.Sources.Count).IsEqualTo(expected.Sources.Count);
                for (var i = 0; i < expected.Sources.Count; i++)
                    await AssertAreEquivalent(expected.Sources[i], actual.Sources[i]);
            }
        }
    }
}
