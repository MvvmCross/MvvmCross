namespace MvvmCross.Localization
{
    public interface IMvxLocalizedTextSourceOwner
    {
        IMvxLanguageBinder LocalizedTextSource { get; }
    }
}