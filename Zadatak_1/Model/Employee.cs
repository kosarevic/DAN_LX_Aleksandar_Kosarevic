using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1.Model
{
    public class Employee
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JMBG { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string RegistrationNumber { get; set; }
        public string PhoneNumber { get; set; }
        public Location Location { get; set; }
        public Sector Sector { get; set; }
        public Employee Manager { get; set; }

    }
}
