using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class InfoKhadem
    {
        [Key]
        public int Id { set; get; }
        public String? PhoneNumber { set; get; }
        public String? FName { set; get; }
        public String? LName { set; get; }
        public String? CodeKhadem { set; get; }

        public ICollection<TimeShitChild>? TimeShitChilds { get; set; }
    }
}
