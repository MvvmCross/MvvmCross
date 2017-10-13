---
layout: documentation
title: File
category: Plugins
---
The `File` plugin provides cross-platform access to a File Store API.

```c#
public interface IMvxFileStore
{
    bool TryReadTextFile(string path, out string contents);
    bool TryReadBinaryFile(string path, out Byte[] contents);
    bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod);
    void WriteFile(string path, string contents);
    void WriteFile(string path, IEnumerable<Byte> contents);
    void WriteFile(string path, Action<Stream> writeMethod);
    bool TryMove(string from, string to, bool deleteExistingTo);
    bool TryCopy(string from, string to, bool overwrite);
    bool Exists(string path);
    bool FolderExists(string folderPath);
    string PathCombine(string items0, string items1);
    string NativePath(string path);
    void EnsureFolderExists(string folderPath);
    IEnumerable<string> GetFilesIn(string folderPath);
    IEnumerable<string> GetFoldersIn(string folderPath);
    void DeleteFile(string path);
    void DeleteFolder(string folderPath, bool recursive);
    Stream OpenRead(string path);
    Stream OpenWrite(string path);
    long GetSize(string path);
    DateTime GetLastWriteTimeUtc(string path);
}
```

This plugin is implemented on all platforms - except Windows Uwp where the `Folder` APIs are currently unimplemented.

By default, the plugin reads and writes files in paths relative to:

platform     | location
-------------|---------
Android      | `Context.FilesDir`
iOS          | `Environment.SpecialFolder.MyDocuments`
Uwp | `Windows.Storage.ApplicationData.Current.LocalFolder.Path`
WPF          | `Environment.SpecialFolder.ApplicationData`

Note: while it works, the use of a synchronous API for File IO on WindowsStore applications is slightly 'naughty'. It's likely that an asynchronous version of the IMvxFileStore interface will be provided in the near future.

