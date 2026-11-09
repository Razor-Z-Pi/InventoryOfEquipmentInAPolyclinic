using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    [Table("mesta")]
    public class Cabinet
    {
        [Key][Column("id")] public int Id { get; set; }
        [Column("nazvanie_kabineta")] public string Name { get; set; } = string.Empty;
        [Column("etazh")] public int Floor { get; set; }
        [Column("nomer_kabineta")] public string CabinetNumber { get; set; } = string.Empty;

        [Column("zdanie_id")] public int BuildingId { get; set; }
        [ForeignKey("BuildingId")] public Building? Building { get; set; }

        // В одном кабинете может быть много оборудования
        public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    }
}
