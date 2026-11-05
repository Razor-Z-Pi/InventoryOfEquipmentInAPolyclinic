using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InventoryOfEquipmentInAPolyclinic.Services.Data;

namespace InventoryOfEquipmentInAPolyclinic.Services
{
    public class BackupData
    {
        public List<Models.Address> Addresses { get; set; } = new();
        public List<Models.EquipmentStatus> Statuses { get; set; } = new();
        public List<Models.Cabinet> Cabinets { get; set; } = new();
        public List<Models.Position> Positions { get; set; } = new();
        public List<Models.Department> Departments { get; set; } = new();
        public List<Models.Employee> Employees { get; set; } = new();
        public List<Models.Equipment> Equipments { get; set; } = new();
        public List<Models.User> Users { get; set; } = new();
        public List<Models.AuditLog> AuditLogs { get; set; } = new();
    }

    public static class BackupService
    {
        public static void SaveBackup(string filePath)
        {
            using var db = new AppDbContext();

            var backup = new BackupData
            {
                Addresses = db.Addresses.ToList(),
                Statuses = db.Statuses.ToList(),
                Cabinets = db.Cabinets.ToList(),
                Positions = db.Positions.ToList(),
                Departments = db.Departments.ToList(),
                Employees = db.Employees.ToList(),
                Equipments = db.Equipments.ToList(),
                Users = db.Users.ToList(),
                AuditLogs = db.AuditLogs.ToList()
            };

            var json = JsonSerializer.Serialize(backup, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void RestoreBackup(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var backup = JsonSerializer.Deserialize<BackupData>(json);

            if (backup == null) throw new Exception("Не удалось прочитать файл бэкапа.");

            using var db = new AppDbContext();

            // Очищаем текущие данные
            db.AuditLogs.RemoveRange(db.AuditLogs);
            db.Equipments.RemoveRange(db.Equipments);
            db.Employees.RemoveRange(db.Employees);
            db.Cabinets.RemoveRange(db.Cabinets);
            db.Departments.RemoveRange(db.Departments);
            db.Positions.RemoveRange(db.Positions);
            db.Statuses.RemoveRange(db.Statuses);
            db.Addresses.RemoveRange(db.Addresses);
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();

            // Загружаем из бэкапа (сначала справочники, потом зависимые таблицы)
            db.Addresses.AddRange(backup.Addresses);
            db.Statuses.AddRange(backup.Statuses);
            db.Positions.AddRange(backup.Positions);
            db.Departments.AddRange(backup.Departments);
            db.Cabinets.AddRange(backup.Cabinets);
            db.Employees.AddRange(backup.Employees);
            db.Users.AddRange(backup.Users);
            db.Equipments.AddRange(backup.Equipments);
            db.AuditLogs.AddRange(backup.AuditLogs);
            db.SaveChanges();
        }
    }
}
