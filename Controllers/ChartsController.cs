using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using MyLittleBusiness.Models;
using System.Linq;

namespace MyLittleBusiness.Controllers
{
    public class ChartsController : Controller
    {
        private readonly MyLittleBusinessContext _context;

        public ChartsController(MyLittleBusinessContext context)
        {
            _context = context;
        }
        
        public IActionResult SalesChart()
        {
            var factures = _context.Factures.Include(f=>f.Client).Include(f=>f.FactureHasItems);

            Dictionary<string, decimal> data = new Dictionary<string, decimal>();

            foreach(var facture in factures)
            {
                if (data.ContainsKey(facture.Client.ClientName))
                {
                    data[key: facture.Client.ClientName] += facture.FactureHasItems.Where(f => f.FactureId == facture.FactureId).Sum(f => f.PriceGross);
                }
                else
                {
                    data.Add(facture.Client.ClientName, facture.FactureHasItems.Where(f => f.FactureId == facture.FactureId).Sum(f => f.PriceGross));
                }
            }

            var SoldItems = _context.Factures.Include(f => f.FactureHasItems).ThenInclude(f => f.Item).Include(f => f.Client);

            Dictionary<string, decimal> data2 = new Dictionary<string, decimal>();

            foreach (var facture in SoldItems)
            {
                foreach (var item in facture.FactureHasItems)
                {
                    if (data2.Count() != 0)
                    {
                        if (data2.ContainsKey(item.Item.Name))
                        {
                            data2[key: item.Item.Name] += item.PriceGross;
                        }
                        else
                        {
                            data2.Add(item.Item.Name, item.PriceGross);
                        }
                    }
                    else
                    {
                        data2.Add(item.Item.Name, item.PriceGross);
                    }
                }
            }
            ViewData["data"] = data;
            ViewData["data2"] = data2;
            return View();
        }
    }
}
