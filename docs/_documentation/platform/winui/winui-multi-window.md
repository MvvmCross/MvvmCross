---
layout: documentation
title: WinUI Multi-Window View Presenter
category: Platforms
---

MvvmCross supports multiple windows on the WinUI Platform using the `MvxMultiWindowViewPresenter`, which is the default presenter on WinUI.

To open a new window your page must inherit from `MvxWindowsPage<TViewModel>` and optionally implement the interface `IMvxNeedWindow`. The interface is used when you need access to the Window itself from the page your are presenting.
Additionally you need to annotate your page with `MvxNewWindowPresentationAttribute`.

```C#
[MvxViewFor(typeof(MyViewModel))]
[MvxNewWindowPresentation]
public sealed partial class MyWindowPage : MvxWindowsPage<MyViewModel>, IMvxNeedWindow
{
    public MyWindowPage()
    {
        InitializeComponent();
    }

    public void SetWindow(Window window, AppWindow appWindow)
    {
        // access the Window in this method from `IMvxNeedWindow`
        AppWindow = appWindow;
        AppWindowUtils.SetTitleBar(appWindow, "Hello new window");
    }

    public AppWindow AppWindow { get; set; }

    public bool CanClose()
    {
        return true;
    }
}
```

The flow in MvvmCross looks something like this:
- When the attribute `MvxNewWindowPresentationAttribute` is detected by the ViewPresenter, it will attempt to open it in a new Window
- All navigate calls that have to support to be executed in a Window other then the main Window have to pass the ViewModel it comes from, as the Source
- An attempt is then made to find the correct window in any of the open windows. If no window is found the `MainWindow` is used instead
- The newly created ViewModel (from the navigation request) is then registered with the Window, so if it wants to call Navigate itself we can find it
- `IMvxNeedWindow` is used to give the page access to the Window it is in. This allows the page, and by extension the ViewModel via Binding, to set and change the Title and possibly other things on the Window itself
- If the ViewModel or another service requires direct access to the window, you can use `IMvxMultiWindowsService` through the IoCContainer

For example to show a file picker:

```C#
public FileService(IMvxMultiWindowsService multiWindowsService)
{
    _multiWindowsService = multiWindowsService;
}

/// <inheritdoc />
public async Task<string?> SelectFolder(IMvxViewModel callingViewModel)
{
    try
    {
        var picker = this.GetPicker<FolderPicker>(callingViewModel);
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        picker.FileTypeFilter.Add("*");
    
        StorageFolder? folder = await picker.PickSingleFolderAsync();
        return folder?.Path;
    }
    catch (COMException)
    {
        // Sometimes the folder picker throws a ComException. This appears to be an issue with the .NET framework.
        // Return the default folder when this happens.
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}

private T GetPicker<T>(IMvxViewModel callingViewModel) where T : new()
{
    var picker = new T();
    
    Window mainWindow = _multiWindowsService.GetWindow(callingViewModel);
    IntPtr handle = WindowNative.GetWindowHandle(mainWindow);
    InitializeWithWindow.Initialize(picker, handle);
    
    return picker;
}
```
