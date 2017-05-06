namespace $rootnamespace$

open System.Collections.Specialized
open System.Windows.Input
open Android.App
open Android.Views
open Android.Widget
open MvvmCross.Platform.IoC
open MvvmCross.Binding.BindingContext
open System.ComponentModel

// This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
// are preserved in the deployed app
type LinkerPleaseInclude =
    member this.Include(button: Button) =
        button.Click.Add(fun e -> button.Text <- button.Text + "")

    member this.Include(checkBox: CheckBox) =
        checkBox.CheckedChange.Add(fun e -> checkBox.Checked <- not checkBox.Checked)
            
    member this.Include(switch: Switch) =
        switch.CheckedChange.Add(fun e -> switch.Checked <- not switch.Checked)

    member this.Include(view: View) =
        view.Click.Add(fun e -> view.ContentDescription <- view.ContentDescription + "")

    member this.Include(text: TextView) =
        text.AfterTextChanged.Add(fun e -> text.Text <- "" + text.Text)
        text.Hint <- "" + text.Hint
            
    member this.Include(text: CheckedTextView) =
        text.AfterTextChanged.Add(fun e -> text.Text <- "" + text.Text)
        text.Hint <- "" + text.Hint

    member this.Include(cb: CompoundButton) =
        cb.CheckedChange.Add(fun e -> cb.Checked <- not cb.Checked)

    member this.Include(sb: SeekBar) =
        sb.ProgressChanged.Add(fun e -> sb.Progress <- sb.Progress + 1)

    member this.Include(radioGroup: RadioGroup) =
        radioGroup.CheckedChange.Add(fun e -> radioGroup.Check(e.CheckedId))

    member this.Include(radioButton: RadioButton) =
        radioButton.CheckedChange.Add(fun e -> radioButton.Checked <- e.IsChecked)

    member this.Include(act: Activity) =
        act.Title <- act.Title + ""
                
    member this.Include(changed: INotifyCollectionChanged) =
        changed.CollectionChanged.Add(fun e -> (e.Action, e.NewItems, e.NewStartingIndex, e.OldItems, e.OldStartingIndex) |> ignore)
            
    member this.Include(command: ICommand) =
        command.CanExecuteChanged.Add(fun e -> if (command.CanExecute(null)) then command.Execute(null))
                
    member this.Include(injector: MvxPropertyInjector) =
        new MvxPropertyInjector()
    
    member this.Include(changed: INotifyPropertyChanged) =
        changed.PropertyChanged.Add(fun e -> e.PropertyName |> ignore)
                        
    member this.Include(context: MvxTaskBasedBindingContext) =
        context.Dispose()
        let context2 = new MvxTaskBasedBindingContext()
        context2.Dispose()
        