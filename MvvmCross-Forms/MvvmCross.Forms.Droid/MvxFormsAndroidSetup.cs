using System;
using System.Collections.Generic;
using Android.Content;
using MvvmCross.Binding;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Droid.Presenters;
using MvvmCross.Localization;
using MvvmCross.Platform;

namespace MvvmCross.Forms.Droid
{
    public abstract class MvxFormsAndroidSetup : MvxAndroidSetup
    {
        public MvxFormsAndroidSetup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var presenter = new MvxFormsDroidPagePresenter();
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }

        protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<System.Reflection.Assembly>(base.ValueConverterAssemblies);
                toReturn.Add(typeof(MvxLanguageConverter).Assembly);
                return toReturn;
            }
        }

        protected override void InitializeBindingBuilder()
        {
            MvxBindingBuilder bindingBuilder = CreateBindingBuilder();

            this.RegisterBindingBuilderCallbacks();
            bindingBuilder.DoRegistration();
        }

        protected new MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxFormsBindingBuilder();
        }
    }
}
