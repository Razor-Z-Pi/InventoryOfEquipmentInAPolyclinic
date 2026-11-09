using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOfEquipmentInAPolyclinic.Models;

namespace InventoryOfEquipmentInAPolyclinic.Services
{
    public class DatabaseContext
    {
        public ObservableCollection<Address> Addresses { get; } = new();
        public ObservableCollection<EquipmentStatus> Statuses { get; } = new();
        public ObservableCollection<Cabinet> Cabinets { get; } = new();
        public ObservableCollection<Position> Positions { get; } = new();
        public ObservableCollection<Department> Departments { get; } = new();
        public ObservableCollection<Employee> Employees { get; } = new();
        public ObservableCollection<Equipment> Equipments { get; } = new();

        public DatabaseContext()
        {
            SeedData();
        }

        private void SeedData()
        {
            // Адреса
            Addresses.Add(new Address { Id = 1, FullAddress = "Г. Абакан, ул. Джугашвили, 67" });

            // Статусы
            Statuses.Add(new EquipmentStatus { Id = 1, Name = "В эксплуатации" });
            Statuses.Add(new EquipmentStatus { Id = 2, Name = "На ремонте" });
            Statuses.Add(new EquipmentStatus { Id = 3, Name = "списано" });

            // Должности
            Positions.Add(new Position { Id = 1, Name = "Старшая медсестра" });
            Positions.Add(new Position { Id = 2, Name = "Врач-терапевт" });
            Positions.Add(new Position { Id = 3, Name = "Врач-гинеколог" });
            Positions.Add(new Position { Id = 4, Name = "Главврач" });

            // Подразделения
            Departments.Add(new Department { Id = 1, Name = "Отделение ультразвуковой диагностики" });
            Departments.Add(new Department { Id = 2, Name = "Отделение функциональной диагностики" });
            Departments.Add(new Department { Id = 3, Name = "Мужская консультация" });
            Departments.Add(new Department { Id = 4, Name = "Административно-управленческий персонал (АУП)" });

            // Сотрудники
            Employees.Add(new Employee { Id = 1, FullName = "Иванова Мария Петровна", PositionId = 1, DepartmentId = 1 });
            Employees.Add(new Employee { Id = 2, FullName = "Козлов Иван Валерьевич", PositionId = 2, DepartmentId = 2 });
            Employees.Add(new Employee { Id = 3, FullName = "Тимошенко Пётр Игоревич", PositionId = 3, DepartmentId = 3 });
            Employees.Add(new Employee { Id = 4, FullName = "Глухарев Акакий Иоанович", PositionId = 4, DepartmentId = 4 });

            // Оборудование
            Equipments.Add(new Equipment { Id = 1, Name = "Аппарат УЗИ", Model = "Mindray DC-70", SerialNumber = "SN-MD-874512", ReleaseDate = new DateTime(2021, 03, 15), CommissionDate = new DateTime(2021, 05, 10), ServiceLife = 10, StatusId = 1, CabinetId = 1, EmployeeId = 1 });
            Equipments.Add(new Equipment { Id = 2, Name = "Электрокардиограф", Model = "SCHILLER CARDIODIT AT-102", SerialNumber = "SN-SHD-20026", ReleaseDate = new DateTime(2019, 11, 22), CommissionDate = new DateTime(2020, 01, 15), ServiceLife = 8, StatusId = 1, CabinetId = 1, EmployeeId = 2 });
            Equipments.Add(new Equipment { Id = 3, Name = "Тонометр", Model = "Omron M3 Comfort", SerialNumber = "SN-SX-84892", ReleaseDate = new DateTime(2022, 07, 08), CommissionDate = new DateTime(2022, 08, 20), ServiceLife = 5, StatusId = 1, CabinetId = 2, EmployeeId = 3 });
            Equipments.Add(new Equipment { Id = 4, Name = "Компьютер рабочий", Model = "HP ProDesk 400 G7", SerialNumber = "SN-DF-02982", ReleaseDate = new DateTime(2023, 02, 23), CommissionDate = new DateTime(2023, 12, 22), ServiceLife = 5, StatusId = 2, CabinetId = 1, EmployeeId = 2 });
            Equipments.Add(new Equipment { Id = 5, Name = "Дефибриллятор", Model = "ZOLL R Series", SerialNumber = "SN-SM-37828", ReleaseDate = new DateTime(2017, 10, 27), CommissionDate = new DateTime(2018, 09, 01), ServiceLife = 10, StatusId = 1, CabinetId = 3, EmployeeId = 1 });
        }
    }
}
