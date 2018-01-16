using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.JsonLocalization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Playground.Core.Services
{
    public class TextProviderBuilder : MvxTextProviderBuilder
    {
        public TextProviderBuilder() : base("Playground.Core", "Resources", new MvxEmbeddedJsonDictionaryTextProvider(false))
        {
        }

        protected override IDictionary<string, string> ResourceFiles
        {
            get
            {
                var dictionary = GetType().GetTypeInfo()
                                     .Assembly
                                     .CreatableTypes()
                                     .Where(t => t.Name.EndsWith("ViewModel"))
                                     .ToDictionary(t => t.Name, t => t.Name);

                dictionary.Add("Text", "Text");

                return dictionary;
            }
        }
    }
}