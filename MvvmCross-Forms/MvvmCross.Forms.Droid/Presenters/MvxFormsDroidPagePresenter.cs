// MvxFormsDroidPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using MvvmCross.Droid.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;

namespace MvvmCross.Forms.Droid.Presenters
{
    public class MvxFormsDroidPagePresenter
        : MvxFormsPagePresenter
        , IMvxAndroidViewPresenter
    {
        public MvxFormsDroidPagePresenter()
        {
        }

        public MvxFormsDroidPagePresenter(MvxFormsApplication mvxFormsApp)
            : base(mvxFormsApp)
        {
        }
    }
}
