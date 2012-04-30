using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace Phone7.Fx.Commands.PhonePage
{
    /// <summary>
    /// Static Class that holds all Dependency Properties and Static methods to allow 
    /// the Click event of the ButtonBase class to be attached to a Command. 
    /// </summary>
    /// <remarks>
    /// This class is required, because Silverlight doesn't have native support for Commands. 
    /// </remarks>
    public static class BackKeyPress
    {
        private static readonly DependencyProperty BackKeyPressCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "BackKeyPressCommandBehavior",
            typeof(PhoneApplicationPageBackKeyPressCommandBehavior),
            typeof(BackKeyPress),
            null);


        /// <summary>
        /// Command to execute on click event.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(BackKeyPress),
            new PropertyMetadata(OnSetCommandCallback));

        /// <summary>
        /// Command parameter to supply on command execution.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter",
            typeof(object),
            typeof(BackKeyPress),
            new PropertyMetadata(OnSetCommandParameterCallback));


        /// <summary>
        /// Sets the <see cref="ICommand"/> to execute on the click event.
        /// </summary>
        /// <param name="buttonBase">ButtonBase dependency object to attach command</param>
        /// <param name="command">Command to attach</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for buttonbase")]
        public static void SetCommand(Microsoft.Phone.Controls.PhoneApplicationPage buttonBase, ICommand command)
        {
            buttonBase.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="ButtonBase"/>.
        /// </summary>
        /// <param name="buttonBase">ButtonBase containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for buttonbase")]
        public static ICommand GetCommand(Microsoft.Phone.Controls.PhoneApplicationPage buttonBase)
        {
            return buttonBase.GetValue(CommandProperty) as ICommand;
        }

        /// <summary>
        /// Sets the value for the CommandParameter attached property on the provided <see cref="ButtonBase"/>.
        /// </summary>
        /// <param name="buttonBase">ButtonBase to attach CommandParameter</param>
        /// <param name="parameter">Parameter value to attach</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for buttonbase")]
        public static void SetCommandParameter(Microsoft.Phone.Controls.PhoneApplicationPage buttonBase, object parameter)
        {
            buttonBase.SetValue(CommandParameterProperty, parameter);
        }

        /// <summary>
        /// Gets the value in CommandParameter attached property on the provided <see cref="ButtonBase"/>
        /// </summary>
        /// <param name="buttonBase">ButtonBase that has the CommandParameter</param>
        /// <returns>The value of the property</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for buttonbase")]
        public static object GetCommandParameter(Microsoft.Phone.Controls.PhoneApplicationPage buttonBase)
        {
            return buttonBase.GetValue(CommandParameterProperty);
        }

        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Microsoft.Phone.Controls.PhoneApplicationPage buttonBase = dependencyObject as Microsoft.Phone.Controls.PhoneApplicationPage;
            if (buttonBase != null)
            {
                PhoneApplicationPageBackKeyPressCommandBehavior behavior = GetOrCreateBehavior(buttonBase);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Microsoft.Phone.Controls.PhoneApplicationPage buttonBase = dependencyObject as Microsoft.Phone.Controls.PhoneApplicationPage;
            if (buttonBase != null)
            {
                PhoneApplicationPageBackKeyPressCommandBehavior behavior = GetOrCreateBehavior(buttonBase);
                behavior.CommandParameter = e.NewValue;
            }
        }

        private static PhoneApplicationPageBackKeyPressCommandBehavior GetOrCreateBehavior(Microsoft.Phone.Controls.PhoneApplicationPage buttonBase)
        {
            PhoneApplicationPageBackKeyPressCommandBehavior behavior = buttonBase.GetValue(BackKeyPressCommandBehaviorProperty) as PhoneApplicationPageBackKeyPressCommandBehavior;
            if (behavior == null)
            {
                behavior = new PhoneApplicationPageBackKeyPressCommandBehavior(buttonBase);
                buttonBase.SetValue(BackKeyPressCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
    }
}