namespace MvvmCross.Binding.Test.Parse.Binding
{
    using MvvmCross.Binding.Parse.Binding;
    using MvvmCross.Test.Core;

    using NUnit.Framework;

    public abstract class MvxBindingTest
        : MvxIoCSupportingTest
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
            Assert.AreEqual(expected.Function, actual.Function);
            Assert.AreEqual(expected.Literal, actual.Literal);
            if (expected.Sources == null)
                Assert.IsNull(actual.Sources);
            else
            {
                Assert.AreEqual(expected.Sources.Count, actual.Sources.Count);
                for (var i = 0; i < expected.Sources.Count; i++)
                    AssertAreEquivalent(expected.Sources[i], actual.Sources[i]);
            }
        }
    }
}