using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone.Controls
{
	/// <summary>
	/// Static Class that holds all Dependency Properties and Static methods to allow 
	/// the Click event of the IApplicationBarMenuItem interface to be attached to a Command. 
	/// </summary>
	/// <remarks>
	/// This class is required, because Silverlight for WinPhone doesn't have native support for Commands. 
	/// </remarks>
	public static class AppBarItemClick
	{
		private static readonly DependencyProperty ClickCommandBehaviorProperty = DependencyProperty.RegisterAttached(
				"ClickCommandBehavior",
				typeof(AppBarItemCommandBehavior<IApplicationBarMenuItem>),
				typeof(AppBarItemClick),
				null);


		/// <summary>
		/// Command to execute on click event.
		/// </summary>
		public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
				"Command",
				typeof(ICommand),
				typeof(AppBarItemClick),
				new PropertyMetadata(OnSetCommandCallback));

		/// <summary>
		/// Command parameter to supply on command execution.
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
				"CommandParameter",
				typeof(object),
				typeof(AppBarItemClick),
				new PropertyMetadata(OnSetCommandParameterCallback));


		/// <summary>
		/// Sets the <see cref="ICommand"/> to execute on the click event.
		/// </summary>
		/// <param name="item">AppBarItem dependency object to attach command</param>
		/// <param name="command">Command to attach</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for IApplicationBarMenuItem")]
		public static void SetCommand(IApplicationBarMenuItem item, ICommand command)
		{
			(item as FrameworkElement).SetValue(CommandProperty, command);
		}

		/// <summary>
		/// Retrieves the <see cref="ICommand"/> attached to the <see cref="IApplicationBarMenuItem"/>.
		/// </summary>
		/// <param name="item">IApplicationBarMenuItem containing the Command dependency property</param>
		/// <returns>The value of the command attached</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for IApplicationBarMenuItem")]
		public static ICommand GetCommand(IApplicationBarMenuItem item)
		{
			return (item as FrameworkElement).GetValue(CommandProperty) as ICommand;
		}

		/// <summary>
		/// Sets the value for the CommandParameter attached property on the provided <see cref="IApplicationBarMenuItem"/>.
		/// </summary>
		/// <param name="item">IApplicationBarMenuItem to attach CommandParameter</param>
		/// <param name="parameter">Parameter value to attach</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for IApplicationBarMenuItem")]
		public static void SetCommandParameter(IApplicationBarMenuItem item, object parameter)
		{
			(item as FrameworkElement).SetValue(CommandParameterProperty, parameter);
		}

		/// <summary>
		/// Gets the value in CommandParameter attached property on the provided <see cref="IApplicationBarMenuItem"/>
		/// </summary>
		/// <param name="item">IApplicationBarMenuItem that has the CommandParameter</param>
		/// <returns>The value of the property</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for IApplicationBarMenuItem")]
		public static object GetCommandParameter(IApplicationBarMenuItem item)
		{
			return (item as FrameworkElement).GetValue(CommandParameterProperty);
		}

		private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			IApplicationBarMenuItem item = dependencyObject as IApplicationBarMenuItem;
			if (item != null)
			{
				AppBarItemCommandBehavior<IApplicationBarMenuItem> behavior = GetOrCreateBehavior(item);
				behavior.Command = e.NewValue as ICommand;
			}
		}

		private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			IApplicationBarMenuItem item = dependencyObject as IApplicationBarMenuItem;
			if (item != null)
			{
				AppBarItemCommandBehavior<IApplicationBarMenuItem> behavior = GetOrCreateBehavior(item);
				behavior.CommandParameter = e.NewValue;
			}
		}

		private static AppBarItemCommandBehavior<IApplicationBarMenuItem> GetOrCreateBehavior(IApplicationBarMenuItem item)
		{
			AppBarItemCommandBehavior<IApplicationBarMenuItem> behavior = (item as FrameworkElement).GetValue(ClickCommandBehaviorProperty) as AppBarItemCommandBehavior<IApplicationBarMenuItem>;
			if (behavior == null)
			{
				behavior = new AppBarItemCommandBehavior<IApplicationBarMenuItem>(item);
				(item as FrameworkElement).SetValue(ClickCommandBehaviorProperty, behavior);
			}

			return behavior;
		}
	}
}