// MvxRgbaValueConverterTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using NUnit.Framework;
using System.Globalization;

namespace MvvmCross.Plugins.Color.Test
{
    [TestFixture]
    public class MvxRgbaValueConverterTest : MvxColorValueConverterTest
    {
        [Test]
        public void Test3DigitStrings()
        {
            ClearAll();

            var tests = new string[]
                {
                    "fff",
                    "FFF",
                    "000",
                    "123",
                    "A23",
                    "02A"
                };
            var results = new uint[]
                {
                    0xFFffffff,
                    0xFFffffff,
                    0xFF000000,
                    0xFF112233,
                    0xFFAA2233,
                    0xFF0022AA
                };

            RunTests(tests, results);
        }

        private static void RunTests(string[] tests, uint[] results)
        {
            for (var i = 0; i < tests.Length; i++)
            {
                var converter = new MvxRGBAValueConverter();
                var actual = converter.Convert(tests[i], typeof(object), null, CultureInfo.CurrentUICulture);
                var wrapped = actual as WrappedColor;
                Assert.IsNotNull(wrapped);
                Assert.AreEqual(results[i], (uint)wrapped.Color.ARGB);
            }

            for (var i = 0; i < tests.Length; i++)
            {
                var converter = new MvxRGBAValueConverter();
                var actual = converter.Convert("#" + tests[i], typeof(object), null, CultureInfo.CurrentUICulture);
                var wrapped = actual as WrappedColor;
                Assert.IsNotNull(wrapped);
                Assert.AreEqual(results[i], (uint)wrapped.Color.ARGB);
            }
        }

        [Test]
        public void Test6DigitStrings()
        {
            ClearAll();

            var tests = new string[]
                {
                    "ffffff",
                    "FFFFFF",
                    "000000",
                    "123456",
                    "A23BCD",
                    "02A040"
                };
            var results = new uint[]
                {
                    0xFFffffff,
                    0xFFffffff,
                    0xFF000000,
                    0xFF123456,
                    0xFFA23BCD,
                    0xFF02A040
                };

            RunTests(tests, results);
        }

        [Test]
        public void Test8DigitStrings()
        {
            ClearAll();

            // NOTE - MvxColor stores ARGB, but these strings are RGBA
            var tests = new string[]
                {
                    "ffffffff",
                    "FFFFFFFF",
                    "00000000",
                    "12345678",
                    "A23BCDA9",
                    "02A04012"
                };
            var results = new uint[]
                {
                    0xFFffffff,
                    0xFFffffff,
                    0x00000000,
                    0x78123456,
                    0xA9A23BCD,
                    0x1202A040
                };

            RunTests(tests, results);
        }
    }
}