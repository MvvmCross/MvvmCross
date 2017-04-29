using MvvmCross.Droid.Views;
using MvvmCross.Forms.Presenter.Core;

namespace MvvmCross.Forms.Presenter.Droid
{
    public class MvxFormsDroidMasterDetailPagePresenter
        : MvxFormsMasterDetailPagePresenter
        , IMvxAndroidViewPresenter
    {
        public MvxFormsDroidMasterDetailPagePresenter()
        {
        }

        public MvxFormsDroidMasterDetailPagePresenter(MvxFormsApp mvxFormsApp)
            : base(mvxFormsApp)
        {
        }
    }
}