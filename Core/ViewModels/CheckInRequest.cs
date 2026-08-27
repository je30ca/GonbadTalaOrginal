using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class CheckInRequest
    {
        public string PhoneNumber { get; set; }
        public DateTime? DateNow { set; get; }
        public DateTime? TimeIn { set; get; }
        public int CardNumber { set; get; }
        public String? IsMosafer { set; get; }
        public int PickUpBy { set; get; }

    }
}
