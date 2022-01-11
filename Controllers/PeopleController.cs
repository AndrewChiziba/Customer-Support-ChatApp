using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelloFuture.Data;
using HelloFuture.Models;
using Microsoft.AspNetCore.Authorization;
using HelloFuture.Helpers;
using System.Security.Claims;

namespace HelloFuture.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Customer + "," + Roles.Agent)]
    public class PeopleController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        string curr_userId;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.People.ToListAsync());
        }

        public async Task<IActionResult> MyProfile()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curr_customer = _context.People.First(id => id.UserId == curr_userId);
            var onep = await _context.People.Where(id => id.Id == curr_customer.Id).ToListAsync();
            return View(onep);
            
        }

        public async Task<IActionResult> AgentProfile()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curr_Agent = _context.CallAgents.First(id => id.UserId == curr_userId);
            var onep = await _context.CallAgents.Where(id => id.Id == curr_Agent.Id).ToListAsync();
            return View(onep);

        }

        // GET:
        public async Task<IActionResult> AgentIsFree()
        {
            return View();
        }

        
        public async Task<IActionResult> AgentIsFreed()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curr_Agent = _context.CallAgents.First(id => id.UserId == curr_userId);
            curr_Agent.available = true;
            _context.SaveChanges();

            return RedirectToAction("AgentProfile", "People");

        }

        // GET: People
        public async Task<IActionResult> AgentNotFree()
        {
            return View();
        }

        public async Task<IActionResult> OccupyAgent()
        {
            curr_userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curr_Agent = _context.CallAgents.First(id => id.UserId == curr_userId);
            curr_Agent.available = false;
            _context.SaveChanges();

            return RedirectToAction("AgentProfile", "People");//tobe changed to home page

        }


        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name,Surname")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Surname")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
