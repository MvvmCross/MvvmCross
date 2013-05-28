// MvxCommandCollection.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxCommandCollection
        : IMvxCommandCollection
    {
        private readonly object _owner;
        private readonly Dictionary<string, IMvxCommand> _commandLookup = new Dictionary<string, IMvxCommand>();
        private readonly Dictionary<string, IMvxCommand> _canExecuteLookup = new Dictionary<string, IMvxCommand>();

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

            IMvxCommand command;
            if (!_canExecuteLookup.TryGetValue(args.PropertyName, out command))
                return;

            command.RaiseCanExecuteChanged();
        }

        private void RaiseAllCanExecuteChanged()
        {
            foreach (var command in _commandLookup)
            {
                command.Value.RaiseCanExecuteChanged();
            }
        }

        public IMvxCommand this[string name]
        {
            get
            {
                if (!_commandLookup.Any())
                {
                    MvxTrace.Trace("MvxCommandCollection is empty - did you forget to add your commands?");
                    return null;
                }

                IMvxCommand toReturn;
                _commandLookup.TryGetValue(name, out toReturn);
                return toReturn;
            }
        }

        public void Add(IMvxCommand command, string name, string canExecuteName)
        {
            AddToLookup(_commandLookup, command, name);
            AddToLookup(_canExecuteLookup, command, canExecuteName);
        }

        private static void AddToLookup(IDictionary<string, IMvxCommand> lookup, IMvxCommand command, string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (lookup.ContainsKey(name))
            {
                MvxTrace.Warning("Ignoring Commmand - it would overwrite the existing Command, name {0}", name);
                return;
            }
            lookup[name] = command;
        }
    }
}