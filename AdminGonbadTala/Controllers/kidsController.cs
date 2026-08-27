using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Models;
using PersianDate.Standard;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AdminGonbadTala.Controllers
{
    public class kidsController : Controller
    {
        private readonly GonbadDbContext _context;

        public kidsController(GonbadDbContext context)
        {
            _context = context;
        }

        // GET: kids
        public async Task<IActionResult> Index(string? searchString)
        {

            // 1. ابتدا تمام کودکان را در نظر می‌گیریم
            var localkids = from k in _context.Kids
                select k;

            // 2. اگر کلمه‌ای جستجو شده باشد، لیست را فیلتر می‌کنیم
            if (!String.IsNullOrEmpty(searchString))
            {
                localkids = localkids.Where(s => s.FirstName.Contains(searchString)
                                                || s.LastName.Contains(searchString)
                                                || s.PhoneNumber.Contains(searchString));
            }
            else
            {
                // اگر سرچ نشده بود، فقط ۵۰ نفر آخر را بیار
                localkids = localkids.OrderByDescending(k => k.Id).Take(50);
            }

            // مقدار جستجو شده را به View می‌فرستیم تا در باکس جستجو باقی بماند
            ViewData["CurrentFilter"] = searchString;

                //return View(await kids.ToListAsync());
            return View(await localkids.ToListAsync());
        }

        // GET: kids/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kid == null)
            {
                return NotFound();
            }

            return View(kid);
        }

        // GET: kids/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: kids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,IsTraveler,Age,Guardian")] kid kid, string BirthDate, int KidId)
        {

            // ۱. سناریوی اول: کودک از قبل در دیتابیس وجود دارد
            if (KidId > 0)
            {
                if (!string.IsNullOrEmpty(BirthDate))
                {
                    var EnBirthDateStr = ToEnglishNumbers(BirthDate);
                    // تبدیل رشته شمسی به میلادی و انتساب به مدل
                    kid.BirthDate = EnBirthDateStr.ToEn();
                    kid.Id = KidId;
                }
                _context.Update(kid);
                await _context.SaveChangesAsync();

                int? currentKhademId = HttpContext.Session.GetInt32("KhademId");
                var newTimeSheet = new TimeSheet
                {
                    ChilddId = KidId, // آیدی کودکی که همین الان ساخته شد
                    EntryTime = DateTime.Now, // ساعت ورود همین الان ثبت شود
                    ExitTime = null, // هنوز خارج نشده است
                   Guardian = kid.Guardian,
                    RegisteredByKhademId = currentKhademId // به صورت خودکار از سشن خوانده شد
                };
                _context.TimeSheets.Add(newTimeSheet);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "ورود کودک با موفقیت ثبت شد.";
                return RedirectToAction(nameof(Index));
            }
            // حالا اینجا تاریخ‌ها را به صورت رشته (شمسی) داریم
            if (!string.IsNullOrEmpty(BirthDate))
            {
                var EnBirthDateStr = ToEnglishNumbers(BirthDate);
                // تبدیل رشته شمسی به میلادی و انتساب به مدل
                kid.BirthDate = EnBirthDateStr.ToEn();
            }
            if (ModelState.IsValid)
            {
                _context.Add(kid);
                await _context.SaveChangesAsync();
                // ۲. دریافت ID خادم فعال از سشن
                int? currentKhademId = HttpContext.Session.GetInt32("KhademId");
                var newTimeSheet = new TimeSheet
                {
                    ChilddId = kid.Id, // آیدی کودکی که همین الان ساخته شد
                    EntryTime = DateTime.Now, // ساعت ورود همین الان ثبت شود
                    ExitTime = null, // هنوز خارج نشده است
                    Guardian = kid.Guardian,
                    RegisteredByKhademId = currentKhademId // به صورت خودکار از سشن خوانده شد
                };

                _context.TimeSheets.Add(newTimeSheet);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "TimeSheets");
            }
            return View(kid);
        }

        // GET: kids/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids.FindAsync(id);
            if (kid == null)
            {
                return NotFound();
            }
            return View(kid);
        }

        // POST: kids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,IsTraveler,Age,Guardian")] kid kid, string BirthDate)
        {
            if (id != kid.Id)
            {
                return NotFound();
            }
            // حالا اینجا تاریخ‌ها را به صورت رشته (شمسی) داریم
            if (!string.IsNullOrEmpty(BirthDate))
            {
                var EnBirthDateStr = ToEnglishNumbers(BirthDate);
                // تبدیل رشته شمسی به میلادی و انتساب به مدل
                kid.BirthDate = EnBirthDateStr.ToEn();
            }

            //if (!string.IsNullOrEmpty(RegisterDateStr))
            //{
            //    var EnRegisterDateStr = ToEnglishNumbers(RegisterDateStr);
            //    infoChild.RegisterDate = EnRegisterDateStr.ToEn();
            //    var now = DateTime.Now;
            //    infoChild.RegisterDate = new DateTime(
            //        infoChild.RegisterDate.Value.Year,
            //        infoChild.RegisterDate.Value.Month,
            //        infoChild.RegisterDate.Value.Day,
            //        now.Hour,
            //        now.Minute,
            //        now.Second
            //    );
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!kidExists(kid.Id))
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
            return View(kid);
        }

        // GET: kids/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kid == null)
            {
                return NotFound();
            }

            return View(kid);
        }

        // POST: kids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kid = await _context.Kids.FindAsync(id);
            if (kid != null)
            {
                _context.Kids.Remove(kid);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> NewEntry(int kidId, string companion)
        {
            // ۱. بررسی می‌کنیم که آیا این کودک واقعاً وجود دارد؟
            var kid = await _context.Kids.FindAsync(kidId);
            if (kid == null)
            {
                return NotFound();
            }

            // ۲. یک رکورد تایم‌شیت جدید برای او می‌سازیم
            var newTimeSheet = new TimeSheet
            {
                ChilddId = kid.Id,
                EntryTime = DateTime.Now,
                ExitTime = null, // چون تازه وارد شده
                Guardian = companion
            };

            _context.TimeSheets.Add(newTimeSheet);
            await _context.SaveChangesAsync();

            // ۳. هدایت به صفحه تایم‌شیت‌ها برای دیدن لیست حضور و غیاب
            return RedirectToAction("Index", "TimeSheets");
        }

        [HttpGet]
        public IActionResult GetKidByPhone(string phone)
        {
            var kid = _context.Kids.FirstOrDefault(k => k.PhoneNumber == phone);
            if (kid != null)
            {
                return Json(new { success = true, firstName = kid.FirstName,
                    lastName = kid.LastName,
                    id = kid.Id,
                    istraveler= kid.IsTraveler,guardian=kid.Guardian,
                    // تبدیل تاریخ میلادی دیتابیس به رشته شمسی جهت نمایش در فرم
                    birthDate = ConvertGregorianToPersian(kid.BirthDate),

                    // محاسبه خودکار سن
                    ageDisplay = CalculateAge(kid.BirthDate)


                });
            }
            
            return Json(new { success = false });
        }

        public async Task<kid> GetchildById(int id)
        {
            var kid=  await _context.Kids.FindAsync(id);
            return kid;
        }

        //// POST: InfoChilds/Exit/5
        //[HttpPost, ActionName("Exit")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Exit(int id)
        //{
        //    var allkids = await _context.GetchildById(id);
        //    infoChild.ExitTime = DateTime.Now;

        //    if (infoChild != null)
        //    {
        //        await _context.UpdateChild(infoChild);
        //    }


        //    return RedirectToAction(nameof(Index));
        //}
        public static string ToEnglishNumbers(string input)
        {
            return input.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2")
                .Replace("۳", "3").Replace("۴", "4").Replace("۵", "5")
                .Replace("۶", "6").Replace("۷", "7").Replace("۸", "8");
        }
        // تبدیل میلادی به شمسی
        private string ConvertGregorianToPersian(DateTime? date)
        {
            if (!date.HasValue) return "";

            var pc = new System.Globalization.PersianCalendar();
            return $"{pc.GetYear(date.Value):0000}/{pc.GetMonth(date.Value):00}/{pc.GetDayOfMonth(date.Value):00}";
        }
        [HttpGet]
        public IActionResult CreatePartial()
        {
            // اگر نیاز به مدل خالی یا ViewModel دارید اینجا پاس بدهید
            return PartialView("~/Views/Kids/_CreateKidPartial.cshtml");
        }
        // محاسبه دقیق سن بر اساس سال
        private string CalculateAge(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return "0";

            var today = DateTime.Today;
            var age = today.Year - birthDate.Value.Year;

            // اگر هنوز روز تولدش در امسال نرسیده، یک سال کم کن
            if (birthDate.Value.Date > today.AddYears(-age)) age--;

            return age.ToString();
        }

        private bool kidExists(int id)
        {
            return _context.Kids.Any(e => e.Id == id);
        }
    }
}
