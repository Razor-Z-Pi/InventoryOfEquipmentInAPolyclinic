using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InventoryOfEquipmentInAPolyclinic.Models
{
    // Справочник Адресов
    public class Address
    {
        [Key] public int Id { get; set; }
        public string FullAddress { get; set; } = string.Empty;
    }
}
