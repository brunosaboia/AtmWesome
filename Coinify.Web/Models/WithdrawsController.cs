using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Coinify.Web.Models
{
    public class WithdrawsController : Controller
    {
        private readonly CoinifyWebContext _context;

        public WithdrawsController(CoinifyWebContext context)
        {
            _context = context;    
        }

        // GET: Withdraws
        public async Task<IActionResult> Index()
        {
            return View(await _context.Withdraw.ToListAsync());
        }

        // GET: Withdraws/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var withdraw = await _context.Withdraw
                .SingleOrDefaultAsync(m => m.WithdrawId == id);
            if (withdraw == null)
            {
                return NotFound();
            }

            return View(withdraw);
        }

        // GET: Withdraws/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Withdraws/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WithdrawId,WithdrawDate")] Withdraw withdraw)
        {
            if (ModelState.IsValid)
            {
                _context.Add(withdraw);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(withdraw);
        }

        // GET: Withdraws/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var withdraw = await _context.Withdraw.SingleOrDefaultAsync(m => m.WithdrawId == id);
            if (withdraw == null)
            {
                return NotFound();
            }
            return View(withdraw);
        }

        // POST: Withdraws/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WithdrawId,WithdrawDate")] Withdraw withdraw)
        {
            if (id != withdraw.WithdrawId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(withdraw);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WithdrawExists(withdraw.WithdrawId))
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
            return View(withdraw);
        }

        // GET: Withdraws/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var withdraw = await _context.Withdraw
                .SingleOrDefaultAsync(m => m.WithdrawId == id);
            if (withdraw == null)
            {
                return NotFound();
            }

            return View(withdraw);
        }

        // POST: Withdraws/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var withdraw = await _context.Withdraw.SingleOrDefaultAsync(m => m.WithdrawId == id);
            _context.Withdraw.Remove(withdraw);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WithdrawExists(int id)
        {
            return _context.Withdraw.Any(e => e.WithdrawId == id);
        }
    }
}
