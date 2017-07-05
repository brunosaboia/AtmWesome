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

namespace Coinify.Web.Controllers
{
    public class WithdrawsController : Controller
    {
        private readonly CoinifyWebContext _context;

        public WithdrawsController(CoinifyWebContext context)
        {
            _context = context;
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
            ViewBag.Users =  GetUsers().Result;
            ViewBag.Atms = GetAtms().Result;

            return View();
        }

        private Dictionary<IMoney, int> GenerateValidMoneyDictionary(CurrencyDictionary dict)
        {
            var validNotes = dict
                .NoteDictionary
                .Where(kvp => kvp.Value > 0)
                .Select(kvp => new KeyValuePair<IMoney, int>(kvp.Key, kvp.Value))
                .OrderByDescending(kvp => kvp.Key.Value);

            var validCoins = dict
                .CoinDictionary
                .Where(kvp => kvp.Value > 0)
                .Select(kvp => new KeyValuePair<IMoney, int>(kvp.Key, kvp.Value))
                .OrderByDescending(c => c.Key.Value);

            var validMoney = validNotes
                .Union(validCoins)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return validMoney;
        }

        private CurrencyDictionary WithdrawFromAtm(User user, AutomatedTellerMachine atm, int amount)
        {
            var curDict = atm.CurrencyDictionary;

            var ret = new CurrencyDictionary()
            {
                CoinDictionary = new Dictionary<Coin, int>(),
                NoteDictionary = new Dictionary<Note, int>()
            };

            if (user.Balance - amount < 0)
            {
                throw new InsufficentFundsException($"User {user.UserId} has insuficient funds to withdraw ${amount}");
            }

            user.Balance -= amount;

            var validMoney = GenerateValidMoneyDictionary(curDict);

            foreach (var kvp in validMoney.ToList())
            {
                while (validMoney[kvp.Key] != 0)
                {
                    if ((amount - kvp.Key.Value) < 0)
                        break;

                    amount -= kvp.Key.Value;
                    validMoney[kvp.Key]--;

                    if (kvp.Key is Note)
                    {
                        var note = kvp.Key as Note;

                        if (ret.NoteDictionary.ContainsKey(note))
                            ret.NoteDictionary[note]++;
                        else ret.NoteDictionary[note] = 1;
                    }
                    else if (kvp.Key is Coin)
                    {
                        var coin = kvp.Key as Coin;

                        if (ret.CoinDictionary.ContainsKey(coin))
                            ret.CoinDictionary[coin]++;
                        else ret.CoinDictionary[coin] = 1;
                    }
                    else break;
                }
            }
            if(amount > 0)
            {
                throw new
                    InsufficientChangeException($"There is no change in ATM {atm.AutomatedTellerMachineId} to withdraw ${amount}");
            }

            atm.CurrencyDictionary = curDict;

            _context.Update(user);
            _context.Update(atm);

            _context.SaveChanges();

            return ret;
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
                    model.CurrencyDictionary = WithdrawFromAtm(model.User, model.AutomatedTellerMachine, withdraw.Amount);
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
                return RedirectToAction("Index");
            }
            return View(withdraw);
        }
    
        private bool WithdrawExists(int id)
        {
            return _context.Withdraw.Any(e => e.WithdrawId == id);
        }
    }
}
