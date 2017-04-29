namespace MasterDetailExample.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }


        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var presenter = new MvxFormsDroidMasterDetailPagePresenter();
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }
    }
}