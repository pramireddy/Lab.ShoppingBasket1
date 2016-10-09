using System.Linq;
using Lab.ShoppingBasket.BLL;
using Lab.ShoppingBasket.BLL.Repositories;
using NUnit.Framework;

namespace Lab.ShoppingBasket.BLLTest
{
    [TestFixture]
    public class ShoppingBasketTest
    {
        private IProductRepository _productRepository;
        
        [OneTimeSetUp]
        public void TestSetup()
        {
            _productRepository = new ProductRepository();
            
        }
        [Test]
        public void Test_ShoppingBasket_AddItemToBasket()
        {
            // Arrange
            IShoppingBasket shoppingBasket = new BLL.ShoppingBasket();
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(1));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(1));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(2));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(3));

            // Act
            var itemsCount = shoppingBasket.GetBasketItems().Count();

            // Assert
            Assert.AreEqual(3, itemsCount);

        }

        [Test]
        public void Test_ShoppingBasket_RemoveItemFromBasket()
        {
            // Arrange
            IShoppingBasket shoppingBasket = new BLL.ShoppingBasket();
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(1));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(1));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(2));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(3));

            shoppingBasket.RemoveItemFromBasket(_productRepository.GetProduct(1));
            shoppingBasket.RemoveItemFromBasket(_productRepository.GetProduct(3));

            // Act
            var itemsCount = shoppingBasket.GetBasketItems().Count();

            // Assert
            Assert.AreEqual(2, itemsCount);

        }

        [Test]
        public void Test_ShoppingBasket_EmptyBasketItems()
        {
            // Arrange
            IShoppingBasket shoppingBasket = new BLL.ShoppingBasket();
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(1));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(1));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(2));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(3));

            shoppingBasket.RemoveItemFromBasket(_productRepository.GetProduct(1));
            shoppingBasket.RemoveItemFromBasket(_productRepository.GetProduct(3));

            // Act
            shoppingBasket.EmptyBasketItems();
            var itemsCount = shoppingBasket.GetBasketItems().Count();

            // Assert
            Assert.AreEqual(0, itemsCount);

        }
    }
}
