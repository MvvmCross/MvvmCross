namespace Cirrious.MvvmCross.Localization
{
    public abstract class MvxBaseResourceProvider
    {
        protected static string MakeLookupKey(string namespaceKey, string typeKey)
        {
            return string.Format("{0}|{1}", namespaceKey, typeKey);
        }

        protected static string MakeLookupKey(string namespaceKey, string typeKey, string name)
        {
            return string.Format("{0}|{1}|{2}", namespaceKey, typeKey, name);
        }
    }
}