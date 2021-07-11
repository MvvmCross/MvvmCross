using System;
using System.Collections.Generic;
using System.Text;

namespace Playground.Wpf.UI
{
    public class App : Playground.Core.App
    {
        public override void Initialize()
        {
            RegisterAppStart<Playground.Wpf.UI.ViewModels.RootViewModel>();
            base.Initialize();
        }
    }
}
