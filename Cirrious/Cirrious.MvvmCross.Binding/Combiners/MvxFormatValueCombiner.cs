// MvxFormatValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Linq;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public class MvxFormatValueCombiner : MvxValueCombiner
    {
        public override bool TryGetValue(System.Collections.Generic.IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var list = steps.ToList();

            object formatObject;
            list.First().TryGetValue(out formatObject);
            var formatString = formatObject == null ? "" : formatObject.ToString();

            var values = list.Skip(1).Select(s =>
                {
                    object stepValue;
                    s.TryGetValue(out stepValue);
                    return stepValue;
                }).ToArray();

            value = string.Format(formatString, values);
            return true;
        }
    }
}