﻿// MvxEventToCommand.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region Credit - MvvmLight

// This file is based on the EventToCommand mechanism from MVVMLight - used under MIT license -see http://mvvmlight.codeplex.com

#endregion Credit - MvvmLight

using System;
using System.Windows;
using System.Windows.Input;

namespace MvvmCross.Wpf.Commands
{
    public class MvxEventToCommand : TriggerAction<DependencyObject>
    {
        /// <summary>
        ///     Identifies the <see cref="CommandParameter" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(MvxEventToCommand),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var sender = s as MvxEventToCommand;

                    if (sender?.AssociatedObject == null)
                        return;

                    sender.EnableDisableElement();
                }));

        /// <summary>
        ///     Identifies the <see cref="Command" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(MvxEventToCommand),
            new PropertyMetadata(
                null,
                (s, e) => OnCommandChanged(s as MvxEventToCommand, e)));

        /// <summary>
        ///     Identifies the <see cref="MustToggleIsEnabled" /> dependency property
        /// </summary>
        public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(
            "MustToggleIsEnabled",
            typeof(bool),
            typeof(MvxEventToCommand),
            new PropertyMetadata(
                false,
                (s, e) =>
                {
                    var sender = s as MvxEventToCommand;

                    if (sender?.AssociatedObject == null)
                        return;

                    sender.EnableDisableElement();
                }));

        private object _commandParameterValue;

        private bool? _mustToggleValue;

        /// <summary>
        ///     Gets or sets the ICommand that this trigger is bound to. This
        ///     is a DependencyProperty.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);

            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        ///     Gets or sets an object that will be passed to the <see cref="Command" />
        ///     attached to this trigger. This is a DependencyProperty.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);

            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        ///     Gets or sets an object that will be passed to the <see cref="Command" />
        ///     attached to this trigger. This property is here for compatibility
        ///     with the Silverlight version. This is NOT a DependencyProperty.
        ///     For databinding, use the <see cref="CommandParameter" /> property.
        /// </summary>
        public object CommandParameterValue
        {
            get => _commandParameterValue ?? CommandParameter;

            set
            {
                _commandParameterValue = value;
                EnableDisableElement();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the attached element must be
        ///     disabled when the <see cref="Command" /> property's CanExecuteChanged
        ///     event fires. If this property is true, and the command's CanExecute
        ///     method returns false, the element will be disabled. If this property
        ///     is false, the element will not be disabled when the command's
        ///     CanExecute method changes. This is a DependencyProperty.
        /// </summary>
        public bool MustToggleIsEnabled
        {
            get => (bool) GetValue(MustToggleIsEnabledProperty);

            set => SetValue(MustToggleIsEnabledProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the attached element must be
        ///     disabled when the <see cref="Command" /> property's CanExecuteChanged
        ///     event fires. If this property is true, and the command's CanExecute
        ///     method returns false, the element will be disabled. This property is here for
        ///     compatibility with the Silverlight version. This is NOT a DependencyProperty.
        ///     For databinding, use the <see cref="MustToggleIsEnabled" /> property.
        /// </summary>
        public bool MustToggleIsEnabledValue
        {
            get => _mustToggleValue ?? MustToggleIsEnabled;

            set
            {
                _mustToggleValue = value;
                EnableDisableElement();
            }
        }

        /// <summary>
        ///     Called when this trigger is attached to a FrameworkElement.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            EnableDisableElement();
        }

#if SILVERLIGHT
        private Control GetAssociatedObject()
        {
            return AssociatedObject as Control;
        }
#else

        /// <summary>
        ///     This method is here for compatibility
        ///     with the Silverlight version.
        /// </summary>
        /// <returns>
        ///     The FrameworkElement to which this trigger
        ///     is attached.
        /// </returns>
        private FrameworkElement GetAssociatedObject()
        {
            return this.AssociatedObject as FrameworkElement;
        }

#endif

        /// <summary>
        ///     This method is here for compatibility
        ///     with the Silverlight 3 version.
        /// </summary>
        /// <returns>
        ///     The command that must be executed when
        ///     this trigger is invoked.
        /// </returns>
        private ICommand GetCommand()
        {
            return Command;
        }

        /// <summary>
        ///     Specifies whether the EventArgs of the event that triggered this
        ///     action should be passed to the bound RelayCommand. If this is true,
        ///     the command should accept arguments of the corresponding
        ///     type (for example RelayCommand&lt;MouseButtonEventArgs&gt;).
        /// </summary>
        public bool PassEventArgsToCommand { get; set; }

        /// <summary>
        ///     Provides a simple way to invoke this trigger programatically
        ///     without any EventArgs.
        /// </summary>
        public void Invoke()
        {
            Invoke(null);
        }

        /// <summary>
        ///     Executes the trigger.
        ///     <para>
        ///         To access the EventArgs of the fired event, use a RelayCommand&lt;EventArgs&gt;
        ///         and leave the CommandParameter and CommandParameterValue empty!
        ///     </para>
        /// </summary>
        /// <param name="parameter">The EventArgs of the fired event.</param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedElementIsDisabled())
                return;

            var command = GetCommand();
            var commandParameter = CommandParameterValue;

            if (commandParameter == null
                && PassEventArgsToCommand)
                commandParameter = MapCommandParameter(parameter);

            if (command != null
                && command.CanExecute(commandParameter))
                command.Execute(commandParameter);
        }

        protected virtual object MapCommandParameter(object parameter)
        {
            return parameter;
        }

        private static void OnCommandChanged(
            MvxEventToCommand element,
            DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
                return;

            if (e.OldValue != null)
                ((ICommand) e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;

            var command = (ICommand) e.NewValue;

            if (command != null)
                command.CanExecuteChanged += element.OnCommandCanExecuteChanged;

            element.EnableDisableElement();
        }

        private bool AssociatedElementIsDisabled()
        {
            var element = GetAssociatedObject();

            return this.AssociatedObject == null
                   || element != null
                   && !element.IsEnabled;
        }

        private void EnableDisableElement()
        {
            var element = GetAssociatedObject();

            if (element == null)
                return;

            var command = GetCommand();

            if (MustToggleIsEnabledValue
                && command != null)
                element.IsEnabled = command.CanExecute(CommandParameterValue);
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            EnableDisableElement();
        }
    }
}