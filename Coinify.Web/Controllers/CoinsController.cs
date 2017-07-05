using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coinify.Web.Models;
using Coinify.Web.Models.ViewModels;

namespace Coinify.Web.Controllers
{
    public class CoinsController : Controller
    {
        private readonly CoinifyWebContext _context;

        public CoinsController(CoinifyWebContext context)
        {
            _context = context;
        }
        private async Task<IEnumerable<SelectListItem>> GetCoinSizes()
        {
            var sizes = await _context.CoinSize.ToListAsync();

            var model = sizes.Select(s =>
                new SelectListItem
                {
                    Value = s.CoinSizeId.ToString(),
                    Text = s.Size.ToString(),
                });

            return new SelectList(model, "Value", "Text");
        }

        // GET: Coins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coin.Include(c => c.Size).ToListAsync());
        }

        // GET: Coins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coin = await _context.Coin
                .Include(c => c.Size)
                .SingleOrDefaultAsync(m => m.CoinId == id);
            if (coin == null)
            {
                return NotFound();
            }

            return View(coin);
        }

        // GET: Coins/Create
        public IActionResult Create()
        {
            ViewBag.CoinSizes = GetCoinSizes().Result;

            return View();
        }

        // POST: Coins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoinId,Value,Size")] CoinViewModel coin)
        {
            if (ModelState.IsValid)
            {
                var size = _context.CoinSize.Find(coin.Size);
                var model = new Coin()
                {
                    CoinId = coin.CoinId,
                    Value = coin.Value,
                    Size = size
                };

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Coin created sucessfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.CoinSizes = await GetCoinSizes();
            }
            return View(coin);
        }

        // GET: Coins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coin = await _context.Coin.SingleOrDefaultAsync(m => m.CoinId == id);
            if (coin == null)
            {
                return NotFound();
            }
            ViewBag.CoinSizes = await GetCoinSizes();

            return View(CoinViewModel.FromModel(coin));
        }

        // POST: Coins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CoinId,Value,Size")] CoinViewModel coin)
        {
            if (id != coin.CoinId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var model = _context.Coin.Find(coin.CoinId);
                    var size = _context.CoinSize.Find(coin.Size);

                    model.Value = coin.Value;
                    model.Size = size;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoinExists(coin.CoinId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Coin edited sucessfully";
                return RedirectToAction("Index");
            }

            return View(coin);
        }

        // GET: Coins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coin = await _context.Coin
                .Include(c => c.Size)
                .SingleOrDefaultAsync(m => m.CoinId == id);
            if (coin == null)
            {
                return NotFound();
            }

            return View(coin);
        }

        // POST: Coins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coin = await _context.Coin.SingleOrDefaultAsync(m => m.CoinId == id);
            _context.Coin.Remove(coin);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Coin deleted sucessfully";
            return RedirectToAction("Index");
        }

        private bool CoinExists(int id)
        {
            return _context.Coin.Any(e => e.CoinId == id);
        }
    }
}
