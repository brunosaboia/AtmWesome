using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coinify.Web.Models;

namespace Coinify.Web.Controllers
{
    public class CoinSizesController : Controller
    {
        private readonly CoinifyWebContext _context;

        public CoinSizesController(CoinifyWebContext context)
        {
            _context = context;    
        }

        // GET: CoinSizes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CoinSize.ToListAsync());
        }

        // GET: CoinSizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinSize = await _context.CoinSize
                .SingleOrDefaultAsync(m => m.CoinSizeId == id);
            if (coinSize == null)
            {
                return NotFound();
            }

            return View(coinSize);
        }

        // GET: CoinSizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoinSizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoinSizeId,Size")] CoinSize coinSize)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coinSize);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(coinSize);
        }

        // GET: CoinSizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinSize = await _context.CoinSize.SingleOrDefaultAsync(m => m.CoinSizeId == id);
            if (coinSize == null)
            {
                return NotFound();
            }
            return View(coinSize);
        }

        // POST: CoinSizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CoinSizeId,Size")] CoinSize coinSize)
        {
            if (id != coinSize.CoinSizeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coinSize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoinSizeExists(coinSize.CoinSizeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(coinSize);
        }

        // GET: CoinSizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinSize = await _context.CoinSize
                .SingleOrDefaultAsync(m => m.CoinSizeId == id);
            if (coinSize == null)
            {
                return NotFound();
            }

            return View(coinSize);
        }

        // POST: CoinSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coinSize = await _context.CoinSize.SingleOrDefaultAsync(m => m.CoinSizeId == id);
            _context.CoinSize.Remove(coinSize);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CoinSizeExists(int id)
        {
            return _context.CoinSize.Any(e => e.CoinSizeId == id);
        }
    }
}
