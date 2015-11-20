// MvxFormatValueCombiner.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Bindings.SourceSteps;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Combiners
{
    public class MvxFormatValueCombiner : MvxValueCombiner
    {
        public override bool TryGetValue(System.Collections.Generic.IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var list = steps.ToList();

            if (list.Count < 1)
            {
                MvxBindingTrace.Warning("Format called with no parameters - will fail");
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            var formatObject = list.First().GetValue();
            if (formatObject == MvxBindingConstant.DoNothing)
            {
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            if (formatObject == MvxBindingConstant.UnsetValue)
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            var formatString = formatObject == null ? "" : formatObject.ToString();

            var values = list.Skip(1).Select(s => s.GetValue()).ToArray();

            if (values.Any(v => v == MvxBindingConstant.DoNothing))
            {
                value = MvxBindingConstant.DoNothing;
                return true;
            }

            if (values.Any(v => v == MvxBindingConstant.UnsetValue))
            {
                value = MvxBindingConstant.UnsetValue;
                return true;
            }

            value = string.Format(formatString, values);
            return true;
        }
    }
}