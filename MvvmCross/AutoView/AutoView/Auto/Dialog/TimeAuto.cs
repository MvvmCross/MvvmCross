// TimeAuto.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto.Dialog
{
    using System;
    using System.Linq.Expressions;
    using System.Windows.Input;

    public class TimeAuto : ValueElementAuto
    {
        public TimeAuto(string key = null, Expression<Func<object>> bindingExpression = null, string converter = null,
                        string converterParameter = null, string value = null, string caption = null,
                        string onlyFor = null, string notFor = null, Expression<Func<ICommand>> selectedCommand = null,
                        string layoutName = null)
            : base(
                key ?? "Time", bindingExpression, converter, converterParameter, value, caption, onlyFor, notFor,
                selectedCommand, layoutName)
        {
        }
    }
}