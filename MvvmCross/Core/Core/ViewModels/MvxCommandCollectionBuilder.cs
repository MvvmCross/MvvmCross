﻿// MvxCommandCollectionBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Platform;

namespace MvvmCross.Core.ViewModels
{
    public class MvxCommandCollectionBuilder
        : IMvxCommandCollectionBuilder
    {
        private const string DefaultCommandSuffix = "Command";
        private const string DefaultCanExecutePrefix = "CanExecute";

        public string CommandSuffix { get; set; }
        public IEnumerable<string> AdditionalCommandSuffixes { get; set; }
        public string CanExecutePrefix { get; set; }

        public MvxCommandCollectionBuilder()
        {
            CanExecutePrefix = DefaultCanExecutePrefix;
            CommandSuffix = DefaultCommandSuffix;
            AdditionalCommandSuffixes = null;
        }

        public virtual IMvxCommandCollection BuildCollectionFor(object owner)
        {
            var toReturn = new MvxCommandCollection(owner);
            CreateCommands(owner, toReturn);
            return toReturn;
        }

        protected virtual void CreateCommands(object owner, MvxCommandCollection toReturn)
        {
            var commandMethods =
                from method in
                    owner.GetType()
                         .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                let parameterCount = method.GetParameters().Count()
                where parameterCount <= 1
                let commandName = GetCommandNameOrNull(method)
                where !string.IsNullOrEmpty(commandName)
                select new { Method = method, CommandName = commandName, HasParameter = parameterCount > 0 };

            foreach (var item in commandMethods)
            {
                CreateCommand(owner, toReturn, item.Method, item.CommandName, item.HasParameter);
            }
        }

        protected virtual void CreateCommand(object owner, MvxCommandCollection collection, MethodInfo commandMethod,
                                             string commandName, bool hasParameter)
        {
            var canExecuteProperty = CanExecutePropertyInfo(owner.GetType(), commandMethod);

            var helper = hasParameter
                             ? (IMvxCommandBuilder)
                               new MvxParameterizedCommandBuilder(commandMethod, canExecuteProperty)
                             : new MvxCommandBuilder(commandMethod, canExecuteProperty);

            var command = helper.ToCommand(owner);
            collection.Add(command, commandName, helper.CanExecutePropertyName);
        }

        protected virtual PropertyInfo CanExecutePropertyInfo(Type type, MethodInfo commandMethod)
        {
            var canExecuteName = CanExecuteProperyName(commandMethod);
            if (string.IsNullOrEmpty(canExecuteName))
                return null;
            var canExecute = type.GetProperty(canExecuteName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            if (canExecute == null)
                return null;
            if (canExecute.PropertyType != typeof(bool))
                return null;
            if (!canExecute.CanRead)
                return null;

            return canExecute;
        }

        protected virtual string GetCommandNameOrNull(MethodInfo method)
        {
            var commandAttribute = CommandAttribute(method);
            if (commandAttribute != null)
                return commandAttribute.CommandName;

            var name = GetConventionalCommandNameOrNull(method, CommandSuffix);
            if (name != null)
                return name;

            if (AdditionalCommandSuffixes != null)
            {
                foreach (var additionalCommandSuffix in AdditionalCommandSuffixes)
                {
                    name = GetConventionalCommandNameOrNull(method, additionalCommandSuffix);
                    if (name != null)
                        return name;
                }
            }

            return null;
        }

        protected virtual string GetConventionalCommandNameOrNull(MethodInfo method, string suffix)
        {
            if (!method.Name.EndsWith(suffix))
                return null;

            var length = method.Name.Length - suffix.Length;
            if (length <= 0)
                return null;

            return method.Name.Substring(0, length);
        }

        protected MvxCommandAttribute CommandAttribute(MethodInfo method)
        {
            return (MvxCommandAttribute)method.GetCustomAttributes(typeof(MvxCommandAttribute), true).FirstOrDefault();
        }

        protected virtual string CanExecuteProperyName(MethodInfo method)
        {
            var commandAttribute = CommandAttribute(method);
            if (commandAttribute != null)
                return commandAttribute.CanExecutePropertyName;

            return CanExecutePrefix + method.Name;
        }

        #region Nested classes for building commands by reflection - 'hidden as nested' currently as they are not used anywhere else

        public interface IMvxCommandBuilder
        {
            IMvxCommand ToCommand(object owner);

            string CanExecutePropertyName { get; }
        }

        public abstract class MvxBaseCommandBuilder : IMvxCommandBuilder
        {
            private readonly MethodInfo _executeMethodInfo;
            private readonly PropertyInfo _canExecutePropertyInfo;

            protected MethodInfo ExecuteMethodInfo => _executeMethodInfo;

            protected PropertyInfo CanExecutePropertyInfo => _canExecutePropertyInfo;

            protected MvxBaseCommandBuilder(MethodInfo executeMethodInfo, PropertyInfo canExecutePropertyInfo)
            {
                _executeMethodInfo = executeMethodInfo;
                _canExecutePropertyInfo = canExecutePropertyInfo;
            }

            public abstract IMvxCommand ToCommand(object owner);

            public string CanExecutePropertyName => _canExecutePropertyInfo?.Name;
        }

        public class MvxCommandBuilder : MvxBaseCommandBuilder
        {
            public MvxCommandBuilder(MethodInfo executeMethodInfo, PropertyInfo canExecutePropertyInfo)
                : base(executeMethodInfo, canExecutePropertyInfo)
            {
            }

            public override IMvxCommand ToCommand(object owner)
            {
                var executeAction = new Action(() => ExecuteMethodInfo.Invoke(owner, new object[0]));
                Func<bool> canExecuteFunc = null;
                if (CanExecutePropertyInfo != null)
                    canExecuteFunc = () => (bool)CanExecutePropertyInfo.GetValue(owner, null);

                return new MvxCommand(executeAction, canExecuteFunc);
            }
        }

        public class MvxParameterizedCommandBuilder : MvxBaseCommandBuilder
        {
            public MvxParameterizedCommandBuilder(MethodInfo executeMethodInfo, PropertyInfo canExecutePropertyInfo)
                : base(executeMethodInfo, canExecutePropertyInfo)
            {
            }

            public override IMvxCommand ToCommand(object owner)
            {
                var executeAction = new Action<object>((obj) => ExecuteMethodInfo.Invoke(owner, new[] { obj }));
                Func<object, bool> canExecuteFunc = null;
                if (CanExecutePropertyInfo != null)
                    canExecuteFunc = (ignored) => (bool)CanExecutePropertyInfo.GetValue(owner, null);

                return new MvxCommand<object>(executeAction, canExecuteFunc);
            }
        }

        /*
         * the <T> version is not used because of MonoTouch AoT compilation challenges
        public class MvxParameterizedCommandBuilder<T> : MvxBaseCommandBuilder
        {
            public MvxParameterizedCommandBuilder(MethodInfo executeMethodInfo, PropertyInfo canExecutePropertyInfo)
                : base(executeMethodInfo, canExecutePropertyInfo)
            {
            }

            public override IMvxCommand ToCommand(object owner)
            {
                var executeAction = new Action<T>((obj) => ExecuteMethodInfo.Invoke(owner, new object[] { obj }));
                Func<T, bool> canExecuteFunc = null;
                if (CanExecutePropertyInfo != null)
                    canExecuteFunc = new Func<T, bool>((ignored) => (bool)CanExecutePropertyInfo.GetValue(owner, null));

                return new MvxCommand<T>(executeAction, canExecuteFunc);
            }
        }
        */

        #endregion Nested classes for building commands by reflection - 'hidden as nested' currently as they are not used anywhere else
    }
}