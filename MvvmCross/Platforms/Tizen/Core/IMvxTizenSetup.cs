using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core;
using Tizen.Applications;

namespace MvvmCross.Platforms.Tizen.Core
{
    public interface IMvxTizenSetup : IMvxSetup
    {
        void PlatformInitialize(CoreApplication coreApplication);
    }
}
