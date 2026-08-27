using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class TimeSheet
    {
        public int Id { get; set; }

        public DateTime EntryTime { get; set; } // زمان ورود

        public DateTime? ExitTime { get; set; } // زمان خروج (علامت سوال یعنی می‌تواند ابتدا خالی باشد)

        // ارتباط با جدول کودک
        public int ChilddId { get; set; }
        public kid Childd { get; set; }
        public string Guardian { get; set; } // کی میاد دنبالش؟ (مثلاً "مادر" یا "پدر")

        // شناسه خادمی که این رکورد را ثبت کرده است
        public int? RegisteredByKhademId { get; set; }

        [ForeignKey("RegisteredByKhademId")]
        public virtual Khadem RegisteredByKhadem { get; set; }
    }
}
