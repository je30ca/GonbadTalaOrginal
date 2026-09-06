using System.ComponentModel.DataAnnotations;

namespace AdminGonbadTala.Models;

public class QasedakReportCreateViewModel
{
    public DateTime ExecutionDate { get; set; }
    public string Shift { get; set; } = string.Empty;

    [Required(ErrorMessage = "لطفاً حداقل یک خادم را انتخاب کنید")]
    public List<int> KhademIds { get; set; } = [];

    [Required(ErrorMessage = "لطفاً موضوع را وارد کنید")]
    [Display(Name = "موضوع")]
    public string Subject { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    [Display(Name = "تعداد اجرای طرح قاصدک")]
    public int ExecutionCount { get; set; }

    [Required(ErrorMessage = "حداقل یک حلقهٔ ارائه‌شده انتخاب کنید")]
    public string PresentedCircles { get; set; } = string.Empty;

    [Required(ErrorMessage = "حداقل یک محل اجرا انتخاب کنید")]
    public string ExecutionLocations { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    [Display(Name = "تعداد مخاطبین")]
    public int AudienceCount { get; set; }

    [Required(ErrorMessage = "لطفاً ردهٔ سنی را وارد کنید")]
    [Display(Name = "ردهٔ سنی")]
    public string AgeGroup { get; set; } = string.Empty;
}
