using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class WebAppContattoController : Controller
    {
        private readonly WebAppProgettoDbContext _context;
        private readonly UserManager<WebAppUser> _userManager;

        public WebAppContattoController(WebAppProgettoDbContext context, UserManager<WebAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: WebAppContatto
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var contatti = await _context.Contatti.Where(c => c.UserId == user.Id)
                .Include(c => c.User)
                .ToListAsync();
            return View(contatti);
        }

        // GET: WebAppContatto/Details/5
        public async Task<IActionResult> Dettagli(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webAppContatto = await _context.Contatti
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.ContattoID == id);
            if (webAppContatto == null)
            {
                return NotFound();
            }

            return View(webAppContatto);
        }

        // GET: WebAppContatto/AggiungiOModifica
        public async Task<IActionResult> Aggiungi()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = userId;
            var user = await _context.Users.FindAsync(userId);
            ViewData["User"] = user;
            return View(new WebAppContatto());
            
        }

        // POST: WebAppContatto/Aggiungi
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aggiungi([Bind("ContattoID,ContattoName,ContattoCognome,ContattoCitta,ContattoTelefono,UserId")] WebAppContatto webAppContatto)
        {
            var User = await _context.Users.FindAsync(webAppContatto.UserId);
            webAppContatto.User = User;

            if (ModelState.IsValid)
            {
                _context.Add(webAppContatto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", webAppContatto.UserId);
            return View(webAppContatto);
        }

        // GET: WebAppContatto/Modifica/5
        public async Task<IActionResult> Modifica(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webAppContatto = await _context.Contatti.FindAsync(id);
            if (webAppContatto == null)
            {
                return NotFound();
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", webAppContatto.UserId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = userId;
            //var user = await _context.Users.FindAsync(userId);
            //ViewData["User"] = user;
            //return View(new WebAppContatto());
            return View(webAppContatto);
        }

        // POST: WebAppContatto/Modifica/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifica(int id, [Bind("ContattoID,ContattoName,ContattoCognome,ContattoCitta,ContattoTelefono,UserId")] WebAppContatto webAppContatto)
        {
            if (id != webAppContatto.ContattoID)
            {
                return NotFound();
            }

            var User = await _context.Users.FindAsync(webAppContatto.UserId);
            webAppContatto.User = User;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(webAppContatto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WebAppContattoExists(webAppContatto.ContattoID))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", webAppContatto.UserId);
            return View(webAppContatto);
        }

        // GET: WebAppContatto/Delete/5
        public async Task<IActionResult> Elimina(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webAppContatto = await _context.Contatti
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.ContattoID == id);
            if (webAppContatto == null)
            {
                return NotFound();
            }

            return View(webAppContatto);
        }

        // POST: WebAppContatto/Delete/5
        [HttpPost, ActionName("Elimina")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfermaElimina(int id)
        {
            var webAppContatto = await _context.Contatti.FindAsync(id);
            if (webAppContatto != null)
            {
                _context.Contatti.Remove(webAppContatto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WebAppContattoExists(int id)
        {
            return _context.Contatti.Any(e => e.ContattoID == id);
        }
    }
}
