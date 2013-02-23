namespace Cirrious.CrossCore.Interfaces.Core
{
#warning Really needs another name - IMvxDataConsumer sucks (and it doesn't really cosnumer)
    public interface IMvxDataConsumer
    {
		object DataContext { get; set; }
    }
}