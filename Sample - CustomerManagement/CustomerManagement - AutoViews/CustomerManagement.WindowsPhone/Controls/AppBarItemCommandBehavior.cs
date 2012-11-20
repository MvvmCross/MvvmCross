using System;
using System.Windows.Input;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone.Controls
{
	/// <summary>
	/// Behavior to handle connecting a <see cref="IApplicationBarMenuItem"/> to a Command.
	/// </summary>
	/// <typeparam name="T">The target object must derive from IApplicationBarMenuItem</typeparam>
	public class AppBarItemCommandBehavior<T>
			where T : IApplicationBarMenuItem
	{
		private ICommand command;
		private object commandParameter;
		private readonly WeakReference targetObject;
		private readonly EventHandler commandCanExecuteChangedHandler;

		/// <summary>
		/// Constructor specifying the target object.
		/// </summary>
		/// <param name="targetObject">The target object the behavior is attached to.</param>
		public AppBarItemCommandBehavior(T targetObject)
		{
			this.targetObject = new WeakReference(targetObject);
			commandCanExecuteChangedHandler = new EventHandler(this.CommandCanExecuteChanged);
			((T)this.targetObject.Target).Click += (s, e) => ExecuteCommand();
		}

		/// <summary>
		/// Corresponding command to be execute and monitored for <see cref="ICommand.CanExecuteChanged"/>
		/// </summary>
		public ICommand Command
		{
			get
			{
				return command;
			}
			set
			{
				if (this.command != null)
					this.command.CanExecuteChanged -= this.commandCanExecuteChangedHandler;

				this.command = value;
				if (this.command != null)
				{
					this.command.CanExecuteChanged += this.commandCanExecuteChangedHandler;
					UpdateEnabledState();
				}
			}
		}

		/// <summary>
		/// The parameter to supply the command during execution
		/// </summary>
		public object CommandParameter
		{
			get
			{
				return this.commandParameter;
			}
			set
			{
				if (this.commandParameter != value)
				{
					this.commandParameter = value;
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
				return (T)targetObject.Target;
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
				TargetObject.IsEnabled = this.Command.CanExecute(this.CommandParameter);
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
				this.Command.Execute(this.CommandParameter);
		}
	}
}