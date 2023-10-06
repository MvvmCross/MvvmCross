// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Commands
{
#nullable enable
    public class MvxCommandCollection
        : IMvxCommandCollection
    {
        private readonly object _owner;
        private readonly Dictionary<string, IMvxCommand> _commandLookup = new Dictionary<string, IMvxCommand>();
        private readonly Dictionary<string, List<IMvxCommand>> _canExecuteLookup = new Dictionary<string, List<IMvxCommand>>();

        public MvxCommandCollection(object owner)
        {
            _owner = owner;
            SubscribeToNotifyPropertyChanged();
        }

        private void SubscribeToNotifyPropertyChanged()
        {
            var inpc = _owner as INotifyPropertyChanged;
            if (inpc == null)
                return;

            inpc.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // if args.PropertyName is empty then it means all properties have changed.
            if (string.IsNullOrEmpty(args.PropertyName))
            {
                RaiseAllCanExecuteChanged();
                return;
            }

            if (_canExecuteLookup.TryGetValue(args.PropertyName, out List<IMvxCommand>? commands))
            {
                foreach (var command in commands)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }

        private void RaiseAllCanExecuteChanged()
        {
            foreach (var command in _commandLookup)
            {
                command.Value.RaiseCanExecuteChanged();
            }
        }

        public IMvxCommand? this[string name]
        {
            get
            {
                if (_commandLookup.Count == 0)
                {
                    MvxLogHost.Default?.Log(LogLevel.Trace, "MvxCommandCollection is empty - did you forget to add your commands?");
                    return null;
                }

                IMvxCommand? toReturn;
                _commandLookup.TryGetValue(name, out toReturn);
                return toReturn;
            }
        }

        public void Add(IMvxCommand command, string name, string? canExecuteName)
        {
            AddToLookup(_commandLookup, command, name);
            AddToLookup(_canExecuteLookup, command, canExecuteName);
        }

        private static void AddToLookup(IDictionary<string, IMvxCommand> lookup, IMvxCommand command, string? name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (lookup.ContainsKey(name!))
            {
                MvxLogHost.Default?.Log(LogLevel.Warning, "Ignoring Commmand - it would overwrite the existing Command, name {name}", name);
                return;
            }
            lookup[name!] = command;
        }

        private static void AddToLookup(IDictionary<string, List<IMvxCommand>> lookup, IMvxCommand command, string? name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            // If no collection exists, create a new one
            if (!lookup.TryGetValue(name!, out var commands))
            {
                commands = new List<IMvxCommand>();
                lookup[name!] = commands;
            }

            // Protect against adding command twice
            if (!commands.Contains(command))
            {
                commands.Add(command);
            }
        }
    }
#nullable restore
}
