using System;
using System.Windows.Controls;
using System.Windows.Input;


namespace Phone7.Fx.Commands
{
    /// <summary>
    /// Base behavior to handle connecting a <see cref="Control"/> to a Command.
    /// </summary>
    /// <typeparam name="T">The target object must derive from Control</typeparam>
    /// <remarks>
    /// </remarks>
    public class CommandBehaviorBase<T>
        where T : System.Windows.Controls.Control
    {
        private ICommand _command;
        private object _commandParameter;
        private readonly WeakReference _targetObject;
        private readonly EventHandler _commandCanExecuteChangedHandler;


        /// <summary>
        /// Constructor specifying the target object.
        /// </summary>
        /// <param name="targetObject">The target object the behavior is attached to.</param>
        public CommandBehaviorBase(T targetObject)
        {
            this._targetObject = new WeakReference(targetObject);
            this._commandCanExecuteChangedHandler = new EventHandler(this.CommandCanExecuteChanged);
        }

        /// <summary>
        /// Corresponding command to be execute and monitored for <see cref="ICommand.CanExecuteChanged"/>
        /// </summary>
        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (this._command != null)
                {
                    this._command.CanExecuteChanged -= this._commandCanExecuteChangedHandler;
                }

                this._command = value;
                if (this._command != null)
                {
                    this._command.CanExecuteChanged += this._commandCanExecuteChangedHandler;
                    UpdateEnabledState();
                }
            }
        }

        /// <summary>
        /// The parameter to supply the command during execution
        /// </summary>
        public object CommandParameter
        {
            get { return this._commandParameter; }
            set
            {
                if (this._commandParameter != value)
                {
                    this._commandParameter = value;
                    this.UpdateEnabledState();
                }
            }
        }

        /// <summary>
        /// Object to which this behavior is attached.
        /// </summary>
        protected T TargetObject
        {
            get
            {
                return _targetObject.Target as T;
            }
        }


        /// <summary>
        /// Updates the target object's IsEnabled property based on the commands ability to execute.
        /// </summary>
        protected virtual void UpdateEnabledState()
        {
            if (TargetObject == null)
            {
                this.Command = null;
                this.CommandParameter = null;
            }
            else if (this.Command != null)
            {
                TargetObject.IsEnabled = this.Command.CanExecute(this.CommandParameter);
            }
        }

        private void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledState();
        }

        /// <summary>
        /// Executes the command, if it's set, providing the <see cref="CommandParameter"/>
        /// </summary>
        protected virtual void ExecuteCommand()
        {
            if (this.Command != null)
            {
                this.Command.Execute(this.CommandParameter);
            }
        }
    }
}