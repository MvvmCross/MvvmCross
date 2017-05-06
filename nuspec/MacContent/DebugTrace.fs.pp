namespace $rootnamespace$
                   
open System
open System.Diagnostics
open MvvmCross.Platform.Platform

type DebugTrace() =

    member this.Trace(level: MvxTraceLevel, tag: string, message: string) =
        printfn "%s:%O:%s" tag level message

    interface IMvxTrace with
        
        member this.Trace(level: MvxTraceLevel, tag: string, message: Func<string>) =
            printfn "%s:%O:%s" tag level (message.Invoke())

        member this.Trace(level: MvxTraceLevel, tag: string, message: string) =
            this.Trace(level, tag, message)

        member this.Trace(level: MvxTraceLevel, tag: string, message: string, [<ParamArray>] args: Object[]) =
            try
                let formatString = sprintf "%s:%O:%s" tag level message
                Debug.WriteLine(formatString, args)
            with 
                | :? FormatException -> this.Trace(MvxTraceLevel.Error, tag, (sprintf "Exception during trace of %O %s" level message))