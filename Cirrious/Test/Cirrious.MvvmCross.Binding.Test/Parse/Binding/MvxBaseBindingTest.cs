using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using Cirrious.MvvmCross.Test.Core;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Binding.Test.Parse.Binding
{
    public abstract class MvxBaseBindingTest
        : BaseIoCSupportingTest
    {
        protected void AssertAreEquivalent(MvxSerializableBindingSpecification expected,
                                         MvxSerializableBindingSpecification actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (var kvp in expected)
            {
                Assert.IsTrue(actual.ContainsKey(kvp.Key));
                AssertAreEquivalent(kvp.Value, actual[kvp.Key]);
            }
        }

        protected void AssertAreEquivalent(MvxSerializableBindingDescription expected,
                                         MvxSerializableBindingDescription actual)
        {
            Assert.AreEqual(expected.Converter, actual.Converter);
            Assert.AreEqual(expected.ConverterParameter, actual.ConverterParameter);
            Assert.AreEqual(expected.FallbackValue, actual.FallbackValue);
            Assert.AreEqual(expected.Mode, actual.Mode);
            Assert.AreEqual(expected.Path, actual.Path);
        }
    }
}