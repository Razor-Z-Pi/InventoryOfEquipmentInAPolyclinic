using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    [Table("podrazdeleniya")]
    public class Department
    {
        [Key][Column("id")] public int Id { get; set; }
        [Column("nazvanie")] public string Name { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
