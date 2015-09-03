// MvxRgbIntValueConverterTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Globalization;
using NUnit.Framework;

namespace MvvmCross.Plugins.Color.Test
{
    [TestFixture]
    public class MvxRgbIntValueConverterTest : MvxColorValueConverterTest
    {
        private static void RunTests(int[] tests, uint[] results)
        {
            for (var i = 0; i < tests.Length; i++)
            {
                var converter = new MvxRGBIntColorValueConverter();
                var actual = converter.Convert(tests[i], typeof(object), null, CultureInfo.CurrentUICulture);
                var wrapped = actual as WrappedColor;
                Assert.IsNotNull(wrapped);
                Assert.AreEqual(results[i], (uint)wrapped.Color.ARGB);
            }
        }

        [Test]
        public void TestInputs()
        {
            ClearAll();

            var tests = new int[]
                {
                    0xffffff,
                    0x000000,
                    0x123456,
                    0xA23BCD,
                    0x02A040,
                    (int)0x7B02A040,
                };
            var results = new uint[]
                {
                    0xFFffffff,
                    0xFF000000,
                    0xFF123456,
                    0xFFA23BCD,
                    0xFF02A040,
                    0xFF02A040,
                };

            RunTests(tests, results);
        }
    }
}