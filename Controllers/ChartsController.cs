using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLittleBusiness.Models;

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
            var factures = _context.Factures.Include(f=>f.Client).Include(f=>f.FactureHasItems).ThenInclude(f=>f.Item);

            ViewData["data"] = GetTop5Clients();

            ViewData["data2"] = GetAllSoldItems();

            ViewData["data3"] = DailyIncomeBrutto();

            ViewData["data4"] = MonthlyIncomeBrutto();

            ViewData["data5"] = YearlyIncomeBrutto();

            ViewData["data6"] = GetYearlyIncByMonths();

            return View();

        }

        public Dictionary<string, decimal> GetTop5Clients()
        {
            Dictionary<string, decimal> data = new Dictionary<string, decimal>();

            var factures = _context.Factures.Include(f => f.Client).Include(f => f.FactureHasItems).ThenInclude(f => f.Item);

            foreach (var facture in factures)
            {
                var tmp = facture.FactureHasItems.Where(f => f.FactureId == facture.FactureId).Sum(f => f.PriceGross);
                if (data.ContainsKey(facture.Client.ClientName))
                {
                    data[key: facture.Client.ClientName] += tmp;
                }
                else
                {
                    data.Add(facture.Client.ClientName, tmp);
                }
            }
            return data.OrderByDescending(f => f.Value).Take(5).ToDictionary(f => f.Key, f => f.Value);
        }

        public Dictionary<string, decimal> GetAllSoldItems()
        {
            Dictionary<string, decimal> data2 = new Dictionary<string, decimal>();

            var factures = _context.Factures.Include(f => f.Client).Include(f => f.FactureHasItems).ThenInclude(f => f.Item);

            foreach (var facture in factures)
            {
                foreach (var item in facture.FactureHasItems)
                {
                    if (data2.Count() != 0)
                    {
                        if (data2.ContainsKey(item.Item.Name))
                        {
                            data2[key: item.Item.Name] += item.Amount;
                        }
                        else
                        {
                            data2.Add(item.Item.Name, item.Amount);
                        }
                    }
                    else
                    {
                        data2.Add(item.Item.Name, item.Amount);
                    }
                }
            }

            return data2;
        }

        public Dictionary<int, decimal> GetYearlyIncByMonths()
        {
            Dictionary<int, decimal> data = new Dictionary<int, decimal>();

            for (int i = 1; i <= 12; i++)
            {
                var tmp = _context.Factures
                .Where(f => f.Date.Year == DateTime.Today.Year)
                .Where(f => f.Date.Month == i)
                .Include(f => f.FactureHasItems)
                .Select(f => f.FactureHasItems
                .Sum(f => f.PriceGross))
                .AsEnumerable()
                .Sum();
                data.Add(i, tmp);
            }

            return data;
        }

        public decimal DailyIncomeBrutto()
        {
            decimal data = _context.Factures
                .Where(f => f.Date.Date == DateTime.Today)
                .Include(f => f.FactureHasItems)
                .Select(f => f.FactureHasItems
                .Sum(f => f.PriceGross))
                .AsEnumerable()
                .Sum();

            return data;
        }

        public decimal MonthlyIncomeBrutto()
        {
            decimal data = _context.Factures
                .Where(f => f.Date.Year == DateTime.Today.Year)
                .Where(f => f.Date.Month == DateTime.Today.Month)
                .Include(f => f.FactureHasItems)
                .Select(f => f.FactureHasItems
                .Sum(f => f.PriceGross))
                .AsEnumerable()
                .Sum();

            return data;
        }

        public decimal YearlyIncomeBrutto()
        {
            decimal data = _context.Factures
                .Where(f => f.Date.Year == DateTime.Today.Year)
                .Include(f => f.FactureHasItems)
                .Select(f => f.FactureHasItems
                .Sum(f => f.PriceGross))
                .AsEnumerable()
                .Sum();

            return data;
        }


    }
}
