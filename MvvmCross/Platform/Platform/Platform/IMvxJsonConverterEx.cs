// IMvxJsonConverterEx.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

using System.IO;

namespace MvvmCross.Platform.Platform
{
    public interface IMvxJsonConverterEx 
        : IMvxJsonConverter
    {
        T DeserializeObject<T>(Stream stream);
    }
}