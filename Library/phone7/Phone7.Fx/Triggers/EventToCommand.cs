using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Phone7.Fx.Triggers
{
    //public class EventToCommand : TriggerAction<FrameworkElement>
    //{

    //    public bool EventArgsToCommandParameter
    //    {
    //        get;
    //        set;
    //    }


    //    public void Invoke()
    //    {
    //        Invoke(null);
    //    }


    //    protected override void Invoke(object parameter)
    //    {
    //        if (AssociatedElementIsDisabled())
    //        {
    //            return;
    //        }

    //        var command = GetCommand();
    //        var commandParameter = CommandParameterValue;

    //        if (commandParameter == null
    //            && EventArgsToCommandParameter && command != null
    //            && command.CanExecute(commandParameter))
    //        {
    //            //commandParameter = parameter;
    //            command.Execute(parameter);
    //            return;
    //        }

    //        if (command != null
    //            && command.CanExecute(commandParameter))
    //        {
    //            command.Execute(commandParameter);
    //        }
    //    }

    //    private static void OnCommandChanged(EventToCommand element, DependencyPropertyChangedEventArgs e)
    //    {
    //        if (element == null)
    //        {
    //            return;
    //        }

    //        if (e.OldValue != null)
    //        {
    //            ((ICommand)e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
    //        }

    //        var command = (ICommand)e.NewValue;

    //        if (command != null)
    //        {
    //            command.CanExecuteChanged += element.OnCommandCanExecuteChanged;
    //        }

    //        element.EnableDisableElement();
    //    }

    //    private bool AssociatedElementIsDisabled()
    //    {
    //        var element = GetAssociatedObject();

    //        return element != null
    //               && !element.IsEnabled;
    //    }

    //    private void EnableDisableElement()
    //    {
    //        var element = GetAssociatedObject();

    //        if (element == null)
    //        {
    //            return;
    //        }

    //        var command = this.GetCommand();

    //        if (this.MustToggleIsEnabledValue
    //            && command != null)
    //        {
    //            element.IsEnabled = command.CanExecute(this.CommandParameterValue);
    //        }
    //    }

    //    private void OnCommandCanExecuteChanged(object sender, EventArgs e)
    //    {
    //        EnableDisableElement();
    //    }


    //    /// Identifies the <see cref="CommandParameter" /> dependency property
    //    /// </summary>
    //    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
    //        "CommandParameter",
    //        typeof(Binding),
    //        typeof(EventToCommand),
    //        new PropertyMetadata(
    //            null,
    //            (s, e) =>
    //            {
    //                var sender = s as EventToCommand;
    //                if (sender != null)
    //                {
    //                    sender._listenerParameter.Binding = e.NewValue as Binding;
    //                }
    //            }));

    //    /// <summary>
    //    /// Identifies the <see cref="Command" /> dependency property
    //    /// </summary>
    //    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
    //        "Command",
    //        typeof(Binding),
    //        typeof(EventToCommand),
    //        new PropertyMetadata(
    //            null,
    //            (s, e) =>
    //            {
    //                var sender = s as EventToCommand;
    //                if (sender != null)
    //                {
    //                    sender._listenerCommand.Binding = e.NewValue as Binding;
    //                }
    //            }));

    //    /// <summary>
    //    /// Identifies the <see cref="MustToggleIsEnabled" /> dependency property
    //    /// </summary>
    //    public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(
    //        "MustToggleIsEnabled",
    //        typeof(Binding),
    //        typeof(EventToCommand),
    //        new PropertyMetadata(
    //            null,
    //            (s, e) =>
    //            {
    //                var sender = s as EventToCommand;
    //                if (sender != null)
    //                {
    //                    sender._listenerToggle.Binding = e.NewValue as Binding;
    //                }
    //            }));

    //    private readonly BindingListener _listenerCommand;

    //    private readonly BindingListener _listenerParameter;

    //    private readonly BindingListener _listenerToggle;

    //    private object _commandParameterValue;

    //    private bool? _mustToggleValue;

    //    /// <summary>
    //    /// Initializes a new instance of the EventToCommand class.
    //    /// </summary>
    //    public EventToCommand()
    //    {
    //        _listenerCommand = new BindingListener(
    //            this,
    //            (s, e) =>
    //            {
    //                var sender = s as BindingListener;
    //                if (sender != null)
    //                {
    //                    OnCommandChanged(
    //                        sender.Context as EventToCommand,
    //                        e.EventArgs);
    //                }
    //            });

    //        _listenerParameter = new BindingListener(
    //            this,
    //            (s, e) => CheckEnableDisable(s as BindingListener));

    //        _listenerToggle = new BindingListener(
    //            this,
    //            (s, e) => CheckEnableDisable(s as BindingListener));
    //    }

    //    /// <summary>
    //    /// Gets or sets the ICommand that this trigger is bound to. This
    //    /// is a DependencyProperty.
    //    /// </summary>
    //    public Binding Command
    //    {
    //        get
    //        {
    //            return (Binding)GetValue(CommandProperty);
    //        }

    //        set
    //        {
    //            SetValue(CommandProperty, value);
    //        }
    //    }

    //    /// <summary>
    //    /// Gets or sets an object that will be passed to the <see cref="Command" />
    //    /// attached to this trigger. This is a DependencyProperty. Because of limitations
    //    /// of Silverlight, you can only set databindings on this property. If you
    //    /// wish to use hard coded values, use the <see cref="CommandParameterValue" />
    //    /// property.
    //    /// </summary>
    //    public Binding CommandParameter
    //    {
    //        get
    //        {
    //            return (Binding)this.GetValue(CommandParameterProperty);
    //        }

    //        set
    //        {
    //            this.SetValue(CommandParameterProperty, value);
    //        }
    //    }

    //    /// <summary>
    //    /// Gets or sets an object that will be passed to the <see cref="Command" />
    //    /// attached to this trigger. This is NOT a DependencyProperty. Use this
    //    /// property if you want to set a hard coded value.
    //    /// For databinding, use the <see cref="CommandParameter" /> property.
    //    /// </summary>
    //    public object CommandParameterValue
    //    {
    //        get
    //        {
    //            if (_commandParameterValue != null)
    //            {
    //                return _commandParameterValue;
    //            }

    //            return this._listenerParameter.Value;
    //        }

    //        set
    //        {
    //            if (_listenerParameter.Value != null)
    //            {
    //                throw new InvalidOperationException(
    //                    "Cannot set CommandParameterValue when CommandParameter is already set");
    //            }

    //            _commandParameterValue = value;
    //            EnableDisableElement();
    //        }
    //    }

    //    /// <summary>
    //    /// Gets or sets a value indicating whether the attached element must be
    //    /// disabled when the <see cref="Command" /> property's CanExecuteChanged
    //    /// event fires. If this property is true, and the command's CanExecute 
    //    /// method returns false, the element will be disabled. If this property
    //    /// is false, the element will not be disabled when the command's
    //    /// CanExecute method changes.
    //    /// If the attached element is not a <see cref="Control" />, this property
    //    /// has no effect. 
    //    /// This is a DependencyProperty.
    //    /// Because of limitations of Silverlight, you can only set databindings 
    //    /// on this property. If you wish to use hard coded values, use 
    //    /// the <see cref="CommandParameterValue" /> property.
    //    /// </summary>
    //    public Binding MustToggleIsEnabled
    //    {
    //        get
    //        {
    //            return (Binding)this.GetValue(MustToggleIsEnabledProperty);
    //        }

    //        set
    //        {
    //            this.SetValue(MustToggleIsEnabledProperty, value);
    //        }
    //    }

    //    /// <summary>
    //    /// Gets or sets a value indicating whether the attached element must be
    //    /// disabled when the <see cref="Command" /> property's CanExecuteChanged
    //    /// event fires. If this property is true, and the command's CanExecute 
    //    /// method returns false, the element will be disabled.
    //    /// If the attached element is not a <see cref="Control" />, this property
    //    /// has no effect. 
    //    /// This property is here for compatibility with the Silverlight version. 
    //    /// This is NOT a DependencyProperty.
    //    /// Use this property if you want to set a hard coded value.
    //    /// For databinding, use the <see cref="MustToggleIsEnabled" /> property.
    //    /// </summary>
    //    public bool MustToggleIsEnabledValue
    //    {
    //        get
    //        {
    //            if (_mustToggleValue != null)
    //            {
    //                return _mustToggleValue.Value;
    //            }

    //            if (_listenerToggle.Value != null)
    //            {
    //                return (bool)_listenerToggle.Value;
    //            }

    //            return false;
    //        }

    //        set
    //        {
    //            if (_listenerToggle.Value != null)
    //            {
    //                throw new InvalidOperationException(
    //                    "Cannot set MustToggleIsEnabledValue when MustToggleIsEnabled is already set");
    //            }

    //            _mustToggleValue = value;
    //            EnableDisableElement();
    //        }
    //    }

    //    /// <summary>
    //    /// Called when this trigger is attached to a FrameworkElement.
    //    /// </summary>
    //    protected override void OnAttached()
    //    {
    //        base.OnAttached();

    //        _listenerCommand.Element = this.AssociatedObject;
    //        _listenerParameter.Element = this.AssociatedObject;
    //        _listenerToggle.Element = this.AssociatedObject;

    //        EnableDisableElement();
    //    }

    //    /// <summary>
    //    /// Called when this trigger is detached from its associated object.
    //    /// </summary>
    //    protected override void OnDetaching()
    //    {
    //        base.OnDetaching();

    //        _listenerCommand.Element = null;
    //        _listenerParameter.Element = null;
    //        _listenerToggle.Element = null;
    //    }

    //    private static void CheckEnableDisable(BindingListener listener)
    //    {
    //        if (listener == null)
    //        {
    //            return;
    //        }

    //        var thisObject = listener.Context as EventToCommand;
    //        if (thisObject != null)
    //        {
    //            thisObject.EnableDisableElement();
    //        }
    //    }

    //    private Control GetAssociatedObject()
    //    {
    //        return AssociatedObject as Control;
    //    }

    //    private ICommand GetCommand()
    //    {
    //        return _listenerCommand.Value as ICommand;
    //    }

    //}

    public class EventToCommand : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// Identifies the <see cref="CommandParameter" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(EventToCommand),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var sender = s as EventToCommand;
                    if (sender == null)
                    {
                        return;
                    }

                    if (sender.AssociatedObject == null)
                    {
                        return;
                    }

                    sender.EnableDisableElement();
                }));

        /// <summary>
        /// Identifies the <see cref="Command" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(EventToCommand),
            new PropertyMetadata(
                null,
                (s, e) => OnCommandChanged(s as EventToCommand, e)));

        /// <summary>
        /// Identifies the <see cref="MustToggleIsEnabled" /> dependency property
        /// </summary>
        public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(
            "MustToggleIsEnabled",
            typeof(bool),
            typeof(EventToCommand),
            new PropertyMetadata(
                false,
                (s, e) =>
                {
                    var sender = s as EventToCommand;
                    if (sender == null)
                    {
                        return;
                    }

                    if (sender.AssociatedObject == null)
                    {
                        return;
                    }

                    sender.EnableDisableElement();
                }));

        private object _commandParameterValue;

        private bool? _mustToggleValue;

        /// <summary>
        /// Gets or sets the ICommand that this trigger is bound to. This
        /// is a DependencyProperty.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }

            set
            {
                SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This is a DependencyProperty.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }

            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This property is here for compatibility
        /// with the Silverlight version. This is NOT a DependencyProperty.
        /// For databinding, use the <see cref="CommandParameter" /> property.
        /// </summary>
        public object CommandParameterValue
        {
            get
            {
                return this._commandParameterValue ?? this.CommandParameter;
            }

            set
            {
                _commandParameterValue = value;
                EnableDisableElement();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute 
        /// method returns false, the element will be disabled. If this property
        /// is false, the element will not be disabled when the command's
        /// CanExecute method changes. This is a DependencyProperty.
        /// </summary>
        public bool MustToggleIsEnabled
        {
            get
            {
                return (bool)this.GetValue(MustToggleIsEnabledProperty);
            }

            set
            {
                SetValue(MustToggleIsEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute 
        /// method returns false, the element will be disabled. This property is here for
        /// compatibility with the Silverlight version. This is NOT a DependencyProperty.
        /// For databinding, use the <see cref="MustToggleIsEnabled" /> property.
        /// </summary>
        public bool MustToggleIsEnabledValue
        {
            get
            {
                return this._mustToggleValue == null
                           ? this.MustToggleIsEnabled
                           : this._mustToggleValue.Value;
            }

            set
            {
                _mustToggleValue = value;
                EnableDisableElement();
            }
        }

        /// <summary>
        /// Called when this trigger is attached to a FrameworkElement.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            EnableDisableElement();
        }


        private Control GetAssociatedObject()
        {
            return AssociatedObject as Control;
        }


        /// <summary>
        /// This method is here for compatibility
        /// with the Silverlight 3 version.
        /// </summary>
        /// <returns>The command that must be executed when
        /// this trigger is invoked.</returns>
        private ICommand GetCommand()
        {
            return Command;
        }

        /// <summary>
        /// Specifies whether the EventArgs of the event that triggered this
        /// action should be passed to the bound RelayCommand. If this is true,
        /// the command should accept arguments of the corresponding
        /// type (for example RelayCommand&lt;MouseButtonEventArgs&gt;).
        /// </summary>
        public bool PassEventArgsToCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Provides a simple way to invoke this trigger programatically
        /// without any EventArgs.
        /// </summary>
        public void Invoke()
        {
            Invoke(null);
        }

        /// <summary>
        /// Executes the trigger.
        /// <para>To access the EventArgs of the fired event, use a RelayCommand&lt;EventArgs&gt;
        /// and leave the CommandParameter and CommandParameterValue empty!</para>
        /// </summary>
        /// <param name="parameter">The EventArgs of the fired event.</param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedElementIsDisabled())
            {
                return;
            }

            var command = GetCommand();
            var commandParameter = CommandParameterValue;

            if (commandParameter == null
                && PassEventArgsToCommand)
            {
                commandParameter = parameter;
            }

            if (command != null
                && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }

        private static void OnCommandChanged(
            EventToCommand element,
            DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }

            if (e.OldValue != null)
            {
                ((ICommand)e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
            }

            var command = (ICommand)e.NewValue;

            if (command != null)
            {
                command.CanExecuteChanged += element.OnCommandCanExecuteChanged;
            }

            element.EnableDisableElement();
        }

        private bool AssociatedElementIsDisabled()
        {
            var element = GetAssociatedObject();

            return AssociatedObject == null
                || (element != null
                   && !element.IsEnabled);
        }

        private void EnableDisableElement()
        {
            var element = GetAssociatedObject();

            if (element == null)
            {
                return;
            }

            var command = this.GetCommand();

            if (this.MustToggleIsEnabledValue
                && command != null)
            {
                element.IsEnabled = command.CanExecute(this.CommandParameterValue);
            }
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            EnableDisableElement();
        }
    }
}