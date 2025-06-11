using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class EmployeeWorkOnVehicles
    {
        [Key]
        public int Id { get; set; }
        public int ClientVehicle_Id { get; set; }
        public ClientVehicles ClientVehicle { get; set; }
        public int Employee_Id { get; set; }
        public Employees Employee { get; set; }
        public DateOnly Date { get; set; }
        public string WorkOn { get; set; } //nazwa:cena:zrobione{0/1};nazwa:cena:zrobione{0/1};...
        public bool IsDone { get; set; } = false;
    }
}
