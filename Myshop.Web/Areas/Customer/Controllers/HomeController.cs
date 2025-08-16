using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.DataAccess.Implementation;
using Myshop.Entities.Models;
using Myshop.Entities.Repositories;
using System.Security.Claims;

namespace Myshop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var product = _unitOfWork.Product.GetAll();
            return View(product);
        }

        [HttpGet]
        public IActionResult Details(int productId)
        {

            ShoppingCart newcart = new ShoppingCart()
            {
                ProductId = productId,
                Product = _unitOfWork.Product.GetById(x => x.Id == productId, IncludeWord:"Category"),
                count = 1
            };
            //var Product = _unitOfWork.Product.GetById(x => x.Id == id, IncludeWord:"Category");
            //ViewBag.Count = 1;
            return View(newcart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart CurrentCart = _unitOfWork.ShoppingCart.GetById(x => x.ApplicationUserId == claim.Value
            && x.ProductId == shoppingCart.ProductId) ;
            if (CurrentCart == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(CurrentCart, shoppingCart.count);
            }

            _unitOfWork.complete();

            return RedirectToAction("Index");

        }
    }
}
