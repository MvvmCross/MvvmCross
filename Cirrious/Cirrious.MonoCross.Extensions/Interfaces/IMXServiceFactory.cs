namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXServiceFactory
    {
        T CreateService<T>() where T : class;
    }
}