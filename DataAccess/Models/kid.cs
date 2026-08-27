using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class kid
    {
        public int Id { get; set; } // کلید اصلی برای شناسایی هر کودک

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsTraveler { get; set; } // آیا مسافر است؟ (بله/خیر)

        public int Age { get; set; }
        public DateTime? BirthDate { set; get; }

        public string Guardian { get; set; } // کی میاد دنبالش؟ (مثلاً "مادر" یا "پدر")
    }
}
