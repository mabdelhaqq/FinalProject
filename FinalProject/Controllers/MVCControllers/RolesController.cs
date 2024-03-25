using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FinalProject.Controllers.MVCControllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _manager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _manager = roleManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var roles = await _manager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (!await _manager.RoleExistsAsync(role.Name))
            {
                await _manager.CreateAsync(new IdentityRole(role.Name));
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _manager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, IdentityRole role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRole = await _manager.FindByIdAsync(role.Id);
                    if (existingRole == null)
                    {
                        return NotFound();
                    }

                    existingRole.Name = role.Name;
                    await _manager.UpdateAsync(existingRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RoleExists(role.Id))
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
            return View(role);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _manager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _manager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _manager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            await _manager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoleExists(string id)
        {
            return await _manager.FindByIdAsync(id) != null;
        }
    }
}
