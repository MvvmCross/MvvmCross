// MvxLeafFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;

namespace Cirrious.MvvmCross.Plugins.FieldBinding
{
    public class MvxLeafFieldSourceBinding
        : MvxFieldSourceBinding
    {
        public MvxLeafFieldSourceBinding(object source, FieldInfo fieldInfo)
            : base(source, fieldInfo)
        {
        }

        public override void SetValue(object value)
        {
            FieldInfo.SetValue(Source, value);
        }

        public override Type SourceType
        {
            get { return FieldInfo.FieldType; }
        }

        public override bool TryGetValue(out object value)
        {
            value = FieldInfo.GetValue(Source);
            return true;
        }
    }
}