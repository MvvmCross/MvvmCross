#region Copyright

// <copyright file="BaseKeyedUserInterfaceBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrossUI.Core.Descriptions;

namespace CrossUI.Core.Builder
{
    public abstract class BaseKeyedUserInterfaceBuilder<TInterface> : BaseUserInterfaceBuilder
    {
        public IDictionary<string, Type> KnownKeys { get; private set; }
        public string ConventionalEnding { get; set; }
        public string DefaultKey { get; set; }

        protected BaseKeyedUserInterfaceBuilder(string platformName, string conventionalEnding, string defaultKey)
            : base(platformName)
        {
            DefaultKey = defaultKey;
            ConventionalEnding = conventionalEnding;
            KnownKeys = new Dictionary<string, Type>();
        }

        public void RegisterConventionalKeys(Assembly assembly, string keyNamesEndWith = null)
        {
            keyNamesEndWith = keyNamesEndWith ?? ConventionalEnding;
            var elementTypes = assembly.GetTypes()
                                       .Where(t => t.Name.EndsWith(keyNamesEndWith))
                                       .Where(t => !t.IsAbstract)
                                       .Where(t => typeof (TInterface).IsAssignableFrom(t));

            foreach (var elementType in elementTypes)
            {
                var name = elementType.Name;
                if (name.EndsWith(keyNamesEndWith))
                    name = name.Substring(0, name.Length - keyNamesEndWith.Length);

                KnownKeys[name] = elementType;
            }
        }

        public object Build(KeyedDescription description)
        {
            if (!ShouldBuildDescription(description))
            {
                return null;
            }

            var key = string.IsNullOrEmpty(description.Key) ? DefaultKey : description.Key;
            Type type;
            if (!KnownKeys.TryGetValue(key, out type))
            {
                throw new KeyNotFoundException("Could not find class for " + description.Key);
            }

            var constructor = type.GetConstructors()
                                  .FirstOrDefault(c => c.GetParameters().All(p => p.IsOptional));
            if (constructor == null)
            {
                throw new ArgumentException("No parameterless Constructor found for " + key);
            }

            // due to Mono and WP implementation issues, we don't use Type.Missing
            //var parameters = constructor.GetParameters().Select(p => (object)Type.Missing).ToArray();
            var parameters = constructor.GetParameters().Select(p => p.DefaultValue).ToArray();
            var instance = constructor.Invoke(null, parameters);

            return instance;
        }
    }
}