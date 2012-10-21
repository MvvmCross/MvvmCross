using System.Reflection;

namespace Foobar.Dialog.Core.Builder
{
    public interface IPropertySetter
    {
        void Set(object element, string targetPropertyName, string configuration);
    }
}