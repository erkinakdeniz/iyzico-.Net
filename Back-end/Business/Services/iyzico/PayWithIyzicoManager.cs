using Business.Constants;
using Core.Utilities.ComputerInfo;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.iyzico
{

    public class PayWithIyzicoManager : IIyzicoService
    {
        IBasketRepository _basketRepository;
        IOrderRepository _orderRepository;
        IUserRepository _userRepository;
        IProductRepository _productRepository;

        public PayWithIyzicoManager(IBasketRepository basketRepository, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _basketRepository = basketRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<IResult>  PayWithIyzico(Iyzico iyzicoModel)
        {
          //  var user = _userRepository.Get(x => x.Email == iyzicoModel.Email);
            var Order = new Order
            {
                UserId = 1,
                Massage = Messages.OrderOk,
                OrderDate = DateTime.Now,
                ShipCity = iyzicoModel.City,
                Status = 0,
                Baskets = iyzicoModel.cartItems.Select(x => new Basket
                {
                    ProductID = x.Product.ProductID,
                    Price = x.Product.UnitPrice,
                    Quantity = x.Quantity
                }).ToList()

            };
            _orderRepository.Add(Order);
           await _orderRepository.SaveChangesAsync();
            var Basket = iyzicoModel.cartItems.Select(x => new Basket
            {
                OrderId = Order.OrderId,
                ProductID = x.Product.ProductID,
                Price = x.Product.UnitPrice,
                Quantity = x.Quantity
            }).ToList();

            foreach (var basket in Basket)
            {
                _basketRepository.Add(basket);
              await  _basketRepository.SaveChangesAsync();
            }
            Options options = new Options
            {
                ApiKey = "sandbox-7TreZkKaeEDhVpzAepDV3cFkCvSSISTl",
                SecretKey = "sandbox-AmUtYs5RgBbZYvacQJhjOHNxyP6JsuZi",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };

            CreatePaymentRequest request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = Order.OrderId.ToString(),
                Price = iyzicoModel.Price,
                PaidPrice = iyzicoModel.Price, //indirim filan varsa
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = Order.OrderId.ToString(),
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString()
            };
            PaymentCard paymentCard = new PaymentCard
            {
                CardHolderName = iyzicoModel.CardHolderName,
                CardNumber = iyzicoModel.CardNumber.Trim().Replace(" ",string.Empty),
                ExpireMonth = iyzicoModel.ExpireMonth,
                ExpireYear = iyzicoModel.ExpireYear,
                Cvc = iyzicoModel.Cvc,
                RegisterCard = iyzicoModel.RegisterCard,
            };
            request.PaymentCard = paymentCard;
            ComputerInfo computerInfo = new ComputerInfo();
            
            Buyer buyer = new Buyer()
            {
                Id = "1",
                Name = iyzicoModel.Name,
                Surname = iyzicoModel.Surname,
                Email = iyzicoModel.Email,
                IdentityNumber = iyzicoModel.IdentityNumber,
                RegistrationAddress = iyzicoModel.RegistrationAddress,
                Ip = computerInfo.ComputerIPAdress,
                City = iyzicoModel.City,
                Country = iyzicoModel.Country,
            };
            request.Buyer = buyer;

            Address shippingAddress = new Address()
            {
                ContactName = iyzicoModel.Name,
                City = iyzicoModel.City,
                Country = iyzicoModel.Country,
                Description = iyzicoModel.RegistrationAddress,
            };
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address()
            {

                ContactName = iyzicoModel.Name,
                City = iyzicoModel.City,
                Country = iyzicoModel.Country,
                Description = iyzicoModel.RegistrationAddress,
            };
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();

            foreach (var item in iyzicoModel.cartItems)
            {
                BasketItem basketItem = new BasketItem
                {
                    Id = item.Product.ProductID.ToString(),
                    Category1 = item.Product.SubCategoryId.ToString(),
                    Name = item.Product.ProductName,
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (item.Product.UnitPrice * item.Quantity).ToString()
                };
                basketItems.Add(basketItem);

            }

            request.BasketItems = basketItems;

            Payment payment = Payment.Create(request, options);
            if (payment.Status == "success")
            {
                foreach (var item in Order.Baskets)
                {
                    item.OrderId = Order.OrderId;
                }
                Order.Status = 1;
                Order.Massage = "Sipariş Alındı";
                _orderRepository.Update(Order);
               await _orderRepository.SaveChangesAsync();
                return new SuccessResult(Messages.PaySuccess);
            }
            else
            {
                foreach (var item in Order.Baskets)
                {
                    item.OrderId = Order.OrderId;
                }
                Order.Status = 0;
                Order.Massage = payment.ErrorMessage;
                _orderRepository.Update(Order);
                await _orderRepository.SaveChangesAsync();
                return new ErrorResult(payment.ErrorMessage);
            }

        }

    }
}
