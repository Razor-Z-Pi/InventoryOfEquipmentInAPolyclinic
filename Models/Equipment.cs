using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    [Table("oborudovanie")]
    public class Equipment
    {
        [Key][Column("id")] public int Id { get; set; }
        [Column("nazvanie")] public string Name { get; set; } = string.Empty;
        [Column("model")] public string Model { get; set; } = string.Empty;
        [Column("serijnyj_nomer")] public string SerialNumber { get; set; } = string.Empty;
        [Column("data_vypuska")] public DateTime ReleaseDate { get; set; }
        [Column("data_vvoda")] public DateTime CommissionDate { get; set; }
        [Column("srok_sluzhby_let")] public int ServiceLife { get; set; }

        [Column("status_id")] public int StatusId { get; set; }
        [ForeignKey("StatusId")] public EquipmentStatus? Status { get; set; }

        [Column("mesto_id")] public int CabinetId { get; set; }
        [ForeignKey("CabinetId")] public Cabinet? Cabinet { get; set; }

        [Column("otvetstvennyj_id")] public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")] public Employee? Employee { get; set; }

        // Связь с техобслуживанием (одно оборудование - много записей ТО)
        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
    }
}
