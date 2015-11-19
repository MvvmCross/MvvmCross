// MvxPostfixAwareViewToViewModelNameMapping.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxPostfixAwareViewToViewModelNameMapping
        : MvxViewToViewModelNameMapping
    {
        private readonly string[] _postfixes;

        public MvxPostfixAwareViewToViewModelNameMapping(params string[] postfixes)
        {
            _postfixes = postfixes;
        }

        public override string Map(string inputName)
        {
            foreach (var postfix in _postfixes)
            {
                if (inputName.EndsWith(postfix) && inputName.Length > postfix.Length)
                {
                    inputName = inputName.Substring(0, inputName.Length - postfix.Length);
                    break;
                }
            }
            return base.Map(inputName);
        }
    }
}