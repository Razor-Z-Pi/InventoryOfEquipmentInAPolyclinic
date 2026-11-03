using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    public class EquipmentDisplay
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string CabinetName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;

        // Оставляем оригинальный объект для удаления/редактирования
        public Equipment OriginalEquipment { get; set; }

        public DateTime CommissionDate { get; set; }
        public int ServiceLife { get; set; }

        public DateTime ExpirationDate => CommissionDate.AddYears(ServiceLife);

        public string LifeStatus
        {
            get
            {
                var daysLeft = (ExpirationDate - DateTime.Now).Days;
                if (daysLeft < 0) return "Просрочено";
                if (daysLeft <= 90) return "Истекает"; // 3 месяца
                return "В норме";
            }
        }
    }
}
