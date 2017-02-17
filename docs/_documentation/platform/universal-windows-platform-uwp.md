---
layout: documentation
title: Universal Windows platform (UWP)
category: Platform
---
## Installation
This tutorial will walk you through the installation of MvvmCross into a new Universal Windows Platform project.

Create a new solution in Visual Studio. Right-click the solution in `Solution Explorer` and select `Add -> New Project...`.

In the `Add New Project` dialog select the `Windows Universal` tab and select the `Blank App` template. In the bottom you can set the name. We will use `MvvmCrossDocs.WindowsUniversal` in this example.
[block:image]
{
  "images": [
    {
      "image": [],
      "caption": "Creating project"
    }
  ]
}
```
Now, we will open the New Project dialog again and create a Portable Class Library, that will act as the `Core` shared library in MvvmCross. In the dialog click the `Visual C#` node and find `Class Library (Portable for iOS, Android and Windows)`. Name the project, in our case `MvvmCrossDocs.Core`.

We will now want to reference the `Core` project inside of our Universal Windows project. To do that, right-click the References node under the UWP project in Solution Explorer and select `Add reference…`. In the opened dialog navigate to `Projects -> Solution` in the left-side pane and then check the box next to our `Core` project in the list.

Our projects are now ready and we can install `MvvmCross` from **NuGet**. First right-click the solution in `Solution Explorer` and then select `Manage NuGet Packages for Solution`.
[block:image]
{
  "images": [
    {
      "image": []
    }
  ]
}
```
In the NuGet Package Manager window choose the **Browse** tab and enter *mvvmcross* into the search box. MvvmCross package should appear as the first result, which you can select and then check the boxes next to your project names in the right hand pane. When you are done, you can click the **Install** button to install the package. The installation might take a while and you will be prompted to agree with the changes. Confirm the prompt with **OK** and continue.

Now we will need to add some basic code to get MvvmCross up and running.

First inside the `MvvmCrossDocs.Core` portable project create a file named `App.cs` and enter the following:
```C# using MvvmCross.Platform.IoC;\n\nnamespace MvvmCrossDocs.Core\n{\n    public class App : MvvmCross.Core.ViewModels.MvxApplication\n    {\n        public override void Initialize()\n        {\n            CreatableTypes()\n                .EndingWith(\"Service\")\n                .AsInterfaces()\n                .RegisterAsLazySingleton();\n\n            RegisterAppStart<ViewModels.FirstViewModel>();\n        }\n    }\n}",
```
As you can see, in this code we are registering a first view model, which we will create now to be able to demonstrate the functionality of our setup later. Create a folder called `ViewModels` and inside a new file `FirstViewModel.cs`.
```C# using MvvmCross.Core.ViewModels;\n\nnamespace MvvmCrossDocs.Core.ViewModels\n{\n    public class FirstViewModel \n        : MvxViewModel\n    {\n        private string _hello = \"Hello MvvmCross\";\n        public string Hello\n        { \n            get { return _hello; }\n            set { SetProperty (ref _hello, value); }\n        }\n    }\n}",
```
Now we turn our attention to the Universal Windows Project. In the root of the project create a source file called `Setup.cs` with the following contents:
```C# using MvvmCross.Core.ViewModels;\nusing MvvmCross.Platform.Platform;\nusing MvvmCross.WindowsUWP.Platform;\nusing Windows.UI.Xaml.Controls;\n\nnamespace MvvmCrossDocs.WindowsUniversal\n{\n    public class Setup : MvxWindowsSetup\n    {\n        public Setup( Frame rootFrame ) : base( rootFrame )\n        {\n        }\n\n        protected override IMvxApplication CreateApp()\n        {\n            return new Core.App();\n        }\n\n        protected override IMvxTrace CreateDebugTrace()\n        {\n            return new DebugTrace();\n        }\n    }\n}\n",
```
As you can see, the `DebugTrace` class does not exist. This class is recommended for all MvvmCross projects and it facilitates platform-based console logging during debug. Create a new file `DebugTrace.cs` in the root of your UWP project and paste the following:
```C# using System;\nusing System.Diagnostics;\nusing MvvmCross.Platform.Platform;\n\nnamespace MvvmCrossDocs.WindowsUniversal\n{\n    public class DebugTrace : IMvxTrace\n    {\n        public void Trace( MvxTraceLevel level, string tag, Func<string> message )\n        {\n            Debug.WriteLine( tag + \":\" + level + \":\" + message() );\n        }\n\n        public void Trace( MvxTraceLevel level, string tag, string message )\n        {\n            Debug.WriteLine( tag + \":\" + level + \":\" + message );\n        }\n\n        public void Trace( MvxTraceLevel level, string tag, string message, params object[] args )\n        {\n            try\n            {\n                Debug.WriteLine( tag + \":\" + level + \":\" + message, args );\n            }\n            catch ( FormatException )\n            {\n                Trace( MvxTraceLevel.Error, tag, \"Exception during trace of {0} {1}\", level, message );\n            }\n        }\n    }\n}",
```
Now navigate to the `App.xaml.cs` file in Solution Explorer. We want to start MvvmCross when the app launches. For that to happen, find the conditional `if` check for app's `rootFrame.Content` and replace it with the following:
```C# if (rootFrame.Content == null)\n{\n    var setup = new Setup( rootFrame );\n    setup.Initialize();\n\n    var start = MvvmCross.Platform.Mvx.Resolve<MvvmCross.Core.ViewModels.IMvxAppStart>();\n    start.Start();\n}",
```
Finally, we will create a sample view. Create a folder `Views` in the UWP project and create a new XAML Blank Page called `FirstView.xaml` inside.

Open the XAML file and replace its contents with the following XAML code:
```C# <views:MvxWindowsPage\n    x:Class=\"MvvmCrossDocs.WindowsUniversal.Views.FirstView\"\n    xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\n    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n    xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n    xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n    xmlns:views=\"using:MvvmCross.WindowsUWP.Views\"\n    mc:Ignorable=\"d\">\n\n    <Grid Background=\"{StaticResource ApplicationPageBackgroundThemeBrush}\">\n        <StackPanel>\n            <TextBox Text=\"{Binding Hello, Mode=TwoWay}\" />\n            <TextBlock Text=\"{Binding Hello}\" />\n        </StackPanel>\n    </Grid>\n</views:MvxWindowsPage>",
      "language": "xml"
    }
  ]
}
```
Now there is only one more thing left to do. Open the `FirstView.xaml.cs` code file and change the type from which the `FirstView` class derives to `MvxWindowsPage`:

public sealed partial class FirstView : MvxWindowsPage

Now everything should be correctly set up and you can try to launch the application. If everything is correct, you should see a UI very similar to the following.
[block:image]
{
  "images": [
    {
      "image": []
    }
  ]
}
```
Change the contents of the `TextBox`, and click elsewhere. The text below the `TextBox` should automatically update, proving that the data-binding is working as expected.