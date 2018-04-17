using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MvvmCross.Forms.Core
{
    public interface IMvxFormsSetup
    {
        Application FormsApplication { get; }
    }
}
