// MvxFormsDroidPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// Cirrious.MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Forms.Presenter.Core;

namespace Cirrious.MvvmCross.Forms.Presenter.Droid
{
    public class MvxFormsDroidPagePresenter
        : MvxFormsPagePresenter
        , IMvxAndroidViewPresenter
    {
        public MvxFormsDroidPagePresenter()
        {
        }

        public MvxFormsDroidPagePresenter(MvxFormsApp mvxFormsApp)
            : base(mvxFormsApp)
        {
        }
    }
}
