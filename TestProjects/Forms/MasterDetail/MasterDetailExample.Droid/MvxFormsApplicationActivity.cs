namespace MasterDetailExample.Droid
{
    [Activity(Label = "MvxFormsApplicationActivity", Icon = "@android:color/transparent")]
    public class MvxFormsApplicationActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            var mvxFormsApp = new MvxFormsApp();
            LoadApplication(mvxFormsApp);

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidMasterDetailPagePresenter;
            presenter.MvxFormsApp = mvxFormsApp;

            Mvx.Resolve<IMvxAppStart>().Start();
        }
    }
}