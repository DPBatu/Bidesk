using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidesk.Application.DTOs.User
{
    public class PhoneBookDto
    {
        public string NameSurname { get; set; }
        public string Department { get; set; }
        public string IntercomNumber { get; set; }
        public string? CompanyPhoneNumber { get; set; }
    }
}
