namespace $rootnamespace$.Views

open System
open System.Collections.Generic
open System.Linq
open AppKit
open Foundation
open MvvmCross.Binding.Mac.Views

[<Export("initWithCoder:")>]
type FirstView(coder: NSCoder) = 
    inherit MvxView(coder)