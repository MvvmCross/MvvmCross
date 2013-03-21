// IMvxDataConsumer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.CrossCore.Core
{
#warning Really needs another name - IMvxDataConsumer sucks (and it doesn't really consume)
    public interface IMvxDataConsumer
    {
        object DataContext { get; set; }
    }
}