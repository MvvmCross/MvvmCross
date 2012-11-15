using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(System.Collections.ObjectModel.ObservableCollection<>))]
[assembly: TypeForwardedTo(typeof(System.Collections.ObjectModel.ReadOnlyObservableCollection<>))]
[assembly: TypeForwardedTo(typeof(System.Collections.Specialized.INotifyCollectionChanged))]
[assembly: TypeForwardedTo(typeof(System.Collections.Specialized.NotifyCollectionChangedAction))]
[assembly: TypeForwardedTo(typeof(System.Collections.Specialized.NotifyCollectionChangedEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Collections.Specialized.NotifyCollectionChangedEventHandler))]

// note that in VisualStudio, MONOTOUCH is not defined
#if MONOTOUCH
[assembly: TypeForwardedTo(typeof(System.Windows.Input.ICommand))]
#endif
