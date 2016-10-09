﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lab.ShoppingBasket.Service;
using Lab.ShoppingBasket.BLL;

namespace Lab.ShoppingBasket.API
{

    public class BasketService : IBasketService
    {
        private IList<IBasketProcessor> _basketProcessors;
        private readonly IBasketProcessorFactory _basketProcessorFactory;

        public BasketService(IBasketProcessorFactory basketProcessorFactory)
        {
            _basketProcessorFactory = basketProcessorFactory;
        }

        public BasketServiceResponse GetBasketTotal(IShoppingBasket shoppingBasket)
        {
            try
            {
                if (!shoppingBasket.GetBasketItems().Any())
                    return new BasketServiceResponse()
                    {
                        Notifications = new List<string> { "Your shopping basket is empty" },
                        Success = true,
                        BasketTotal = 0.0m
                    };

                _basketProcessors = CreateBasketProcessors();

                foreach (var processor in _basketProcessors)
                {
                    processor.Process(shoppingBasket);
                }

                // TODO: Use automapper for mapping Basket object to BasketServiceResponse object
                return new BasketServiceResponse
                {
                    Notifications = shoppingBasket.Messages?.ToList(),
                    BasketTotal = shoppingBasket.Total,
                    Success = true
                };


            }
            catch (Exception exception)
            {
                return new BasketServiceResponse
                {
                    ErrorMessage = $"Failed to calculate basket total {exception.Message}",
                    Success = false,
                    BasketTotal = 0.0m
                };
            }

        }

        private IList<IBasketProcessor> CreateBasketProcessors()
        {
            return new List<IBasketProcessor>
            {
               _basketProcessorFactory.CreateProductProcessor(),
               _basketProcessorFactory.CreateOfferVoucherProcessor(),
               _basketProcessorFactory.CreateGiftVoucherProcessor()
            };
        }
    }
}
