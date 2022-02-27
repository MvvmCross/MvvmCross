// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Converters;

namespace MvvmCross.Binding.Combiners
{
    public class MvxFormatValueCombiner : MvxValueCombiner
    {
        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var list = steps.ToList();

            if (list.Count < 1)
            {
                MvxBindingLog.Warning("Format called with no parameters - will fail");
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
