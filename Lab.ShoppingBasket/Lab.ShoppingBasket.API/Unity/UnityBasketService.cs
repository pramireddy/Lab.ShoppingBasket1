using Lab.ShoppingBasket.BLL;
using Microsoft.Practices.Unity;
using Unity.Wcf;

namespace Lab.ShoppingBasket.API.Unity
{
    public class UnityBasketService : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container
                .RegisterType<IBasketProcessorFactory, BasketProcessorFactory>();
        }
    }
}