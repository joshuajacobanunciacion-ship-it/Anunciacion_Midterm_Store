using Microsoft.AspNetCore.Mvc;
using Anunciacion_Midterm_Store.Data;
using Anunciacion_Midterm_Store.Models;

namespace Anunciacion_Midterm_Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var products = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var search = searchString.Trim().ToLower();
                products = products.Where(p => p.Name.ToLower().Contains(search));
            }

            return View(products.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null) return RedirectToAction("Index");
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (product != null)
            {
                
                _db.CartItems.RemoveRange(_db.CartItems.Where(c => c.ProductId == id));
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}