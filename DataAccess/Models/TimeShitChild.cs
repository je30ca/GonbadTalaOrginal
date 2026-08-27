using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class TimeShitChild
    {
        

        [Key]
        public int Id { set; get; }
        public int CardNumber { set; get; }
        
        public String? IsMosafer { set; get; }
        public int PickUpBy { set; get; }
        public DateTime? DateNow{ set; get; }
        public DateTime? TimeIn { set; get; }
        public DateTime? TimeOut { set; get; }

        public int InfoKhademId { set; get; }

        [ForeignKey("InfoKhademId")]
        public InfoKhadem? InfoKhadem { set; get; }

        public int InfochildId { set; get; }

        [ForeignKey("InfochildId")]
        public InfoChild? InfoChild { set; get; }
    }
}
