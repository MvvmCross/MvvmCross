// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class BindingsViewModel : MvxViewModel
    {
        private int _counter = 2;

        public BindingsViewModel()
        {
            _counter = 3;
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            base.SaveStateToBundle(bundle);

            bundle.Data["MyKey"] = _counter.ToString();
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            base.ReloadFromBundle(state);

            _counter = int.Parse(state.Data["MyKey"]);
        }

        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder("Playground.Core", "Text"); }
        }

        private string _bindableText = "I'm bound!";
        public string BindableText
        {
            get
            {
                return _bindableText;
            }
            set
            {
                if (BindableText != value)
                {
                    _bindableText = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
