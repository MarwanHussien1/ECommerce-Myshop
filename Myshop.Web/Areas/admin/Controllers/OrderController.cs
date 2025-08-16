using Microsoft.AspNetCore.Mvc;
using Myshop.Entities.Models;
using Myshop.Entities.ViewModels;
using Myshop.Entities.Repositories;
using Myshop.DataAccess.Implementation;
using Utilities;

namespace Myshop.Web.Areas.admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetData()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(IncludeWord: "ApplicationUser");
            return Json(new { data = orderHeaders });
        }
        public IActionResult Details(int orderid)
        {
            OrderVM orderVm = new OrderVM()
            {
                orderHeader = _unitOfWork.OrderHeader.GetById(x => x.Id ==  orderid,IncludeWord:"ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetail.GetAll(x => x.orderHeader.Id == orderid , IncludeWord:"Product")

            };
            return View(orderVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails()
        {
            var orderfromdb = _unitOfWork.OrderHeader.GetById(x => x.Id == OrderVM.orderHeader.Id);
            orderfromdb.Name = OrderVM.orderHeader.Name;
            orderfromdb.PhoneNumber = OrderVM.orderHeader.PhoneNumber;
            orderfromdb.Address = OrderVM.orderHeader.Address;
            orderfromdb.AddressCity = OrderVM.orderHeader.AddressCity;

            if(OrderVM.orderHeader.Carrier != null)
            {
                orderfromdb.Carrier = OrderVM.orderHeader.Carrier;
            }
            if(OrderVM.orderHeader.TrackingNumber != null)
            {
                orderfromdb.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            }
            orderfromdb.OrderStatus = SD.Approve;
            _unitOfWork.OrderHeader.Update(orderfromdb);
            _unitOfWork.complete();
            TempData["Update"] = "Data Updated Successfully";


            return RedirectToAction("Details","Order",new { orderid = orderfromdb.Id} );
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProccess()
		{
			_unitOfWork.OrderHeader.UpdateOrderStatus(OrderVM.orderHeader.Id, SD.Proccessing, null);
			_unitOfWork.complete();

			TempData["Update"] = "Order Status has Updated Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVM.orderHeader.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{
			var orderfromdb = _unitOfWork.OrderHeader.GetById(u => u.Id == OrderVM.orderHeader.Id);
			orderfromdb.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
			orderfromdb.Carrier = OrderVM.orderHeader.Carrier;
			orderfromdb.OrderStatus = SD.Shipped;
			orderfromdb.ShippingDate = DateTime.Now;

			_unitOfWork.OrderHeader.Update(orderfromdb);
			_unitOfWork.complete();

			TempData["Update"] = "Order has Shipped Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVM.orderHeader.Id });
		}


	}
}
