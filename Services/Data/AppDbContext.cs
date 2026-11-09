using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOfEquipmentInAPolyclinic.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryOfEquipmentInAPolyclinic.Services.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentStatus> Statuses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigService.GetConnectionString();
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи Здание -> Кабинеты (Один ко многим)
            modelBuilder.Entity<Cabinet>()
                .HasOne(c => c.Building)
                .WithMany(b => b.Cabinets)
                .HasForeignKey(c => c.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связи Сотрудник -> Должность/Подразделение
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связи Оборудование -> Техобслуживание
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Equipment)
                .WithMany(e => e.Maintenances)
                .HasForeignKey(m => m.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Здания
            modelBuilder.Entity<Building>().HasData(
                new Building { Id = 1, Address = "Г. Абакан, ул. Джугашвили, 67" }
            );

            // Должности
            modelBuilder.Entity<Position>().HasData(
                new Position { Id = 1, Name = "Старшая медсестра" },
                new Position { Id = 2, Name = "Врач-терапевт" },
                new Position { Id = 3, Name = "Врач-гинеколог" },
                new Position { Id = 4, Name = "Главврач" }
            );

            // Подразделения
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Отделение ультразвуковой диагностики" },
                new Department { Id = 2, Name = "Отделение функциональной диагностики" },
                new Department { Id = 3, Name = "Мужская консультация" },
                new Department { Id = 4, Name = "Административно-управленческий персонал (АУП)" }
            );

            // Статусы оборудования
            modelBuilder.Entity<EquipmentStatus>().HasData(
                new EquipmentStatus { Id = 1, Name = "В эксплуатации" },
                new EquipmentStatus { Id = 2, Name = "На ремонте" },
                new EquipmentStatus { Id = 3, Name = "списано" }
            );

            // Кабинеты
            modelBuilder.Entity<Cabinet>().HasData(
                new Cabinet { Id = 1, BuildingId = 1, Name = "Кабинет терапевта", Floor = 3, CabinetNumber = "301" },
                new Cabinet { Id = 2, BuildingId = 1, Name = "Кабинет гинеколога", Floor = 3, CabinetNumber = "314" },
                new Cabinet { Id = 3, BuildingId = 1, Name = "Лаборатория", Floor = 1, CabinetNumber = "108" },
                new Cabinet { Id = 4, BuildingId = 1, Name = "Стерилизационная", Floor = 1, CabinetNumber = "111" }
            );

            // Сотрудники
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FullName = "Иванова Мария Петровна", PositionId = 1, DepartmentId = 1 },
                new Employee { Id = 2, FullName = "Козлов Иван Валерьевич", PositionId = 2, DepartmentId = 2 },
                new Employee { Id = 3, FullName = "Тимошенко Пётр Игоревич", PositionId = 3, DepartmentId = 3 },
                new Employee { Id = 4, FullName = "Глухарев Акакий Иоанович", PositionId = 4, DepartmentId = 4 }
            );

            // Пользователи
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = "admin123", Role = "Admin" },
                new User { Id = 2, Username = "user", PasswordHash = "user123", Role = "User" }
            );
        }
    }
}
