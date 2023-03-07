using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF_Models;
using EF_Models.Models;
using Microsoft.AspNetCore.Identity;
using Maelstrom.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Maelstrom.Controllers
{
    public class AccountsController : Controller
    {
        private readonly MaelstromContext _context;
        
        public AccountsController(MaelstromContext context)
        {
       
            _context = context;
        }

        // GET: Accounts
        
        public async Task<IActionResult> Index()
        {
            var userIdentity = User.Identity.Name; // This would be more secure if we used the ID property but then we will have to relate those tables

           //var test = HttpContext.User.Identity.Name; //this works but I do not know how secure it is
            var maelstromContext = _context.Accounts.Where(a => a.Email == userIdentity);   /*Include(a => a.AccountStanding).Include(a => a.AccountType).*/
            return View(await maelstromContext.ToListAsync());
        }

        // GET: Accounts/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.AccountStanding)
                .Include(a => a.AccountType)
                .FirstOrDefaultAsync(m => m.AccountID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["AccountStandingID"] = new SelectList(_context.AccountStandings, "AccountStandingID", "Name");
            ViewData["AccountTypeID"] = new SelectList(_context.AccountTypes, "AccountTypeID", "AccountTypeID");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountID,HolderName,StreetAdress,City,StateOrProvince,Country,ZipCode,Email,AccountTypeID,AccountStandingID")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountStandingID"] = new SelectList(_context.AccountStandings, "AccountStandingID", "Name", account.AccountStandingID);
            ViewData["AccountTypeID"] = new SelectList(_context.AccountTypes, "AccountTypeID", "AccountTypeID", account.AccountTypeID);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["AccountStandingID"] = new SelectList(_context.AccountStandings, "AccountStandingID", "Name", account.AccountStandingID);
            ViewData["AccountTypeID"] = new SelectList(_context.AccountTypes, "AccountTypeID", "AccountTypeID", account.AccountTypeID);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountID,HolderName,StreetAdress,City,StateOrProvince,Country,ZipCode,Email,AccountTypeID,AccountStandingID")] Account account)
        {
            if (id != account.AccountID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountID))
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
            ViewData["AccountStandingID"] = new SelectList(_context.AccountStandings, "AccountStandingID", "Name", account.AccountStandingID);
            ViewData["AccountTypeID"] = new SelectList(_context.AccountTypes, "AccountTypeID", "AccountTypeID", account.AccountTypeID);
            return View(account);
        }

        // GET: Accounts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.AccountStanding)
                .Include(a => a.AccountType)
                .FirstOrDefaultAsync(m => m.AccountID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'MaelstromContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
          return _context.Accounts.Any(e => e.AccountID == id);
        }
    }
}
