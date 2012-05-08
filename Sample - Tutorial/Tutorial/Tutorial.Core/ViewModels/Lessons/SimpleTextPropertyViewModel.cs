using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.ViewModels;

namespace Tutorial.Core.ViewModels.Lessons
{
    public class SimpleTextPropertyViewModel
        : MvxViewModel
    {
        private string _theText;
        public string TheText
        {
            get { return _theText; }
            set { _theText = value; RaisePropertyChanged(() => TheText); }
        }

        public SimpleTextPropertyViewModel()
        {
            TheText = "Hello MvvmCross";
        }
    }
}