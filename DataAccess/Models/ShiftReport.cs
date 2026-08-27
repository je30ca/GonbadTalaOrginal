using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class ShiftReport
    {
        public int Id { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        public DateTime ReportDate { get; set; } = DateTime.Today;

        [Display(Name = "شیفت")]
        [Required(ErrorMessage = "لطفاً {0} را انتخاب کنید")]
        public string Shift { get; set; } = "صبح";

        [Display(Name = "موضوع")]
        public string? Subject { get; set; }

        [Display(Name = "قرآن")]
        public string? QuranActivity { get; set; }

        [Display(Name = "حلقه معرفی و احکام")]
        public string? AhkamActivity { get; set; }

        [Display(Name = "بازی")]
        public string? GameActivity { get; set; }

        [Display(Name = "شعر")]
        public string? PoemActivity { get; set; }

        [Display(Name = "داستان یا نمایشنامه")]
        public string? StoryActivity { get; set; }

        [Display(Name = "نقاشی و کاردستی")]
        public string? CraftActivity { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
        [Display(Name = "تعداد کل")]
        public int TotalParticipants { get; set; }

        [Display(Name = "مجاور")]
        public int RegularCount { get; set; }

        [Display(Name = "مسافر")]
        public int TravelerCount { get; set; }

        [Display(Name = "کل خادمین حاضر")]
        public string? PresentKhads { get; set; }

        // 🔴 کلید خارجی و رابطه با جدول خادمین
        [Display(Name = "متصدی گزارش")]
        [Required(ErrorMessage = "لطفاً {0} را انتخاب کنید")]
        public int KhademId { get; set; }

        [ForeignKey("KhademId")]
        public virtual Khadem? Khadem { get; set; }


    }
}

