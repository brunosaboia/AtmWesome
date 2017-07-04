using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coinify.Web.Models;
using Coinify.Web.Models.ViewModels;
using System.Collections.Generic;

namespace Coinify.Web.Controllers
{
    public class AutomatedTellerMachinesController : Controller
    {
        private readonly CoinifyWebContext _context;

        public AutomatedTellerMachinesController(CoinifyWebContext context)
        {
            _context = context;    
        }

        // GET: AutomatedTellerMachines
        public async Task<IActionResult> Index()
        {
            return View(await _context.AutomatedTellerMachine.ToListAsync());
        }

        // GET: AutomatedTellerMachines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var automatedTellerMachine = await _context.AutomatedTellerMachine
                .SingleOrDefaultAsync(m => m.AutomatedTellerMachineId == id);
            if (automatedTellerMachine == null)
            {
                return NotFound();
            }

            return View(automatedTellerMachine);
        }

        // GET: AutomatedTellerMachines/Create
        public IActionResult Create()
        {
            ViewBag.CoinDictionary = _context.Coin.Include(c => c.Size).ToDictionary(k => k, v => 0);
            ViewBag.NoteDictionary = _context.Note.ToDictionary(k => k, v => 0);
            ViewBag.CoinDispenserDictionary = _context.CoinSize.ToDictionary(k => k, v => false);

            return View();
        }

        // POST: AutomatedTellerMachines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutomatedTellerMachineViewModel automatedTellerMachine)
        {
            if (ModelState.IsValid)
            {
                var model = automatedTellerMachine.ToModel(_context);

                // We should not add any notes or coins if the ATM has not a dispenser
                // to spit them out
                if(!model.HasNoteDispenser)
                {
                    model.NoteDictionary = new Dictionary<Note, int>();
                }

                foreach(var kvp in model.CoinDictionary.ToList())
                {
                    if(model.CoinDispensersDictionary.ContainsKey(kvp.Key.Size))
                    {
                        if(!model.CoinDispensersDictionary[kvp.Key.Size])
                        {
                            model.CoinDictionary[kvp.Key] = 0;
                        }
                    }
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(automatedTellerMachine);
        }

        // GET: AutomatedTellerMachines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var automatedTellerMachine = await _context.AutomatedTellerMachine.SingleOrDefaultAsync(m => m.AutomatedTellerMachineId == id);
            if (automatedTellerMachine == null)
            {
                return NotFound();
            }
            return View(automatedTellerMachine);
        }

        // POST: AutomatedTellerMachines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AutomatedTellerMachine automatedTellerMachine)
        {
            if (id != automatedTellerMachine.AutomatedTellerMachineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(automatedTellerMachine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutomatedTellerMachineExists(automatedTellerMachine.AutomatedTellerMachineId))
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
            return View(automatedTellerMachine);
        }

        // GET: AutomatedTellerMachines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var automatedTellerMachine = await _context.AutomatedTellerMachine
                .SingleOrDefaultAsync(m => m.AutomatedTellerMachineId == id);
            if (automatedTellerMachine == null)
            {
                return NotFound();
            }

            return View(automatedTellerMachine);
        }

        // POST: AutomatedTellerMachines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var automatedTellerMachine = await _context.AutomatedTellerMachine.SingleOrDefaultAsync(m => m.AutomatedTellerMachineId == id);
            _context.AutomatedTellerMachine.Remove(automatedTellerMachine);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AutomatedTellerMachineExists(int id)
        {
            return _context.AutomatedTellerMachine.Any(e => e.AutomatedTellerMachineId == id);
        }
    }
}