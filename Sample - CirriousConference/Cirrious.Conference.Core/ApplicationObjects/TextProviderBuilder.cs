using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;

namespace Cirrious.Conference.Core.ApplicationObjects
{
    public class TextProviderBuilder
        : MvxTextProviderBuilder    
    {
        public TextProviderBuilder() 
            : base(Constants.GeneralNamespace, Constants.RootFolderForResources)
        {
        }

        protected override IDictionary<string, string> ResourceFiles
        {
            get
            {
                var dictionary = this.GetType()
                    .Assembly
                    .GetTypes()
                    .Where(t => t.Name.EndsWith("ViewModel"))
                    .Where(t => !t.Name.StartsWith("Base"))
                    .ToDictionary(t => t.Name, t => t.Name);

                dictionary[Constants.Shared] = Constants.Shared;    
                return dictionary;
            }
        }
    }
}
