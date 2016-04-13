// IMvxJsonConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.IO;

namespace MvvmCross.Platform.Platform
{
    public interface IMvxJsonConverter : IMvxTextSerializer
    {
		T DeserializeObject<T>(Stream stream);
    }
}