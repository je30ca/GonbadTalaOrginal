using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class CheckOutRequest
    {
        public int VisitId { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
}
