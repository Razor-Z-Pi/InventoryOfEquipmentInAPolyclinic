using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    [Table("to_obsluzhivanie")]
    public class Maintenance
    {
        [Key][Column("id")] public int Id { get; set; }

        [Column("oborudovanie_id")] public int EquipmentId { get; set; }
        [ForeignKey("EquipmentId")] public Equipment? Equipment { get; set; }

        [Column("data_to")] public DateTime DateTo { get; set; }
        [Column("data_sleduyushchego")] public DateTime DateNext { get; set; }

        // Enum в MariaDB мапится на string. "плановое", "внеплановое"
        [Column("tip")] public string Type { get; set; } = string.Empty;
        [Column("kommentarij")] public string Comment { get; set; } = string.Empty;
    }
}
