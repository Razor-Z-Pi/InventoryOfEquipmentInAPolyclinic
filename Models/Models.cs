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

    // Справочник Статусов
    public class EquipmentStatus
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Справочник Кабинетов
    public class Cabinet
    {
        [Key] public int Id { get; set; }
        public int AddressId { get; set; } // FK
        public string Name { get; set; } = string.Empty;
        public int Floor { get; set; }
        public string CabinetNumber { get; set; } = string.Empty;
    }

    // Справочник Должностей
    public class Position
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Справочник Подразделений
    public class Department
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Сотрудники
    public class Employee
    {
        [Key] public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int PositionId { get; set; } // FK
        public int DepartmentId { get; set; } // FK
    }

    // Главная таблица Оборудования
    public class Equipment
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public DateTime CommissionDate { get; set; }
        public int ServiceLife { get; set; } // лет
        public int StatusId { get; set; } // FK
        public int CabinetId { get; set; } // FK
        public int EmployeeId { get; set; } // FK
    }
}
