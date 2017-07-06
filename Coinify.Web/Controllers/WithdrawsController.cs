using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coinify.Web.Models;
using Coinify.Web.Models.ViewModels;
using Coinify.Web.Exceptions;
using Coinify.Web.Helpers;

namespace Coinify.Web.Controllers
{
    public class WithdrawsController : Controller
    {
        private readonly CoinifyWebContext _context;

        public WithdrawsController(CoinifyWebContext context)
        {
            _context = context;
        }

        public IActionResult Reports()
        {
            return View();
        }

        public async Task<IActionResult> ReportLeastUsedMoney(int? id = null)
        {
            var model = await ReportHelper.GenerateMoneyReport(_context, id);

            return View(model);
        }

        public async Task<IActionResult> ReportWithdrawByAtm(int id)
        {
            var atm = await _context
                .AutomatedTellerMachine
                .Include(a => a.CurrencyDictionary)
                .SingleOrDefaultAsync(a => a.AutomatedTellerMachineId == id);

            ViewBag.Atm = atm.Alias;
            ViewBag.AtmBalance = atm.CurrencyDictionary.Balance;
            ViewBag.AtmId = atm.AutomatedTellerMachineId;

            if (atm == null) return NotFound();

            var model = await _context
                .Withdraw
                .Include(w => w.User)
                .Include(w => w.CurrencyDictionary)
                .Where(w => w.AutomatedTellerMachine.AutomatedTellerMachineId == id)
                .OrderByDescending(w => w.WithdrawDate)
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> ReportWithdrawByAtms()
        {
            var atms = await _context
                .AutomatedTellerMachine
                .ToListAsync();

            return View(atms);
        }

        private async Task<IEnumerable<SelectListItem>> GetUsers()
        {
            var users = await _context.User.ToListAsync();

            var model = users.Select(s =>
                new SelectListItem
                {
                    Value = s.UserId.ToString(),
                    Text = s.Name,
                });

            return new SelectList(model, "Value", "Text");
        }

        private async Task<IEnumerable<SelectListItem>> GetAtms()
        {
            var atms = await _context.AutomatedTellerMachine.ToListAsync();

            var model = atms.Select(s =>
                new SelectListItem
                {
                    Value = s.AutomatedTellerMachineId.ToString(),
                    Text = s.Alias,
                });

            return new SelectList(model, "Value", "Text");
        }

        // GET: Withdraws
        public async Task<IActionResult> Index()
        {
            return View(await _context
                .Withdraw
                .Include(w => w.User)
                .Include(w => w.AutomatedTellerMachine)
                .Include(w => w.CurrencyDictionary)
                .OrderByDescending(w => w.WithdrawDate)
                .ToListAsync());
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
            ViewBag.Users =  GetUsers().Result;
            ViewBag.Atms = GetAtms().Result;

            return View();
        }

        // POST: Withdraws/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create
            ([Bind("WithdrawId,WithdrawDate,Amount,AtmId,UserId")] WithdrawViewModel withdraw)
        {
            if (ModelState.IsValid)
            {
                var model = withdraw.ToModel(_context);

                bool hasError = model.AutomatedTellerMachine == null || model.User == null;

                try
                {
                    model.CurrencyDictionary = WithdrawHelper.WithdrawFromAtm(model.User, model.AutomatedTellerMachine, withdraw.Amount, _context);
                }
                catch (InsufficentFundsException)
                {
                    ModelState.AddModelError("UserId", $"User {model.User.Name} has insufficient funds (${model.User.Balance}) to withdraw ${withdraw.Amount}");
                    hasError = true;
                }
                catch (InsufficientChangeException)
                {
                    ModelState.AddModelError("AtmId", $"ATM {model.AutomatedTellerMachine.Alias} has insufficient change to withdraw ${withdraw.Amount}");
                    hasError = true;
                }
                catch
                {
                    throw;
                }

                if (model.AutomatedTellerMachine == null)
                {
                    ModelState.AddModelError("AtmId", "ATM not found");
                }

                if (model.User == null)
                {
                    ModelState.AddModelError("UserId", "User not found");
                }

                if (hasError)
                {
                    ViewBag.Users = await GetUsers();
                    ViewBag.Atms = await GetAtms();

                    return View(withdraw);
                }

                model.WithdrawDate = DateTime.Now;

                _context.Add(model);
                await _context.SaveChangesAsync();

                (TempData["SuccessMessage"], TempData["InfoMessages"]) = BuildWithdrawMessage(model);

                return RedirectToAction("Index");
            }
            return View(withdraw);
        }

        private (string successMessage, string [] infoMessages) BuildWithdrawMessage(Withdraw withdraw)
        {
            var genericInfo = $"Success! {withdraw.User.Name} has withdrawn ${withdraw.CurrencyDictionary.Balance} from ATM {withdraw.AutomatedTellerMachine.Alias}. Spend it wisely and have fun!";

            var detailedInfo = ReportHelper
                .GenerateWithdrawDetails(withdraw.CurrencyDictionary);

            return (genericInfo, detailedInfo);
        }
    
        private bool WithdrawExists(int id)
        {
            return _context.Withdraw.Any(e => e.WithdrawId == id);
        }
    }
}
