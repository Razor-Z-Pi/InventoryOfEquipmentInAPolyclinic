using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    [Table("sotrudniki")]
    public class Employee
    {
        [Key][Column("id")] public int Id { get; set; }
        [Column("fio")] public string FullName { get; set; } = string.Empty;

        [Column("dolzhnost_id")] public int PositionId { get; set; }
        [ForeignKey("PositionId")] public Position? Position { get; set; }

        [Column("podrazdelenie_id")] public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")] public Department? Department { get; set; }

        public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    }
}
