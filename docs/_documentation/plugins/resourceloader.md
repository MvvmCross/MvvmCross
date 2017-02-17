---
layout: documentation
title: ResourceLoader
category: Plugins
---
The `ResourceLoader` plugin provides access to files bundled within the app package.

- On Android, this is for files bundled in the `Assets` folder and marked as Build Action of `AndroidAsset`
- On iOS and Windows, this is for files bundled with a Build Action of `Content` 

On several platforms, the ResourceLoader plugin requires an `IMvxFileStore` is available. One easy way to supply this is to load the `File` plugin.

The main interface supplied by this plugin is:
```C# public interface IMvxResourceLoader\n{\n  bool ResourceExists(string resourcePath);\n  string GetTextResource(string resourcePath);\n  void GetResourceStream(string resourcePath, Action<Stream> streamAction);\n}",
```
For a text file 'Hello.txt' bundled in a folder 'Foo', this can be called as:    

    var loader = Mvx.Resolve<IMvxResourceLoader>();
    var contents = loader.GetTextResource("Foo/Hello.txt");

Samples using the ResourceLoader plugin include:

- Babel - JsonLocalisation - see https://github.com/slodge/MvvmCross-Tutorials/tree/master/Babel
- Conference - the sessions are loaded from Json resources - see https://github.com/slodge/MvvmCross-Tutorials/tree/master/Sample%20-%20CirriousConference