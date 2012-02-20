using System.Reflection;
using Android.Content;

namespace Cirrious.MvvmCross.Android.Interfaces
{
    public interface IMvxAndroidGlobals
    {
        string ExecutableNamespace { get; }
        Assembly ExecutableAssembly { get; }
        Context ApplicationContext { get; }
    }
}