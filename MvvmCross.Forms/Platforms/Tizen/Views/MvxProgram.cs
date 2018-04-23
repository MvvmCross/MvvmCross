using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platforms.Tizen.Core;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Tizen.Views
{
    public class MvxProgram : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        private Application _formsApplication;
        protected Application FormsApplication
        {
            get
            {
                if (_formsApplication == null)
                {
                    var formsPresenter = Mvx.Resolve<IMvxFormsViewPresenter>();
                    _formsApplication = formsPresenter.FormsApplication;
                }

                return _formsApplication;
            }
        }

        private static global::Xamarin.Forms.Platform.Tizen.FormsApplication _application;
        protected static global::Xamarin.Forms.Platform.Tizen.FormsApplication Application
        {
            get
            {
                if (_application == null)
                {
                    _application = new MvxProgram();
                }

                return _application;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            var setup = MvxTizenSetupSingleton.EnsureSingletonAvailable(Application);
            setup.EnsureInitialized();

            LoadApplication(FormsApplication);
        }

        static void Main(string[] args)
        {
            global::Xamarin.Forms.Platform.Tizen.Forms.Init(Application);
            Application.Run(args);
        }
    }
}
