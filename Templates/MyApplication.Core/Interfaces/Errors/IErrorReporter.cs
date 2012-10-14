namespace MyApplication.Core.Interfaces.Errors
{
    public interface IErrorReporter
    {
        void ReportError(string error);
    }
}