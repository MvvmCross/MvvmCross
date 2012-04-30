using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Phone7.Fx.Commands.LoadingListBox
{

    public static class NeedItems
    {
        private static readonly DependencyProperty NeedItemsCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "NeedItemsCommandBehavior",
            typeof(NeedItemsCommandBehavior),
            typeof(NeedItems),
            null);


        /// <summary>
        /// Command to execute on click event.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(NeedItems),
            new PropertyMetadata(OnSetCommandCallback));

        /// <summary>
        /// Command parameter to supply on command execution.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter",
            typeof(object),
            typeof(NeedItems),
            new PropertyMetadata(OnSetCommandParameterCallback));


       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for buttonbase")]
        public static void SetCommand(Controls.LoadingListBox ctrl, ICommand command)
        {
            ctrl.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="ButtonBase"/>.
        /// </summary>
        /// <param name="ctrl">ButtonBase containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for buttonbase")]
        public static ICommand GetCommand(Controls.LoadingListBox ctrl)
        {
            return ctrl.GetValue(CommandProperty) as ICommand;
        }

        /// <summary>
        /// Sets the value for the CommandParameter attached property on the provided <see cref="ButtonBase"/>.
        /// </summary>
        /// <param name="ctrl">ButtonBase to attach CommandParameter</param>
        /// <param name="parameter">Parameter value to attach</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for buttonbase")]
        public static void SetCommandParameter(Controls.LoadingListBox ctrl, object parameter)
        {
            ctrl.SetValue(CommandParameterProperty, parameter);
        }

        /// <summary>
        /// Gets the value in CommandParameter attached property on the provided <see cref="ButtonBase"/>
        /// </summary>
        /// <param name="ctrl">ButtonBase that has the CommandParameter</param>
        /// <returns>The value of the property</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for buttonbase")]
        public static object GetCommandParameter(Controls.LoadingListBox ctrl)
        {
            return ctrl.GetValue(CommandParameterProperty);
        }

        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Controls.LoadingListBox ctrl = dependencyObject as Controls.LoadingListBox;
            if (ctrl != null)
            {
                NeedItemsCommandBehavior behavior = GetOrCreateBehavior(ctrl);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Controls.LoadingListBox ctrl = dependencyObject as Controls.LoadingListBox;
            if (ctrl != null)
            {
                NeedItemsCommandBehavior behavior = GetOrCreateBehavior(ctrl);
                behavior.CommandParameter = e.NewValue;
            }
        }

        private static NeedItemsCommandBehavior GetOrCreateBehavior(Controls.LoadingListBox ctrl)
        {
            NeedItemsCommandBehavior behavior = ctrl.GetValue(NeedItemsCommandBehaviorProperty) as NeedItemsCommandBehavior;
            if (behavior == null)
            {
                behavior = new NeedItemsCommandBehavior(ctrl);
                ctrl.SetValue(NeedItemsCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
    }
}