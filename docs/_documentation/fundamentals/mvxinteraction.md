---
layout: documentation
title: ViewModel to View Interaction
category: Fundamentals
order: 11
---

Sometimes when interacting between a View and ViewModel, you want to offload some responsibility to the View. In other words, you want
to request interaction from the View.
However, you still want to keep the View and ViewModel separate from each other. This is where `IMvxInteraction` comes into the picture.

`IMvxInteraction` lets the ViewModel interact with the View. Similarly to how a View would interact with a ViewModel through `MvxCommand`.
`IMvxInteraction` allows the dev to pass along an arbitrary payload, which in complex scenarios also could have callbacks into the ViewModel.
It decouples the View and ViewModel from knowing each other directly. The only thing the View sees is a `IMvxInteraction` instance, which it
can listen to the `Requested` event to know when an interaction is requested.

# Example

Let us start with a simple example. The scenario is that you want to signal the View to show a dialog with a couple of options, before you
want to proceed executing a Command.

So the flow would be:

1. Press a button or something that triggers a `MvxCommand`
2. The command needs interaction from the user, yes/no
3. Command finishes based on interaction

Lets start by defining our interaction object in our core project.

## Interaction class

```c#
public class YesNoQuestion
{
    public Action<bool> YesNoCallback { get; set; }
    public string Question { get; set; }
}
```

This seems pretty simple enough. We have a callback when the user presses yes or no, and a text for the question in the dialog.

## ViewModel definition

In our `ViewModel` we need to define a `MvxInteraction`

```c#
private MvxInteraction<YesNoQuestion> _interaction =
    new MvxInteraction<YesNoQuestion>();
    
// need to expose it as a public property for binding (only IMvxInteraction is needed in the view)
public IMvxInteraction<YesNoCancel> Interaction => _interaction;
```

Now lets imagine we have a `MvxCommand` the user triggers to finish creating their profile. Here we want this interaction to happen,
to ask if they are sure.

```c#
private void DoFinishProfileCommand()
{
    // 1. do cool stuff with profile data
    // ...
    
    // 2. request interaction from view
    // 3. execution continues in callbacks
    var request = new YesNoQuestion
    {
        YesNoCallback = async (ok) => 
        {
            if (ok)
                await SaveProfile();
            else
                await Cancel();
        },
        Question = "Do you want to save your profile?"
    };
    
    _interaction.Raise(request);
}
```

## View definition

Now that we can request an interaction from the ViewModel, we need to react to it from the View. A small bit of boiler plate is needed
here. To keep this example simple we subscribe directly to the `Requested` event. However, you may prefer to use `WeakSubscribe` or use Rx.Net's
`Observable.FromEventPattern`.

```c#
private IMvxInteraction<YesNoQuestion> _interaction;
public IMvxInteraction<YesNoQuestion> Interaction
{
    get => _interaction;
    set
    {
        if (_interaction != null)
            _interaction.Requested -= OnInteractionRequested;
            
        _interaction = value;
        _interaction.Requested += OnInteractionRequested;
    }
}
```

Now we just need to react to the interaction request when triggered by the event.

```c#
private async void OnInteractionRequested(object sender, MvxValueEventArgs<YesNoQuestion> eventArgs)
{
    var yesNoQuestion = eventArgs.Value;
    // show dialog
    var status = await ShowDialog(yesNoQuestion.Question);
    yesNoQuestion.YesNoCallback(status == DialogStatus.Yes);
}
```

## Wiring up Interaction between View and ViewModel

Now that we have all the behavior defined, we just need to wire the View up to the ViewModel. As always this is done through bindings, 
which we will leverage here as well.

As per usual, we just need to create a binding set and apply the binding.

```c#
var set = this.CreateBindingSet<OurView, OurViewModel>();
set.Bind(this).For(view => view.Interaction).To(viewModel => viewModel.Interaction).OneWay();
set.Apply();
```

This is it. You should now be able to interact with the View from your ViewModel.
