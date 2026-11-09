using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    [Table("zdaniya")]
    public class Building
    {
        [Key][Column("id")] public int Id { get; set; }
        [Column("adres")] public string Address { get; set; } = string.Empty;

        // Навигационное свойство: в одном здании много кабинетов
        public ICollection<Cabinet> Cabinets { get; set; } = new List<Cabinet>();
    }
}
