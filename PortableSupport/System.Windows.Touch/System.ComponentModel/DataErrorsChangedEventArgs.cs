// ReSharper disable CheckNamespace
namespace System.ComponentModel
// ReSharper restore CheckNamespace
{
    public sealed class DataErrorsChangedEventArgs : EventArgs
    {
        private readonly string _propertyName;

        public DataErrorsChangedEventArgs(string propertyName)
        {
            _propertyName = propertyName;
        }

        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }
    }
}