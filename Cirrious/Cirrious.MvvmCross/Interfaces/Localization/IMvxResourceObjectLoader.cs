namespace Cirrious.MvvmCross.Interfaces.Localization
{
    public interface IMvxResourceObjectLoader<T>
        where T : IMvxResourceObject
    {
        T Load(string namespaceKey, string typeKey, string entryKey);
    }
}