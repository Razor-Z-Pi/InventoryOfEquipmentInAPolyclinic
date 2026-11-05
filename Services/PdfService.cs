using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOfEquipmentInAPolyclinic.Models;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace InventoryOfEquipmentInAPolyclinic.Services
{
    public static class PdfService
    {
        public static void GenerateWriteOffAct(EquipmentDisplay equipment, string filePath)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.UseSystemFonts = true;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Times New Roman"));

                    // Шапка документа
                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text("МИНИСТЕРСТВО ЗДРАВООХРАНЕНИЯ").Bold();
                        col.Item().AlignCenter().Text("ГБУЗ «Поликлиника №1»").Bold();
                        col.Item().PaddingTop(20).AlignCenter().Text("АКТ О СПИСАНИИ ОБОРУДОВАНИЯ").FontSize(16).Bold();
                        col.Item().PaddingTop(10).AlignRight().Text($"Дата: {DateTime.Now:dd.MM.yyyy}").FontSize(10);
                    });

                    // Контент
                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        col.Item().Text("Комиссия в составе:").Bold();
                        col.Item().Text("Председатель: Главный врач _______________________");
                        col.Item().Text("Члены комиссии: Старшая медсестра _______________________");
                        col.Item().Text("                Врач-терапевт _______________________");

                        col.Item().PaddingTop(20).Text("Произвела осмотр и установила, что следующее оборудование подлежит списанию:");

                        // Таблица с данными
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30); // Номер
                                columns.RelativeColumn(3);  // Название
                                columns.RelativeColumn(2);  // Модель
                                columns.RelativeColumn(2);  // SN
                                columns.RelativeColumn(1);  // Срок
                            });

                            // Шапка таблицы
                            table.Header(header =>
                            {
                                header.Cell().Border(1).Padding(5).AlignCenter().Text("№").Bold();
                                header.Cell().Border(1).Padding(5).Text("Наименование").Bold();
                                header.Cell().Border(1).Padding(5).Text("Модель").Bold();
                                header.Cell().Border(1).Padding(5).Text("Серийный номер").Bold();
                                header.Cell().Border(1).Padding(5).AlignCenter().Text("Срок службы").Bold();
                            });

                            // Данные
                            table.Cell().Border(1).Padding(5).AlignCenter().Text("1");
                            table.Cell().Border(1).Padding(5).Text(equipment.Name);
                            table.Cell().Border(1).Padding(5).Text(equipment.Model);
                            table.Cell().Border(1).Padding(5).Text(equipment.SerialNumber);
                            table.Cell().Border(1).Padding(5).AlignCenter().Text($"{equipment.ServiceLife} лет");
                        });

                        col.Item().PaddingTop(10).Text($"Причина списания: {equipment.LifeStatus}. Оборудование выработало установленный срок службы.");

                        // Подписи
                        col.Item().PaddingTop(40).Row(row =>
                        {
                            row.RelativeItem().Text("Председатель комиссии: ______________");
                            row.RelativeItem().Text("Члены комиссии: ______________");
                        });
                    });

                    // Футер
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf(filePath);
        }
    }
}
