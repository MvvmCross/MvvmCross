namespace $rootnamespace$
          
open UIKit
open System
open System.Collections.Specialized
open System.Windows.Input
open Foundation
open MvvmCross.Binding.BindingContext
open MvvmCross.iOS.Views
open System.ComponentModel
open MvvmCross.Platform.IoC

// This class is never actually executed, but when Xamarin linking is enabled it does ensure types and properties
// are preserved in the deployed app
[<Preserve(AllMembers = true)>]
type LinkerPleaseInclude =
    
    member this.Include(c: MvxTaskBasedBindingContext) =
        c.Dispose()
        let c2 = new MvxTaskBasedBindingContext()
        c2.Dispose()
    
    member this.Include(uiButton: UIButton) =
        uiButton.TouchUpInside.Add(fun e -> uiButton.SetTitle(uiButton.Title(UIControlState.Normal), UIControlState.Normal))
    
    member this.Include(barButton: UIBarButtonItem) =
        barButton.Clicked.Add(fun e -> barButton.Title <- barButton.Title + "")
    
    member this.Include(textField: UITextField) =
        textField.Text <- textField.Text + ""
        textField.EditingChanged.Add(fun e -> textField.Text <- "")
                
    member this.Include(textView: UITextView) =
        textView.Text <- textView.Text + ""
        textView.Changed.Add(fun e -> textView.Text <- "")
        
    member this.Include(label: UILabel) =
        label.Text <- label.Text + ""
        label.AttributedText <- new NSAttributedString(label.AttributedText.ToString() + "")
    
    member this.Include(imageView: UIImageView) =
        imageView.Image <- new UIImage(imageView.Image.CGImage)
    
    member this.Include(date: UIDatePicker) =
        date.Date <- date.Date.AddSeconds(1.0)
        date.ValueChanged.Add(fun e -> date.Date <- NSDate.DistantFuture)
        
    member this.Include(slider: UISlider) =
        slider.Value <- slider.Value + float32(1.0)
        slider.ValueChanged.Add(fun e -> slider.Value <- float32(1.0))
        
    member this.Include(progress: UIProgressView) =
        progress.Progress <- progress.Progress + float32(1.0)
    
    member this.Include(sw: UISwitch) =
        sw.On <- not sw.On
        sw.ValueChanged.Add(fun e -> sw.On <- false)
        
    member this.Include(vc: MvxViewController) =
        vc.Title <- vc.Title + ""

    member this.Include(s: UIStepper) =
        s.Value <- s.Value + 1.0
        s.ValueChanged.Add(fun e -> s.Value <- 0.0)
        
    member this.Include(s: UIPageControl) =
        s.Pages <- s.Pages + nint(1)
        s.ValueChanged.Add(fun e -> s.Pages <- nint(0)) 
                
    member this.Include(changed: INotifyCollectionChanged) =
        changed.CollectionChanged.Add(fun e -> (e.Action, e.NewItems, e.NewStartingIndex, e.OldItems, e.OldStartingIndex) |> ignore)
            
    member this.Include(command: ICommand) =
        command.CanExecuteChanged.Add(fun e -> if (command.CanExecute(null)) then command.Execute(null))
                
    member this.Include(injector: MvxPropertyInjector) =
        new MvxPropertyInjector()
    
    member this.Include(changed: INotifyPropertyChanged) =
        changed.PropertyChanged.Add(fun e -> e.PropertyName |> ignore)