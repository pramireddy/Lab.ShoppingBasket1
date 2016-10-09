using System.Collections.Generic;
using NUnit.Framework;
using Lab.ShoppingBasket.DAL;
using Lab.ShoppingBasket.BLL;
using Lab.ShoppingBasket.BLL.Repositories;
using Lab.ShoppingBasket.Service;

namespace Lab.ShoppingBasket.ServiceTest
{
    [TestFixture]
    public class BasketServiceTest
    {
        private IProductRepository _productRepository;
        private IGiftVoucherRepository _giftVoucherRepository;
        private IOfferVoucherRepository _offerVoucherRepository;
        private IBasketProcessorFactory _basketProcessorFactory;

        [OneTimeSetUp]
        public void TestSetup()
        {
            _productRepository = new ProductRepository();
            _giftVoucherRepository = new GiftVoucherRepository();
            _offerVoucherRepository = new OfferVoucherRepository();
            _basketProcessorFactory = new BasketProcessorFactory();
        }

        /// <summary>
        /// Scenario : Basket1
        /// Given Products 1 Hat @ £10.50 and 1 Jumper @ £54.65
        /// And  1 x £5.00 Gift Voucher XXX-XXX applied
        /// When I call BasketService.GetBasketTotal()
        /// Then Basket Total : £60.15
        /// </summary>
        [Test]
        public void Basket1()
        {
            // arrage
            var shoppingBasket = new BLL.ShoppingBasket
            {
                GiftVouchers = new List<GiftVoucher>
                {
                    _giftVoucherRepository.GetGiftVoucher(1)
                }

            };
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(1));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(2));
            var basetService = new BasketService(_basketProcessorFactory);

            // act 
            var basketServiceResponse = basetService.GetBasketTotal(shoppingBasket);

            // assert
            Assert.AreEqual(60.15, basketServiceResponse.BasketTotal);
            Assert.AreEqual(0, basketServiceResponse.Notifications.Count);
        }

        /// <summary>
        /// Scenario : Basket2
        /// Given Products 1 Hat @ £25.00 and 1 Jumper @ £26.00
        /// And  1 x £5.00 off Head Gear in baskets over £50.00 Offer Voucher YYY-YYY applied
        /// When I call BasketService.GetBasketTotal()
        /// Then Basket Total : £51.00
        /// Then Message: "There are no products in your basket applicable to voucher Voucher YYY-YYY."
        /// </summary>
        [Test]
        public void Basket2()
        {
            // arrage
            var shoppingBasket = new BLL.ShoppingBasket
            {
                OfferVoucher = _offerVoucherRepository.GetOffVoucher(1)
            };
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(3));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(4));
            var basetService = new BasketService(_basketProcessorFactory);

            // act 
            var basketServiceResponse = basetService.GetBasketTotal(shoppingBasket);

            // assert
            Assert.AreEqual(51.00, basketServiceResponse.BasketTotal);
            Assert.AreEqual(1, basketServiceResponse.Notifications.Count);
            Assert.AreEqual("There are no products in your basket applicable to voucher Voucher YYY-YYY.", basketServiceResponse.Notifications[0]);
        }

        /// <summary>
        /// Scenario : Basket3
        /// Given Products 1 Hat @ £25.00 , 1 Jumper @ £26.00 and 1 Head Light (Head Gear Category of Product)  @ £3.50
        /// And  1 x £5.00 off Head Gear in baskets over £50.00 Offer Voucher YYY-YYY applied
        /// When I call BasketService.GetBasketTotal()
        /// Then Basket Total : £51.00
        /// </summary>
        [Test]
        public void Basket3()
        {
            // arrage
            var shoppingBasket = new BLL.ShoppingBasket
            {
                OfferVoucher = _offerVoucherRepository.GetOffVoucher(1)
            };
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(3));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(4));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(5));

            var basetService = new BasketService(_basketProcessorFactory);

            // act 
            var basketServiceResponse = basetService.GetBasketTotal(shoppingBasket);

            // assert
            Assert.AreEqual(51.00, basketServiceResponse.BasketTotal);
            Assert.AreEqual(0, basketServiceResponse.Notifications.Count);
        }

        /// <summary>
        /// Scenario : Basket4
        /// Given Products 1 Hat @ £25.00 and 1 Jumper @ £26.00 
        /// And  1 x £5.00 Gift Voucher XXX-XXX applied
        /// And  1 x £5.00 off Head Gear in baskets over £50.00 Offer Voucher YYY-YYY applied
        /// When I call BasketService.GetBasketTotal()
        /// Then Basket Total : £41.00
        /// </summary>
        [Test]
        public void Basket4()
        {
            // arrage
            var shoppingBasket = new BLL.ShoppingBasket
            {
                GiftVouchers = new List<GiftVoucher>
                {
                    _giftVoucherRepository.GetGiftVoucher(1)
                },
                OfferVoucher = _offerVoucherRepository.GetOffVoucher(2)
            };
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(3));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(4));
            var basetService = new BasketService(_basketProcessorFactory);

            // act 
            var basketServiceResponse = basetService.GetBasketTotal(shoppingBasket);

            // assert
            Assert.AreEqual(41.00, basketServiceResponse.BasketTotal);
            Assert.AreEqual(0, basketServiceResponse.Notifications.Count);
        }

        /// <summary>
        /// Scenario : Basket5
        /// Given Products 1 Hat @ £25.00 and 1 £30 Gift Voucher @ £30.00
        /// And  1 x £5.00 off baskets over £50.00 Offer Voucher YYY-YYY applied
        /// When I call BasketService.GetBasketTotal()
        /// Then Basket Total : £55.00
        /// </summary>
        [Test]
        public void Basket5()
        {
            // arrage
            var shoppingBasket = new BLL.ShoppingBasket
            {
                OfferVoucher = _offerVoucherRepository.GetOffVoucher(2)
            };
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(3));
            shoppingBasket.AddtemToBasket(_productRepository.GetProduct(6));
            var basetService = new BasketService(_basketProcessorFactory);

            // act 
            var basketServiceResponse = basetService.GetBasketTotal(shoppingBasket);

            // assert
            Assert.AreEqual(55.00, basketServiceResponse.BasketTotal);
            Assert.AreEqual(1, basketServiceResponse.Notifications.Count);
            Assert.AreEqual("You have not reached the spend threshold for voucher YYY-YYY. Spend another £25.01 to receive £5.00 discount from your basket total.", basketServiceResponse.Notifications[0]);
        }
    }
}
