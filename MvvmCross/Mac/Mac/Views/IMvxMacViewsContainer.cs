using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Mac.Views
{
    public interface IMvxMacViewsContainer
         : IMvxViewsContainer
         , IMvxMacViewCreator
         , IMvxCurrentRequest
    {
    }
}
