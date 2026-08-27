using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class InfoChild
    {
        [Key]
        public int Id { set; get; }
        public String? PhoneNumber { set; get; }
        public String? FName { set; get; }
        public String? LName { set; get; }

       // public String? CardNumber { set; get; }
        public DateTime? BirthDate { set; get; }
        public DateTime? RegisterDate { set; get; }
        public DateTime? ExitTime { set; get; }
        public bool IsTimeUp => RegisterDate.HasValue && DateTime.Now >= RegisterDate.Value.AddHours(1);
       
        public double RemainingMinutes => RegisterDate.HasValue
            ? (RegisterDate.Value.AddHours(1) - DateTime.Now).TotalMinutes
            : 0;
        public ICollection<TimeShitChild>? TimeShitChilds { get; set; }




    }
}
