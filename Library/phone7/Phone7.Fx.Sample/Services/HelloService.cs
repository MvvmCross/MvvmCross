using Phone7.Fx.Sample.Services.Contracts;

namespace Phone7.Fx.Sample.Services
{
public class HelloService:IHelloService
{
    public string SayHello()
    {
        return "Hello Phone7.Fx !";
    }
}
}