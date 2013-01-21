// MvxJsonBindingDescriptionParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Interfaces.Parse;

namespace Cirrious.MvvmCross.Binding.Parse.Binding.Json
{
    public class MvxJsonBindingDescriptionParser
        : MvxBaseBindingDescriptionParser
    {
        protected override IMvxBindingParser CreateParser()
        {
            return new MvxJsonBindingParser();
        }        
    }
}