using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class CarSalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarSalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarSales
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarSales.ToListAsync());
        }

        // GET: CarSales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSale = await _context.CarSales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carSale == null)
            {
                return NotFound();
            }

            return View(carSale);
        }

        // GET: CarSales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarSales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarId,PurchaseDate,PurchasePrice,AvailableForSaleDate,SalePrice,SaleDate")] CarSale carSale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carSale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carSale);
        }

        // GET: CarSales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSale = await _context.CarSales.FindAsync(id);
            if (carSale == null)
            {
                return NotFound();
            }
            return View(carSale);
        }

        // POST: CarSales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,PurchaseDate,PurchasePrice,AvailableForSaleDate,SalePrice,SaleDate")] CarSale carSale)
        {
            if (id != carSale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carSale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarSaleExists(carSale.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carSale);
        }

        // GET: CarSales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSale = await _context.CarSales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carSale == null)
            {
                return NotFound();
            }

            return View(carSale);
        }

        // POST: CarSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carSale = await _context.CarSales.FindAsync(id);
            if (carSale != null)
            {
                _context.CarSales.Remove(carSale);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarSaleExists(int id)
        {
            return _context.CarSales.Any(e => e.Id == id);
        }
    }
}
