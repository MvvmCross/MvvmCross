﻿// MvxCommandCollection.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxCommandCollection
    {
        private const string CommandSuffix = "Command";
        private const string CanExecutePrefix = "CanExecute";

        private readonly Dictionary<string, IMvxCommand> _commandLookup = new Dictionary<string, IMvxCommand>();
        private readonly Dictionary<string, IMvxCommand> _canExecuteLookup = new Dictionary<string, IMvxCommand>();

        private bool _initialised;

        private readonly object _owner;
        protected object Owner
        {
            get { return _owner; }
        }

        public MvxCommandCollection(object owner)
        {
            _owner = owner;
        }

        public MvxCommandCollection Initialize()
        {
            if (_initialised)
            {
                MvxTrace.Warning("Mulitple calls to Initialize are ignored");
                return this;
            }

            _initialised = true;
            DoInitialization();
            return this;
        }

        protected virtual void DoInitialization()
        {
            CreateCommands();
            SubscribeToNotifyPropertyChanged();
        }

        protected virtual void CreateCommands()
        {
            var commandMethods =
                from method in
                    _owner.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                where method.ReturnType == typeof (void)
                let parameterCount = method.GetParameters().Count()
                where parameterCount <= 1
                let commandName = GetCommandNameOrNull(method)
                where !string.IsNullOrEmpty(commandName)
                select new {Method = method, CommandName = commandName, HasParameter = parameterCount > 0};

            foreach (var item in commandMethods)
            {
                CreateCommandFor(item.Method, item.CommandName, item.HasParameter);
            }
        }

        public IMvxCommand this[string name]
        {
            get
            {
                if (!_initialised)
                {
                    MvxTrace.Warning("MvxCommandCollection used before Initialize() is called");
                    return null;
                }

                IMvxCommand toReturn;
                _commandLookup.TryGetValue(name, out toReturn);
                return toReturn;
            }
        }

        protected virtual void SubscribeToNotifyPropertyChanged()
        {
            var inpc = _owner as INotifyPropertyChanged;
            if (inpc == null)
                return;

            inpc.PropertyChanged += OnPropertyChanged;
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            IMvxCommand command;
            if (!_canExecuteLookup.TryGetValue(args.PropertyName, out command))
                return;

            command.RaiseCanExecuteChanged();
        }

        protected virtual void CreateCommandFor(MethodInfo commandMethod, string commandName, bool hasParameter)
        {
            var canExecuteProperty = CanExecutePropertyInfo(_owner.GetType(), commandMethod);

            var helper = hasParameter
                                    ? (IMvxCommandBuilder)new MvxParameterizedCommandBuilder(commandMethod, canExecuteProperty)
                                    : new MvxCommandBuilder(commandMethod, canExecuteProperty);

            var command = helper.ToCommand(_owner);
            if (!string.IsNullOrEmpty(helper.CanExecutePropertyName))
            {
                _canExecuteLookup[helper.CanExecutePropertyName] = command;
            }
            _commandLookup[commandName] = command;
        }

        private PropertyInfo CanExecutePropertyInfo(Type type, MethodInfo commandMethod)
        {
            var canExecuteName = CanExecuteProperyName(commandMethod);
            if (string.IsNullOrEmpty(canExecuteName))
                return null;
            var canExecute = type.GetProperty(canExecuteName, BindingFlags.Instance | BindingFlags.Public);
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

            if (!method.Name.EndsWith(CommandSuffix))
                return null;

            var length = method.Name.Length - CommandSuffix.Length;
            if (length <= 0)
                return null;

            return method.Name.Substring(0, length);
        }

        protected MvxCommandAttribute CommandAttribute(MethodInfo method)
        {
            return (MvxCommandAttribute)method.GetCustomAttributes(typeof (MvxCommandAttribute), true).FirstOrDefault();
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

            protected MethodInfo ExecuteMethodInfo
            {
                get { return _executeMethodInfo; }
            }

            protected PropertyInfo CanExecutePropertyInfo
            {
                get { return _canExecutePropertyInfo; }
            }

            protected MvxBaseCommandBuilder(MethodInfo executeMethodInfo, PropertyInfo canExecutePropertyInfo)
            {
                _executeMethodInfo = executeMethodInfo;
                _canExecutePropertyInfo = canExecutePropertyInfo;
            }

            public abstract IMvxCommand ToCommand(object owner);

            public string CanExecutePropertyName
            {
                get { return _canExecutePropertyInfo == null ? null : _canExecutePropertyInfo.Name; }
            }
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
                    canExecuteFunc = new Func<bool>(() => (bool)CanExecutePropertyInfo.GetValue(owner, null));

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
                var executeAction = new Action<object>((obj) => ExecuteMethodInfo.Invoke(owner, new object[] { obj }));
                Func<object, bool> canExecuteFunc = null;
                if (CanExecutePropertyInfo != null)
                    canExecuteFunc = new Func<object, bool>((ignored) => (bool)CanExecutePropertyInfo.GetValue(owner, null));

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

        #endregion
    }
}