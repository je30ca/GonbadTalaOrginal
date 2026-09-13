namespace DataAccess.Models;

public static class UserRoles
{
    public const string Management = "مدیریت";
    public const string ShiftLead = "سرشیفت";
    public const string Servant = "خادم";
    public static readonly string[] All = [Management, ShiftLead, Servant];
}
