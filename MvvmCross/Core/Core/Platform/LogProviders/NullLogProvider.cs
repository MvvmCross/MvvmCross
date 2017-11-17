namespace MvvmCross.Core.Platform.LogProviders
{
    internal sealed class NullLogProvider : MvxBaseLogProvider
    {
        protected override Logger GetLogger(string name) => new Logger((logLevel, messageFunc, exception, formatParameters) => true);

    }
}
