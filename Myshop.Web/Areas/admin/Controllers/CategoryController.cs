using Microsoft.AspNetCore.Mvc;
using Myshop.DataAccess.data;
using Myshop.Entities.Models;
using Myshop.Entities.Repositories;

namespace Myshop.Web.Areas.admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;
        //public CategoryController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly  IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var category = _unitOfWork.Category.GetAll();
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.complete();
                TempData["Create"] = "Category has been created successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                return NotFound();
            }
            // var categoryindb = _context.Categories.Find(id);
            var categoryindb = _unitOfWork.Category.GetById(x=> x.Id == id);
            return View(categoryindb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //   _context.Categories.Update(category);
                _unitOfWork.Category.update(category);
                _unitOfWork.complete();
               // _context.SaveChanges();
                TempData["Update"] = "Category has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                NotFound();
            }
            var categorytobedeleted = _unitOfWork.Category.GetById(x => x.Id == id);
            return View(categorytobedeleted);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var categorytobedeleted = _unitOfWork.Category.GetById(x => x.Id == id);

            if (categorytobedeleted == null)
            {
                NotFound();
            }
            _unitOfWork.Category.Remove(categorytobedeleted);
            _unitOfWork.complete();
            //_context.Categories.Remove(categorytobedeleted);
            //_context.SaveChanges();
            TempData["Delete"] = "Data has been deleted succesfully";
            return RedirectToAction("Index");
        }
    }
}
