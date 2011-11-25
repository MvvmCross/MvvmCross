using Cirrious.MonoCross.Extensions.Platform;

namespace Cirrious.MonoCross.Extensions.Controllers
{
    public abstract class MXServiceConsumerActionBasedController<T> : MXActionBasedController<T>, IMXServiceConsumer
    {
    }
}