using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.ChildInfoService;
using Core.InfoKhademService;
using Core.TimeShitChild;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Models;

namespace AdminGonbadTala.Controllers
{
    public class TimeShitChildsController : Controller
    {
        private readonly TimeShitService _timeShitService;
        private readonly ChildInfoService _childInfoService;
        private readonly InfoKhademService _infoKhademService;

        public TimeShitChildsController(TimeShitService timeShitService, ChildInfoService childInfoService, InfoKhademService infoKhademService)
        {
            _timeShitService = timeShitService;
            _childInfoService = childInfoService;
            _infoKhademService = infoKhademService;
        }

        // GET: TimeShitChilds
        public async Task<IActionResult> Index()
        {
            var data =await _timeShitService.GetAllTimeShitChildWithInfo();
            return View(data);
        }

        // GET: TimeShitChilds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _timeShitService.GetAllTimeShitChildWithInfo(a => a.Id == id);
            var data2 = data.FirstOrDefault();
            if (data == null) 
            {
                return NotFound();
            }

            return View(data2);
        }

        // GET: TimeShitChilds/Create
        public async Task< IActionResult> Create()
        {
            ViewData["InfochildId"] = new SelectList(await _childInfoService.GetAllchild(), "Id", "PhoneNumber", "FName", "LName");
            ViewData["InfoKhademId"] = new SelectList( await _infoKhademService.GetAllKhadem(), "Id", "FName");
            return View();
        }

        // POST: TimeShitChilds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardNumber,IsMosafer,PickUpBy,DateNow,TimeIn,TimeOut,InfoKhademId,InfochildId")] TimeShitChild timeShitChild)
        {
            if (ModelState.IsValid)
            {
              await  _timeShitService.CreateTimeShitChild(timeShitChild);
                 
                return RedirectToAction(nameof(Index));
            }
            ViewData["InfochildId"] = new SelectList(await _childInfoService.GetAllchild(), "Id", "PhoneNumber", "FName", "LName");
            ViewData["InfoKhademId"] = new SelectList(await _infoKhademService.GetAllKhadem(), "Id", "Id", timeShitChild.InfoKhademId);
            return View(timeShitChild);
        }

        // GET: TimeShitChilds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeShitChild = await _timeShitService.GetTimeShitChildById(id.Value);

            if (timeShitChild == null)
            {
                return NotFound();
            }

            await _timeShitService.UpdateTimeShitChild(timeShitChild);
            ViewData["InfochildId"] = new SelectList(await _childInfoService.GetAllchild(), "Id", "Id", timeShitChild.InfochildId);
            ViewData["InfoKhademId"] = new SelectList(await _infoKhademService.GetAllKhadem(), "Id", "Id", timeShitChild.InfoKhademId);
            return View(timeShitChild);
        }

        // POST: TimeShitChilds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardNumber,IsMosafer,PickUpBy,DateNow,TimeIn,TimeOut,InfoKhademId,InfochildId")] TimeShitChild timeShitChild)
        {
            if (id != timeShitChild.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  await _timeShitService.UpdateTimeShitChild(timeShitChild);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["InfochildId"] = new SelectList(await _childInfoService.GetAllchild(), "Id", "Id", timeShitChild.InfochildId);
            ViewData["InfoKhademId"] = new SelectList(await _infoKhademService.GetAllKhadem(), "Id", "Id", timeShitChild.InfoKhademId);
            return View(timeShitChild);
        }

        // GET: TimeShitChilds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeShitChild = await _timeShitService.GetTimeShitChildById(id.Value);
               
            if (timeShitChild == null)
            {
                return NotFound();
            }

            return View(timeShitChild);
        }

        // POST: TimeShitChilds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeShitChild =  await _timeShitService.GetTimeShitChildById(id);
            if (timeShitChild != null)
            {
             await   _timeShitService.DeleteTimeShitChild(timeShitChild);
            }

           
            return RedirectToAction(nameof(Index));
        }

        
    }
}
