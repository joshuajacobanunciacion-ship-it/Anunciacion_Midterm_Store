using Microsoft.AspNetCore.Mvc;
using Anunciacion_Midterm_Store.Data;
using Anunciacion_Midterm_Store.Models;

namespace Anunciacion_Midterm_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index()
        {
            return View(_db.CartItems.ToList());
        }

        [HttpPost]
        public IActionResult Add(int productId)
        {
            var product = _db.Products.Find(productId);
            if (product == null) return RedirectToAction("Index", "Products");

            var item = _db.CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item == null)
            {
                _db.CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;   
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var item = _db.CartItems.Find(id);
            if (item != null)
            {
                item.Quantity = quantity < 1 ? 1 : quantity;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var item = _db.CartItems.Find(id);
            if (item != null)
            {
                _db.CartItems.Remove(item);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}