using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Entities.Models;
using Myshop.Entities.Repositories;
using Myshop.Entities.ViewModels;
using System.Security.Claims;
using Utilities;

namespace Myshop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        //public ShoppingCartVM shoppingCartVM { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value,IncludeWord:"Product")
            };

            foreach (var item in shoppingCartVM.CartsList)
            {
                shoppingCartVM.TotalCarts += (item.count * item.Product.Price);
            }
			return View(shoppingCartVM);
        }
        public IActionResult Plus(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetById(x => x.Id == cartid);
            _unitOfWork.ShoppingCart.IncreaseCount(shoppingcart,1);
            _unitOfWork.complete();
            return RedirectToAction("Index");
        }
        public IActionResult Subtract(int cartid)
        {
			var shoppingcart = _unitOfWork.ShoppingCart.GetById(x => x.Id == cartid);
            if(shoppingcart.count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingcart);
				_unitOfWork.complete();
				return RedirectToAction("Index","Home");
            }
            else
            {
				_unitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);
			}
		
			_unitOfWork.complete();
			return RedirectToAction("Index");
		}
        public IActionResult Remove(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetById(x => x.Id == cartid);
            if(shoppingcart.count >= 1)
            {
				_unitOfWork.ShoppingCart.Remove(shoppingcart);
				_unitOfWork.complete();
				return RedirectToAction("Index");

			}
            else
            {
                return RedirectToAction("Index","Home");
            }

		}

        [HttpGet]
        public IActionResult Summary()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value, IncludeWord:"Product"),
                orderHeader = new()
            };

            shoppingCartVM.orderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetById(x => x.Id == claim.Value);
            shoppingCartVM.orderHeader.Name = shoppingCartVM.orderHeader.ApplicationUser.Name;
            shoppingCartVM.orderHeader.Address = shoppingCartVM.orderHeader.ApplicationUser.Address;
            shoppingCartVM.orderHeader.AddressCity = shoppingCartVM.orderHeader.ApplicationUser.city;
            shoppingCartVM.orderHeader.PhoneNumber = shoppingCartVM.orderHeader.ApplicationUser.PhoneNumber;
			

            foreach(var item in shoppingCartVM.CartsList)
            {
                shoppingCartVM.orderHeader.TotalPrice += (item.count * item.Product.Price);
            }

            return View(shoppingCartVM);
        }

       
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult POSTSummary(ShoppingCartVM ShoppingCartVM)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM.CartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "Product");


			ShoppingCartVM.orderHeader.OrderStatus = SD.Pending;
			ShoppingCartVM.orderHeader.PaymentStatus = SD.Pending;
			ShoppingCartVM.orderHeader.OrderDate = DateTime.Now;
			ShoppingCartVM.orderHeader.ApplicationUserId = claim.Value;


			foreach (var item in ShoppingCartVM.CartsList)
			{
				ShoppingCartVM.orderHeader.TotalPrice += (item.count * item.Product.Price);
			}

			_unitOfWork.OrderHeader.Add(ShoppingCartVM.orderHeader);
			_unitOfWork.complete();

			foreach (var item in ShoppingCartVM.CartsList)
			{
				OrderDetail orderDetail = new OrderDetail()
				{
					ProductId = item.ProductId,
					orderHeaderId = ShoppingCartVM.orderHeader.Id,
					price = item.Product.Price,
					count = item.count
				};

				_unitOfWork.OrderDetail.Add(orderDetail);
			}
			_unitOfWork.complete();
			return RedirectToAction("Index", "Home");

			//var domain = "https://localhost:7020/";
			//var options = new SessionCreateOptions
			//{
			//	LineItems = new List<SessionLineItemOptions>(),

			//	Mode = "payment",
			//	SuccessUrl = domain + $"customer/cart/orderconfirmation?id={ShoppingCartVM.OrderHeader.Id}",
			//	CancelUrl = domain + $"customer/cart/index",
			//};

			//foreach (var item in ShoppingCartVM.CartsList)
			//{
			//	var sessionlineoption = new SessionLineItemOptions
			//	{
			//		PriceData = new SessionLineItemPriceDataOptions
			//		{
			//			UnitAmount = (long)(item.Product.Price * 100),
			//			Currency = "usd",
			//			ProductData = new SessionLineItemPriceDataProductDataOptions
			//			{
			//				Name = item.Product.Name,
			//			},
			//		},
			//		Quantity = item.Count,
			//	};
			//	options.LineItems.Add(sessionlineoption);
			//}


			//var service = new SessionService();
			//Session session = service.Create(options);
			//ShoppingCartVM.OrderHeader.SessionId = session.Id;

			//_unitOfWork.Complete();

			//Response.Headers.Add("Location", session.Url);
			//return new StatusCodeResult(303);

			//_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.CartsList);
			//         _unitOfWork.Complete();
			//         return RedirectToAction("Index","Home");

		}

		//public IActionResult OrderConfirmation(int id)
		//{
		//	OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstorDefault(u => u.Id == id);
		//	var service = new SessionService();
		//	Session session = service.Get(orderHeader.SessionId);

		//	if (session.PaymentStatus.ToLower() == "paid")
		//	{
		//		_unitOfWork.OrderHeader.UpdateStatus(id, SD.Approve, SD.Approve);
		//		orderHeader.PaymentIntentId = session.PaymentIntentId;
		//		_unitOfWork.Complete();
		//	}
		//	List<ShoppingCart> shoppingcarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
		//	HttpContext.Session.Clear();
		//	_unitOfWork.ShoppingCart.RemoveRange(shoppingcarts);
		//	_unitOfWork.Complete();
		//	return View(id);
		//}
	}

}
