using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Khadem
    {
        public int Id { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName => $"{FirstName} {LastName}";

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        [Display(Name = "کد پرسنلی")]
        public string PersonalCode { get; set; } 

        [Display(Name = "تخصص")]
        public string Specialization { get; set; } // مثلاً: خردسال، بازی فکری، پذیرش

        [Display(Name = "روز هفته")]
        public string WorkingDay { get; set; } 

        [Display(Name = "شیفت")]
        public int Shift { get; set; } // 1 یا 2 یا 3

        [Required]
        [MaxLength(20)]
        [Display(Name = "نقش کاربری")]
        public string Role { get; set; } = UserRoles.Servant;

        // Only password hashes are persisted. Plain-text passwords must never be
        // stored on this entity or returned to a view.
        public string PasswordHash { get; set; } = string.Empty;
    }
}
