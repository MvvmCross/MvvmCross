// MvxWithArgsEventToCommand.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Commands
{
    public class MvxWithArgsEventToCommand : MvxEventToCommand
    {
        public MvxWithArgsEventToCommand()
        {
            this.PassEventArgsToCommand = true;
        }
    }
}