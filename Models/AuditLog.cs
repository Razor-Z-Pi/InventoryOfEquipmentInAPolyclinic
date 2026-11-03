using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    public class AuditLog
    {
        [Key] public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // "Добавление", "Удаление"
        public string Details { get; set; } = string.Empty; // Что именно сделали
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
