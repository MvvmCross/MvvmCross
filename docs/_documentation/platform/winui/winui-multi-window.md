---
layout: documentation
title: WinUI Multi-Window View Presenter
category: Platforms
---

We can support multiple windows on the WinUI Platform.

- A new attribute has been added MvxNewWindowPresentationAttribute.cs has been added which can be put above views. If detected, the ViewPresenter will open a new window
- All navigate calls that have to support to be executed in a Window other then the main Window have to pass the ViewModel it comes from as the Source. (Note: It is not required that they pass the ViewModel they are coming from, but is the most logical option usually). There are additional methods in the IMvxNavigationService to support this
- An attempt is then made to find the correct window in any of the open windows. If no window is found the mainWindow is used instead

- The newly created ViewModel (from the request) is then registered with the Window, so if it wants to call Navigate itself we can find it

- IMvxNeedWindow is used to give the page access to the Window it is in. This allows the page, and by extension the viewmodel via Binding, to set and change the Title and possibly other things. (We only needed it for the title :) )

- If the ViewModel or another service requires direct access to the window, you can use IMvxMultiWindowsService for the IoCContainer. In our case this was used to load the File and Folder selectors in the correct Window

For example:

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
    
    Window mainWindow = this._multiWindowsService.GetWindow(callingViewModel);
    IntPtr handle = WindowNative.GetWindowHandle(mainWindow);
    InitializeWithWindow.Initialize(picker, handle);
    
    return picker;
}
```
