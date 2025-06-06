using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class ClientVehicles
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Client_Id { get; set; }
        public Clients Client { get; set; }
        [Required]
        public string Car_Model { get; set; }
        [Required]
        public string Car_Vin { get; set; }
        [Required]
        public string Car_RegNo { get; set; }
        [Required]
        public int Car_Year { get; set; }
        public bool IsMaintenanced { get; set; } = false;
        public DateOnly? EstimatedMaintenanceEnd { get; set; }
    }
}
