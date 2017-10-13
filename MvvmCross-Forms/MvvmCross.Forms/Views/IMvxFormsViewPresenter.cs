using System;
using MvvmCross.Forms.Platform;

namespace MvvmCross.Forms.Views
{
    public interface IMvxFormsViewPresenter
    {
        MvxFormsApplication FormsApplication { get; set; }
    }
}
