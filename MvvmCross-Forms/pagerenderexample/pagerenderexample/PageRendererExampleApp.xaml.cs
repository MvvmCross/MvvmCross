
using MvvmCross.Platform;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Core.ViewModels;

namespace PageRendererExample
{
    public partial class PageRendererExampleApp : MvxFormsApp
    {
        public PageRendererExampleApp()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            var startUp = Mvx.Resolve<IMvxAppStart>();
            startUp.Start();
        }
    }
}

