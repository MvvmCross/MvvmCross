using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Samples
{
    public class ConvertersViewModel : MvxViewModel
    {
        public string UppercaseConverterTest => "this text was lowercase";

        public string LowercaseConverterTest => "THIS TEXT WAS UPPERCASE";
    }
}
