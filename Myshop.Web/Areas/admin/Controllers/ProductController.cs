using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Myshop.Entities.Models;
using Myshop.Entities.Repositories;
using Myshop.Entities.ViewModels;

namespace Myshop.Web.Areas.admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfwork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

      
        public IActionResult Index()
        {
            //var products = _unitOfwork.Product.GetAll();
            return View();
        }
		public IActionResult GetData()
		{
			var products = _unitOfwork.Product.GetAll(IncludeWord:"Category");
			return Json(new { data = products });
		}
		[HttpGet]
        public IActionResult Create()
        {
            ProductVm productvm = new ProductVm()
            {
                    Product = new Product(),
                    CategoryList = _unitOfwork.Category.GetAll().Select(x => new SelectListItem
                    {
                      Text = x.Name,
                      Value = x.Id.ToString()
                    })
                    
            };

            return View(productvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVm productVM , IFormFile file)
        {
           
           if(ModelState.IsValid)
            {
				string RootPath = _webHostEnvironment.WebRootPath; //points at wwwroot
				if (file != null)
				{
					string filename = Guid.NewGuid().ToString();
					var upload = Path.Combine(RootPath, @"Images\Product");
					var ext = Path.GetExtension(file.FileName);
					using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
					{
						file.CopyTo(filestream);
					}
					productVM.Product.Img = @"Images\Product\" + filename + ext;
				}
				_unitOfwork.Product.Add(productVM.Product);
                _unitOfwork.complete();
                TempData["Create"] = "New Product Added Successfully";
                return RedirectToAction("Index", "Product");
            }
           return View(productVM);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if( id == 0)
            {
                NotFound();
            }

            ProductVm productVm = new ProductVm()
            {
                Product = _unitOfwork.Product.GetById(X => X.Id == id),
                CategoryList = _unitOfwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })

            };
            return View(productVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVm productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
				string RootPath = _webHostEnvironment.WebRootPath; //points at wwwroot
				if (file != null)
				{
					string filename = Guid.NewGuid().ToString();
					var upload = Path.Combine(RootPath, @"Images\Product");
					var ext = Path.GetExtension(file.FileName);

                    if ( productVM.Product.Img != null)
                    {
                        var oldimg = Path.Combine(RootPath, productVM.Product.Img.TrimStart('/'));
                        if(System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }
					using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
					{
						file.CopyTo(filestream);
					}
					productVM.Product.Img = @"Images\Product\" + filename + ext;
				}
				_unitOfwork.Product.update(productVM.Product);
                _unitOfwork.complete();
                TempData["Update"] = "Product Has been updated successfully";
                return RedirectToAction("Index", "Product");
            }
            return View(productVM);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                NotFound();
            }
            ProductVm productVm = new ProductVm()
            {
                Product = _unitOfwork.Product.GetById(X => X.Id == id),
                CategoryList = _unitOfwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })

            };
            return View(productVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var DeletedProduct = _unitOfwork.Product.GetById(x => x.Id == id);
            if (id == null)
            {
                NotFound();
             //  return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfwork.Product.Remove(DeletedProduct);
			var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, DeletedProduct.Img.TrimStart('/'));
			if (System.IO.File.Exists(oldimg))
			{
				System.IO.File.Delete(oldimg);
			}

			_unitOfwork.complete();
            //return Json(new { success = true, message = "Product deleted successfully" });
            TempData["Delete"] = "Product has been deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
