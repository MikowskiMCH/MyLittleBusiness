using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using MyLittleBusiness.Models;
using Rotativa.AspNetCore;
using System.Drawing;

namespace MyLittleBusiness.Controllers
{
    public class FacturesController : Controller
    {
        private readonly MyLittleBusinessContext _context;

        public FacturesController(MyLittleBusinessContext context)
        {
            _context = context;
        }
        // GET: Factures
        public async Task<ActionResult> Index(int id, string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["Name"] = _context.Clients.Where(f => f.ClientId == id).Select(f => f.ClientName).AsQueryable().FirstOrDefault();
            ViewData["ClientId"] = id;
            ViewData["SortId"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["CurrentSort"] = sortOrder;

            var factures = _context.Factures.Where(f => f.ClientId == id).Include(f=>f.Client).AsQueryable();

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
                case ("id_desc"):
                    factures = factures.OrderByDescending(f => f.FactureId).AsQueryable();
                    break;

                default:
                    factures = factures.OrderBy(f => f.FactureId).AsQueryable();
                    break;
            }

            int pageSize = 5;

            return View(await PaginatedList<Facture>.CreateAsync(factures, pageNumber ?? 1, pageSize));

        }

        // GET: Factures/Details
        public ActionResult Details(int? id)
        {
            decimal sumBrutto = _context.FactureHasItems.Where(x => x.FactureId == id).Sum(y=> y.PriceGross);
            decimal sumNetto = _context.FactureHasItems.Where(x => x.FactureId == id).Sum(y => y.PriceNetto);
            var items = _context.FactureHasItems.Where(x => x.FactureId == id).Select(x => new { x.Item.Name, x.PriceGross, x.Amount });
            ViewData["sumBrutto"] = sumBrutto;
            ViewData["sumNetto"] = sumNetto;
            ViewBag.items = items;
            var facture = _context.Factures
                .Include(y => y.Client)
                .FirstOrDefault(x => x.FactureId == id);
            return View(facture);
        }

        // GET: Factures/Create
        public ActionResult Create(int id)
        {
            ViewBag.ClientId = new SelectList(_context.Clients.ToList(), "ClientId", "ClientName");
            var facture = new Facture
            {
                ClientId = id
            };
            return View(facture);
        }

        // POST: Factures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Facture facture, int id)
        {
            facture.ClientId = id;
            _context.Add(facture);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Factures", new { @id = facture.ClientId });
        }

        // GET: Factures/Edit
        public ActionResult Edit(int id)
        {
            var facture = _context.Factures.FindAsync(id);
            return View(facture);
        }

        // POST: Factures/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Facture facture)
        {
            _context.Update(facture);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Factures", new { id });
        }

        // GET: Factures/Delete
        public ActionResult Delete(int id)
        {
            var facture = _context.Factures.FindAsync(id);
            return View(facture);
        }

        // POST: Factures/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Facture facture)
        {
            _context.Factures.Remove(facture);
            _context.SaveChangesAsync();
            return RedirectToAction("Index", "Factures", new { id });
        }

        public IActionResult PrintFacture(int id)
        {
            Facture facture = (Facture)_context.Factures.Include(f => f.Client).Include(f => f.FactureHasItems).ThenInclude(f => f.Item).Where(f => f.FactureId == id).FirstOrDefault();
            return new ViewAsPdf(facture)
            {
                FileName = String.Format("Faktura-{0}.pdf", id),
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                ContentDisposition = Rotativa.AspNetCore.Options.ContentDisposition.Inline,
            };
        }

        // Redirect to list of factrures for this client
        public IActionResult FacturesHasItems(int? id)
        {
            return RedirectToAction("Index", "FactureHasItems", new { id });
        }

        // Go back to list of clients
        public IActionResult Back(int? id)
        {
            return RedirectToAction("Index", "Clients", new { id });
        }
    }
}
