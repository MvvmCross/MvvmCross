using MvvmCross.Droid.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;

namespace MvvmCross.Forms.Droid.Presenters
{
    public class MvxFormsDroidMasterDetailPagePresenter
        : MvxFormsMasterDetailPagePresenter
        , IMvxAndroidViewPresenter
    {
        public MvxFormsDroidMasterDetailPagePresenter()
        {
        }

        public MvxFormsDroidMasterDetailPagePresenter(MvxFormsApplication mvxFormsApp)
            : base(mvxFormsApp)
        {
        }
    }
}