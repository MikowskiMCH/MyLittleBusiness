using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLittleBusiness.Models;

namespace MyLittleBusiness.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MyLittleBusinessContext _context;

        public ClientsController(MyLittleBusinessContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var clients = _context.Clients.AsQueryable();
            ViewData["CurrentSort"] = sortOrder;
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
                case ("name_desc"):
                    clients = clients.OrderByDescending(f => f.ClientName).AsQueryable();
                    break;

                default:
                    clients = clients.OrderBy(f => f.ClientName).AsQueryable();
                    break;
            }

            int pageSize = 5;

              return View(await PaginatedList<Client>.CreateAsync(clients, pageNumber ?? 1, pageSize));
        }

        // GET: Clients/Details
        public async Task<IActionResult> Details(int? id)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == id);
            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View(new Client());
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            _context.Add(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Clients/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            var client = await _context.Clients.FindAsync(id);
            return View(client);
        }

        // POST: Clients/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            _context.Update(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Clients/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == id);
            return View(client);
        }

        // POST: Clients/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // Redirect to list of factures for choosen client
        public IActionResult Factures(int? id)
        {
            return RedirectToAction("Index", "Factures", new { id });
        }
    }
}
