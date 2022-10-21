using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLittleBusiness.Models;

namespace MyLittleBusiness.Controllers
{
    public class ItemsController : Controller
    {
        private readonly MyLittleBusinessContext _context;

        public ItemsController(MyLittleBusinessContext context)
        {
            _context = context;
        }
        // GET: Items
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSort"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["CurrentSort"] = sortOrder;

            var items = _context.Items.AsQueryable();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(f => f.Name).AsQueryable();
                    break;

                case "price_desc":
                    items = items.OrderByDescending(f => f.PriceGross).AsQueryable();
                    break;

                case "Price":
                    items = items.OrderBy(f => f.PriceGross).AsQueryable();
                    break;

                default:
                    items = items.OrderBy(f => f.Name).AsQueryable();
                    break;

            }

            int pageSize = 5;

            return View(await PaginatedList<Item>.CreateAsync(items, pageNumber ?? 1, pageSize));
        }

        // GET: Item/Details
        public async Task<IActionResult> Details(int? id)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemId == id);
            return View(item);
        }

        // GET: Item/Create
        public IActionResult Create()
        {
            return View(new Item());
        }

        // POST: Item/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            item.PriceGross = item.PriceNetto + (item.PriceNetto * item.VatValue);
            _context.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Item/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            var item = await _context.Items.FindAsync(id);
            return View(item);
        }

        // POST: Item/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item item)
        {
            item.PriceGross = item.PriceNetto + (item.PriceNetto * item.VatValue);
            _context.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Item/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemId == id);
            return View(item);
        }

        // POST: Item/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
