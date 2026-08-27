using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.InfoKhademService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Models;


namespace AdminGonbadTala.Controllers
{
    public class InfoKhademsController : Controller
    {
        private readonly InfoKhademService _infoKhademService;

        public InfoKhademsController(InfoKhademService context)
        {
            _infoKhademService = context;
        }

        // GET: InfoKhadems
        public async Task<IActionResult> Index()
        {
            return View(await _infoKhademService.GetAllKhadem());
        }

        // GET: InfoKhadems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoKhadem = await _infoKhademService.GetkhademById(id.Value);
                
            if (infoKhadem == null)
            {
                return NotFound();
            }

            return View(infoKhadem);
        }

        // GET: InfoKhadems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InfoKhadems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhoneNumber,FName,LName,CodeKhadem")] InfoKhadem infoKhadem)
        {
            if (ModelState.IsValid)
            {
             await _infoKhademService.CreateKhadem(infoKhadem);
                
                return RedirectToAction(nameof(Index));
            }
            return View(infoKhadem);
        }

        // GET: InfoKhadems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoKhadem = await _infoKhademService.GetkhademById(id.Value);

            if (infoKhadem == null)
            {
                return NotFound();
            }

            await _infoKhademService.UpdateKhadem(infoKhadem);
            return View(infoKhadem);
        }

        // POST: InfoKhadems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhoneNumber,FName,LName,CodeKhadem")] InfoKhadem infoKhadem)
        {
            if (id != infoKhadem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  await  _infoKhademService.UpdateKhadem(infoKhadem);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(infoKhadem);
        }

        // GET: InfoKhadems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoKhadem = await _infoKhademService.GetkhademById(id.Value);

               
            if (infoKhadem == null)
            {
                return NotFound();
            }

            return View(infoKhadem);
        }

        // POST: InfoKhadems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var infoKhadem = await _infoKhademService.GetkhademById(id);
            if (infoKhadem != null)
            {
               await _infoKhademService.DeleteKhadem(infoKhadem);
            }

            
            return RedirectToAction(nameof(Index));
        }

       
    }
}
