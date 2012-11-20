namespace CrossUI.Core.Builder
{
    public interface IPropertySetter
    {
        void Set(object element, string targetPropertyName, string configuration);
    }
}