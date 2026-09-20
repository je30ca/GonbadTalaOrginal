using System.ComponentModel.DataAnnotations;

namespace AdminGonbadTala.Models;

public class KhademAttendanceDayViewModel
{
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;
    public List<KhademAttendanceRowViewModel> Rows { get; set; } = [];
}

public class KhademAttendanceRowViewModel
{
    public int KhademId { get; set; }
    public string KhademName { get; set; } = string.Empty;
    public string? EntryTime { get; set; }
    public string? ExitTime { get; set; }
    public string? Notes { get; set; }
}
