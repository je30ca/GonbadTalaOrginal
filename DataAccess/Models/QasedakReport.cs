using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class QasedakReport
{
    public int Id { get; set; }

    [Display(Name = "تاریخ اجرا")]
    public DateTime ExecutionDate { get; set; }

    [Display(Name = "شیفت")]
    [MaxLength(20)]
    public string Shift { get; set; } = string.Empty;

    [Required(ErrorMessage = "لطفاً موضوع را وارد کنید")]
    [Display(Name = "موضوع")]
    public string Subject { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "تعداد اجرا نمی‌تواند منفی باشد")]
    [Display(Name = "تعداد اجرای طرح قاصدک")]
    public int ExecutionCount { get; set; }

    [Required(ErrorMessage = "حداقل یک حلقهٔ ارائه‌شده انتخاب کنید")]
    [Display(Name = "حلقه‌های ارائه‌شده")]
    public string PresentedCircles { get; set; } = string.Empty;

    [Required(ErrorMessage = "حداقل یک محل اجرا انتخاب کنید")]
    [Display(Name = "محل اجرا")]
    public string ExecutionLocations { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "تعداد مخاطبین نمی‌تواند منفی باشد")]
    [Display(Name = "تعداد مخاطبین")]
    public int AudienceCount { get; set; }

    [Required(ErrorMessage = "لطفاً ردهٔ سنی را وارد کنید")]
    [Display(Name = "ردهٔ سنی")]
    public string AgeGroup { get; set; } = string.Empty;

    public int? CreatedByKhademId { get; set; }
    public Khadem? CreatedByKhadem { get; set; }

    public ICollection<QasedakReportKhadem> Khadems { get; set; } = new List<QasedakReportKhadem>();
}
