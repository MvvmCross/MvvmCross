---
layout: documentation
title: Async operations with MvxNotifyTask
category: Advanced
order: 5
---

MvvmCross provides a super useful helper when it comes to async/await: [MvxNotifyTask](https://github.com/MvvmCross/MvvmCross/blob/develop/MvvmCross/Core/Core/ViewModels/MvxNotifyTask.cs). 

You might have missed this, but it is already being used in your application: The Initialize method is fired inside of a MvxNotifyTask, and if your ViewModels derive from `MvxViewModel`, you will find that there is a property called `InitializeTask`. You can use that property to spot any changes to the states of Initialize.

MvxNotifyTask provides you with an object that can watch for different Task states and raise property-changed notifications that you can subscribe to. This means you can bind any Task properties in your Views!

Other than that, this class acts as a _Sandbox_ for async operations: If a Task fails and raises an exception, your app won't crash, and the exception will be available for you through `MvxNotifyTask.Exception`. 

The properties MvxNotifyTask exposes are the following:

```c#
/// <summary>
/// Gets the task being watched. This property never changes and is never <c>null</c>.
/// </summary>
public Task Task { get; private set; }

/// <summary>
/// Gets a task that completes successfully when <see cref="Task"/> completes (successfully, faulted, or canceled). This property never changes and is never <c>null</c>.
/// </summary>
public Task TaskCompleted { get; private set; }

/// <summary>
/// Gets the current task status. This property raises a notification when the task completes.
/// </summary>
public TaskStatus Status { get { return Task.Status; } }

/// <summary>
/// Gets whether the task has completed. This property raises a notification when the value changes to <c>true</c>.
/// </summary>
public bool IsCompleted { get { return Task.IsCompleted; } }

/// <summary>
/// Gets whether the task is busy (not completed). This property raises a notification when the value changes to <c>false</c>.
/// </summary>
public bool IsNotCompleted { get { return !Task.IsCompleted; } }

/// <summary>
/// Gets whether the task has completed successfully. This property raises a notification when the value changes to <c>true</c>.
/// </summary>
public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

/// <summary>
/// Gets whether the task has been canceled. This property raises a notification only if the task is canceled (i.e., if the value changes to <c>true</c>).
/// </summary>
public bool IsCanceled { get { return Task.IsCanceled; } }

/// <summary>
/// Gets whether the task has faulted. This property raises a notification only if the task faults (i.e., if the value changes to <c>true</c>).
/// </summary>
public bool IsFaulted { get { return Task.IsFaulted; } }

/// <summary>
/// Gets the wrapped faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
/// </summary>
public AggregateException Exception { get { return Task.Exception; } }

/// <summary>
/// Gets the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
/// </summary>
public Exception InnerException { get { return (Exception == null) ? null : Exception.InnerException; } }

/// <summary>
/// Gets the error message for the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
/// </summary>
public string ErrorMessage { get { return (InnerException == null) ? null : InnerException.Message; } }

```

### Usage

The way you would typically use MvxNotifyTask is by defining a public property in your ViewModel, that is assigned when a Command operation needs to be triggered, and then binding any properties at View level (just keep in mind this is an example, you can use it in many other ways!).

MvxNotifyTask exposes many constructors through a static class, but the most secure is the one that takes a Func<Task> as a parameter. This is because by using it you can be 100% the task starts to run inside the Sandbox (if the task fails while MvxNotifyTask is not yet listening, your app can crash).

### Example

Suppose you need to perform an async operation when the user taps on a button, and display a certain UI widget while the operation is running. 

This is what your ViewModel would look like: 

```c#
using MvvmCross.Core.ViewModels;
//...

public class MyViewModel : MvxViewModel
{
    private readonly ISomeService _someService;
    
    public MyViewModel(ISomeService someService)
    {
        _someService = someService;

        MyCommand = new MvxCommand(() => MyTaskNotifier = MvxNotifyTask.Create(() => MyMethodAsync(), onException: ex => OnException(ex)));
    }

    public void Prepare()
    {

    }

    public Task Initialize()
    {
        return base.Initialize();
    }
    
    public IMvxCommand MyCommand { get; private set; }

    private MvxNotifyTask _myTaskNotifier;
    public MvxNotifyTask MyTaskNotifier 
    {
        get => _myTaskNotifier;
        private set => SetProperty(ref _myTaskNotifier, value);
    }

    // ...

    private async Task MyMethodAsync()
    {
        await _someService.DoSomethingAsync();
        
        // ...
    }

    private void OnException(Exception exception)
    {
        // log the handled exception!
    }
}
```

That's it! You can also use the optional parameter `onException` to get some code run in case any exception occurs.

Everything that is left now is to assign the bindings on the Views that we want to display while the operation is running. 

On Android:

```xml
<LinearLayout
     android:layout_width="match_parent"
     android:layout_height="wrap_content"
     local:MvxBind="Visible MyTaskNotifier.IsNotCompleted">
</LinearLayout>
```

On iOS:

```c#
var set = this.CreateBindingSet<MyView, MyViewModel>();
set.Bind(_myControl).For("Visibility").To(vm => vm.MyTaskNotifier.IsCompleted);
set.Apply();
```

### Generalizing a way to catch 'em all!

Although MvxNotifyTask provides a clean way to manage async code states at all levels, you can take it one step further and create a custom implementation that makes logging exceptions dead easy:

```c#
public static class CustomNotifyTask
{
    public static MvxNotifyTask Create(Func<Task> task)
    {
        return MvxNotifyTask.Create(
            async () =>
            {
                try
                {
                    await task.Invoke();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    throw ex;
                }
            });
    }
}
```

As long as you run your async operations using a NotifyTask object, your app won't crash anymore.

Disclaimer note: MvxNotifyTask class and its dependencies are originally created by Stephen Cleary and its code is being modified and redistributed from his library Mvvm.Async.
