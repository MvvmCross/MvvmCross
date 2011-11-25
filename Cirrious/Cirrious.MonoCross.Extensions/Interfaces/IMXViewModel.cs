namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXViewModel : IMXStopNowPlease
    {
        IMXViewDispatcher ViewDispatcher { get; set; }
    }
}