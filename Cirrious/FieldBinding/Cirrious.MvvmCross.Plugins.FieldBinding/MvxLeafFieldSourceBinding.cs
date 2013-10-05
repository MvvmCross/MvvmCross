// MvxLeafFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.ExtensionMethods;

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
            var fieldType = FieldInfo.FieldType;
            var safeValue = fieldType.MakeSafeValue(value);
            
            // if safeValue matches the existing value, then don't call set
            if (EqualsCurrentValue(safeValue))
                return;

            FieldInfo.SetValue(Source, safeValue);
        }

        public override Type SourceType
        {
            get { return FieldInfo.FieldType; }
        }

        public override object GetValue()
        {
            return FieldInfo.GetValue(Source);
        }
    }
}