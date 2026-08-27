using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    
        public class childvm
        {
            [Required(ErrorMessage = "نام الزامی است")]
            [StringLength(50, ErrorMessage = "نام نباید بیشتر از 50 کاراکتر باشد")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "نام خانوادگی الزامی است")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "شماره تماس الزامی است")]
            //[Phone(ErrorMessage = "شماره تماس معتبر نیست")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "سن الزامی است")]
            public int Age { get; set; }
            //public String? IsMosafer { set; get; }
    }
     }
