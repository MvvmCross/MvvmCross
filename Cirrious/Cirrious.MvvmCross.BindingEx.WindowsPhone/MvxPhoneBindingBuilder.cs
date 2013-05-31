// MvxPhoneBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Windows.Media;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone
{
    public class MvxPhoneBindingBuilder : MvxCoreBindingBuilder
    {
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;

        public MvxPhoneBindingBuilder(params Assembly[] valueConverterAssemblies)
        {
            _fillValueConvertersAction = (registry) => registry.Fill(valueConverterAssemblies);
        }

        public MvxPhoneBindingBuilder(
            Action<IMvxValueConverterRegistry> fillValueConvertersAction = null)
        {
            _fillValueConvertersAction = fillValueConvertersAction;
        }

        public override void DoRegistration()
        {
            base.DoRegistration();
            InitializeBindingCreator();
        }

        private void InitializeBindingCreator()
        {
            var creator = CreateBindingCreator();
            Mvx.RegisterSingleton<IMvxBindingCreator>(creator);
        }

        protected virtual MvxBindingCreator CreateBindingCreator()
        {
            return new MvxBindingCreator();
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            registry.Fill(this.GetType().Assembly);
            registry.Fill(typeof(Localization.MvxLanguageConverter).Assembly);

            if (_fillValueConvertersAction != null)
                _fillValueConvertersAction(registry);
        }
    }
}