namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces
{
    public interface IMvxDefaultViewTextLoader
    {
        bool HasDefinition(string viewType, string key);
        string GetDefinition(string viewType, string key);
    }
}