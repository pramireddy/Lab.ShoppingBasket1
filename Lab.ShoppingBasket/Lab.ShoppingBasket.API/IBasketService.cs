using Lab.ShoppingBasket.Service;
using System.ServiceModel;
using Lab.ShoppingBasket.BLL;

namespace Lab.ShoppingBasket.API
{
    
    [ServiceContract]
    public interface IBasketService
    {
        [OperationContract]
        BasketServiceResponse GetBasketTotal(IShoppingBasket basket);

    }

}
