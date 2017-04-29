namespace PageRendererExample.UI.Droid
{
    [Activity(Label = "MvxFormsApplicationActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MvxFormsApplicationActivity
        : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);
            var mvxFormsApp = new MvxFormsApp();
            LoadApplication(mvxFormsApp);

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidPagePresenter;
            presenter.MvxFormsApp = mvxFormsApp;

            Mvx.Resolve<IMvxAppStart>().Start();
        }
    }
}