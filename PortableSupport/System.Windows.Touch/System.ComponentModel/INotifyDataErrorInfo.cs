using System.Collections;

// ReSharper disable CheckNamespace
namespace System.ComponentModel
// ReSharper restore CheckNamespace
{
    public interface INotifyDataErrorInfo
    {
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        IEnumerable GetErrors(string propertyName);

        bool HasErrors { get; }
    }
}