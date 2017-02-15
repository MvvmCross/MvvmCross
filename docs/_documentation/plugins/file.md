---
title: "File"
excerpt: ""
---
The `File` plugin provides cross-platform access to a File Store API.
[block:code]
{
  "codes": [
    {
      "code": "public interface IMvxFileStore\n{\n  bool TryReadTextFile(string path, out string contents);\n  bool TryReadBinaryFile(string path, out Byte[] contents);\n  bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod);\n  void WriteFile(string path, string contents);\n  void WriteFile(string path, IEnumerable<Byte> contents);\n  void WriteFile(string path, Action<Stream> writeMethod);\n  bool TryMove(string from, string to, bool deleteExistingTo);\n  bool Exists(string path);\n  bool FolderExists(string folderPath);\n  string PathCombine(string items0, string items1);\n  string NativePath(string path);\n\n  void EnsureFolderExists(string folderPath);\n  IEnumerable<string> GetFilesIn(string folderPath);\n  void DeleteFile(string path);\n  void DeleteFolder(string folderPath, bool recursive);\n}",
      "language": "csharp"
    }
  ]
}
[/block]
This plugin is implemented on all platforms - except WindowsStore where the `Folder` APIs are currently unimplemented.

By defautlt, the plugin reads and writes files in paths relative to:

- Android - `Context.FilesDir`
- iOS - `Environment.SpecialFolder.MyDocuments`
- WindowsPhone - app-specific isolated storage
- WindowsStore - `Windows.Storage.ApplicationData.Current.LocalFolder.Path`
- Wpf - `Environment.SpecialFolder.ApplicationData`

Note: while it works, the use of a synchronous API for File IO on WindowsStore applications is slightly 'naughty'. It's likely that an asynchronous version of the IMvxFileStore interface will be provided in the near future.