using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.ChildInfoService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Models;
using System.Globalization;

using Microsoft.IdentityModel.Tokens;
using PersianDate.Standard;
using Microsoft.AspNetCore.Identity.Data;

namespace AdminGonbadTala.Controllers
{
    public class InfoChildsController : Controller
    {
        private readonly ChildInfoService _context;

        public InfoChildsController(ChildInfoService context)
        {
            _context = context;
            
        }

        // GET: InfoChilds
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetAllchild());
        }

       // GET: InfoChilds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoChild = await _context.GetchildById(id.Value);
            if (infoChild == null)
            {
                return NotFound();
            }

            return View(infoChild);
        }

        // GET: InfoChilds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InfoChilds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhoneNumber,FName,LName")] InfoChild infoChild, string BirthDateStr, string RegisterDateStr)
        {
            //if (!string.IsNullOrEmpty(infoChild.BirthDate.ToString()))
            //{
            //    //infoChild.BirthDate = BirthDatePersian.ToGregorianDate();
            //   // "1393/08/01 16:20".ToEn();
            //   infoChild.BirthDate = infoChild.BirthDate.ToString().ToEn();
            //    infoChild.RegisterDate = infoChild.RegisterDate.ToString().ToEn();
            //}
            // حالا اینجا تاریخ‌ها را به صورت رشته (شمسی) داریم
            if (!string.IsNullOrEmpty(BirthDateStr))
            {
                var EnBirthDateStr = ToEnglishNumbers(BirthDateStr);
                // تبدیل رشته شمسی به میلادی و انتساب به مدل
                infoChild.BirthDate = EnBirthDateStr.ToEn();
            }

            if (!string.IsNullOrEmpty(RegisterDateStr))
            {
                var EnRegisterDateStr= ToEnglishNumbers(RegisterDateStr);
                infoChild.RegisterDate = EnRegisterDateStr.ToEn();
                var now = DateTime.Now;
                infoChild.RegisterDate = new DateTime(
                    infoChild.RegisterDate.Value.Year,
                    infoChild.RegisterDate.Value.Month,
                    infoChild.RegisterDate.Value.Day,
                    now.Hour,
                    now.Minute,
                    now.Second
                );
            }
            if (ModelState.IsValid)
            {
                await _context.CreateChild(infoChild);
                return RedirectToAction(nameof(Index));
            }

            
            return View(infoChild);
        }

       // GET: InfoChilds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoChild = await _context.GetchildById(id.Value);
            if (infoChild == null)
            {
                return NotFound();
            }

          //  await _context.UpdateChild(infoChild);
            return View(infoChild);
        }

        // POST: InfoChilds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhoneNumber,FName,LName,BirthDate,RegisterDate")] InfoChild infoChild)
        {
            if (id != infoChild.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  await  _context.UpdateChild(infoChild);

                }
                catch (DbUpdateConcurrencyException)
                {
                   
                }
                return RedirectToAction(nameof(Index));
            }
            return View(infoChild);
        }

        // GET: InfoChilds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoChild = await _context.GetchildById(id.Value);
            if (infoChild == null)
            {
                return NotFound();
            }

            return View(infoChild);
        }

        // POST: InfoChilds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var infoChild = await _context.GetchildById(id);
            if (infoChild != null)
            {
              await  _context.DeleteChild(infoChild);
            }

           
            return RedirectToAction(nameof(Index));
        }

        // POST: InfoChilds/Exit/5
        [HttpPost, ActionName("Exit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Exit(int id)
        {
            var infoChild = await _context.GetchildById(id);
            infoChild.ExitTime = DateTime.Now;

            if (infoChild != null)
            {
                await _context.UpdateChild(infoChild);
            }


            return RedirectToAction(nameof(Index));
        }

        public static string ToEnglishNumbers(string input)
        {
            return input.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2")
                .Replace("۳", "3").Replace("۴", "4").Replace("۵", "5")
                .Replace("۶", "6").Replace("۷", "7").Replace("۸", "8")
                .Replace("۹", "9");
        }

        ///// <summary>
        ///// ثبت‌نام اولیه بچه (بار اول)
        ///// </summary>
        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterChild([FromBody] RegisterRequest request)
        //{
        //    try
        //    {
        //        // فراخوانی متد ثبت‌نام در سرویس
        //        var child = await _contextAll.RegisterNewChildAsync(
        //            request.FirstName,
        //            request.LastName,
        //            request.PhoneNumber,
        //            request.Age
        //        );

        //        // بازگشت پاسخ موفقیت‌آمیز
        //        return Ok(new
        //        {
        //            message = "ثبت‌نام با موفقیت انجام شد",
        //            childId = child.Id,
        //            name = $"{child.FirstName} {child.LastName}"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        // بازگشت خطا در صورت تکراری بودن شماره یا مشکل دیگر
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}

        ///// <summary>
        ///// ثبت ورود روزانه (برای بچه‌های ثبت‌نام شده)
        ///// </summary>
        //[HttpPost("checkin")]
        //public async Task<IActionResult> CheckInChild([FromBody] CheckInRequest request)
        //{
        //    try
        //    {
        //        // فراخوانی متد ورود در سرویس
        //        var visit = await _playHouseService.CheckInChildAsync(
        //            request.PhoneNumber,
        //            request.CheckInTime
        //        );

        //        return Ok(new
        //        {
        //            message = "ورود با موفقیت ثبت شد",
        //            visitId = visit.Id,
        //            childName = visit.Child.FirstName + " " + visit.Child.LastName
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}



    }
}
