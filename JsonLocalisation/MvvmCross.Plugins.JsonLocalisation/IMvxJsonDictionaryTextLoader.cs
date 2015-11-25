// IMvxJsonDictionaryTextLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Plugins.JsonLocalisation
{
    public interface IMvxJsonDictionaryTextLoader
    {
        void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);

        void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson);
    }
}