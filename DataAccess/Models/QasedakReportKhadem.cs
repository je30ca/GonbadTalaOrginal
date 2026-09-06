namespace DataAccess.Models;

public class QasedakReportKhadem
{
    public int QasedakReportId { get; set; }
    public QasedakReport QasedakReport { get; set; } = null!;

    public int KhademId { get; set; }
    public Khadem Khadem { get; set; } = null!;
}
