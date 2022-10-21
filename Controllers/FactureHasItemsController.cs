using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLittleBusiness.Models;

namespace MyLittleBusiness.Controllers
{
    public class FactureHasItemsController : Controller
    {
        private readonly MyLittleBusinessContext _context;

        public FactureHasItemsController(MyLittleBusinessContext context)
        {
            _context = context;
        }

        // GET: FactureHasItems
        public async Task<IActionResult> Index(int id, string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["factureId"] = id;
            int clientId = _context.Factures.Where(m => m.FactureId == id).Select(m => m.ClientId).FirstOrDefault();
            ViewData["clientId"] = clientId;
            var factureHasitems = _context.FactureHasItems.Where(f => f.FactureId == id).Include(f => f.Facture).Include(f => f.Item).AsQueryable();
            ViewData["AmountSort"] = String.IsNullOrEmpty(sortOrder) ? "amount_desc" : "";
            ViewData["PriceSort"] = "Price" == sortOrder ? "price_desc" : "Price";
            ViewData["NameSort"] = "Name" == sortOrder ? "name_desc" : "Name";
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

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
                case ("amount_desc"):
                    factureHasitems = factureHasitems.OrderByDescending(f => f.Amount).AsQueryable();
                    break;

                case ("price_desc"):
                    factureHasitems = factureHasitems.OrderByDescending(f => f.PriceGross).AsQueryable();
                    break;

                case ("Price"):
                    factureHasitems = factureHasitems.OrderBy(f => f.PriceGross).AsQueryable();
                    break;

                case ("name_desc"):
                    factureHasitems = factureHasitems.OrderByDescending(f => f.Item.Name).AsQueryable();
                    break;

                case ("Name"):
                    factureHasitems = factureHasitems.OrderBy(f => f.Item.Name).AsQueryable();
                    break;

                default:
                    factureHasitems = factureHasitems.OrderBy(f => f.Amount).AsQueryable();
                    break;
            }

            int pageSize = 5;

            return View(await PaginatedList<FactureHasItem>.CreateAsync(factureHasitems, pageNumber ?? 1, pageSize));
        }

        // GET: FactureHasItems/Details
        public async Task<IActionResult> Details(int? id)
        {var factureHasItem = await _context.FactureHasItems
                .Include(f => f.Facture)
                .Include(f => f.Item)
                .FirstOrDefaultAsync(m => m.FactureHasItemId == id);
            return View(factureHasItem);
        }

        // GET: FactureHasItems/Create
        public IActionResult Create(int id)
        {
            ViewBag.FactureId = new SelectList(_context.Factures, "FactureId", "FactureId");
            ViewBag.ItemId = new SelectList(_context.Items, "ItemId", "Name");
            var x = new FactureHasItem();
            x.FactureId = id;
            return View(x);
        }

        // POST: FactureHasItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FactureHasItem factureHasItem)
        {
            factureHasItem.PriceNetto = _context.Items.FirstOrDefault(x => x.ItemId == factureHasItem.ItemId).PriceNetto * factureHasItem.Amount;
            factureHasItem.PriceGross = _context.Items.FirstOrDefault(x => x.ItemId == factureHasItem.ItemId).PriceGross * factureHasItem.Amount;
            _context.Add(factureHasItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "FactureHasItems", new { @id = factureHasItem.FactureId });
        }

        // GET: FactureHasItems/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            var factureHasItem = await _context.FactureHasItems
                .Include(f => f.Facture)
                .Include(f => f.Item)
                .FirstOrDefaultAsync(m => m.FactureHasItemId == id);
            return View(factureHasItem);
        }

        // POST: FactureHasItems/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factureHasItem = await _context.FactureHasItems.FindAsync(id);
            _context.FactureHasItems.Remove(factureHasItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "FactureHasItems", new { @id = factureHasItem.FactureId });
        }

        // Go back to Facture
        public IActionResult Back(int? id)
        {
            return RedirectToAction("Index", "Factures", new {id});
        }

        private bool FactureHasItemExists(int id)
        {
          return _context.FactureHasItems.Any(e => e.FactureHasItemId == id);
        }
    }
}
