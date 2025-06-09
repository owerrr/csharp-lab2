using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public int? Employee_Title_Id { get; set; }
        public EmployeeTitles EmployeeTitle { get; set; }
        public int? Client_Id { get; set; }
        public Clients? Client { get; set; }
        public int? Employee_Id { get; set; }
        public Employees? Employee { get; set; }
    }
}
