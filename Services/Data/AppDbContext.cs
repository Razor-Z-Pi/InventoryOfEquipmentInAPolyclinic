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
        // Таблицы в БД
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<EquipmentStatus> Statuses { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        // Настройка подключения к MariaDB
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigService.GetConnectionString();
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        // Настройка связей
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь Оборудование -> Статус
            modelBuilder.Entity<Equipment>()
                .HasOne<EquipmentStatus>()
                .WithMany()
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь Оборудование -> Кабинет
            modelBuilder.Entity<Equipment>()
                .HasOne<Cabinet>()
                .WithMany()
                .HasForeignKey(e => e.CabinetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь Оборудование -> Сотрудник
            modelBuilder.Entity<Equipment>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Адреса
            modelBuilder.Entity<Address>().HasData(
                new Address { Id = 1, FullAddress = "Г. Абакан, ул. Джугашвили, 67" }
            );

            // Статусы
            modelBuilder.Entity<EquipmentStatus>().HasData(
                new EquipmentStatus { Id = 1, Name = "В эксплуатации" },
                new EquipmentStatus { Id = 2, Name = "На ремонте" },
                new EquipmentStatus { Id = 3, Name = "списано" }
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

            // Кабинеты (Внимание: AddressId = 1)
            modelBuilder.Entity<Cabinet>().HasData(
                new Cabinet { Id = 1, AddressId = 1, Name = "Кабинет терапевта", Floor = 3, CabinetNumber = "301" },
                new Cabinet { Id = 2, AddressId = 1, Name = "Кабинет гинеколога", Floor = 3, CabinetNumber = "314" },
                new Cabinet { Id = 3, AddressId = 1, Name = "Лаборатория", Floor = 1, CabinetNumber = "108" },
                new Cabinet { Id = 4, AddressId = 1, Name = "Стерилизационная", Floor = 1, CabinetNumber = "111" }
            );

            // Сотрудники (PositionId и DepartmentId из таблиц выше)
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FullName = "Иванова Мария Петровна", PositionId = 1, DepartmentId = 1 },
                new Employee { Id = 2, FullName = "Козлов Иван Валерьевич", PositionId = 2, DepartmentId = 2 },
                new Employee { Id = 3, FullName = "Тимошенко Пётр Игоревич", PositionId = 3, DepartmentId = 3 },
                new Employee { Id = 4, FullName = "Глухарев Акакий Иоанович", PositionId = 4, DepartmentId = 4 }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = "admin123", Role = "Admin" },
                new User { Id = 2, Username = "user", PasswordHash = "user123", Role = "User" }
            );
        }
    }
}
