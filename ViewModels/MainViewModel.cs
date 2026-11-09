using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventoryOfEquipmentInAPolyclinic.Models;
using InventoryOfEquipmentInAPolyclinic.Services;
using InventoryOfEquipmentInAPolyclinic.Services.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace InventoryOfEquipmentInAPolyclinic.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly AppDbContext _db;

        // Коллекция для DataGrid
        [ObservableProperty]
        private ObservableCollection<EquipmentDisplay> _equipmentsDisplay;

        [ObservableProperty]
        private ObservableCollection<Equipment> _equipments;

        // Выбранный элемент
        [ObservableProperty]
        private EquipmentDisplay? _selectedEquipment;

        // Коллекции для ComboBox
        [ObservableProperty]
        private ObservableCollection<EquipmentStatus> _statuses;
        [ObservableProperty]
        private ObservableCollection<Cabinet> _cabinets;
        [ObservableProperty]
        private ObservableCollection<Employee> _employees;

        // Выбранные значения в ComboBox для нового оборудования
        [ObservableProperty] private EquipmentStatus? _selectedStatus;
        [ObservableProperty] private Cabinet? _selectedCabinet;
        [ObservableProperty] private Employee? _selectedEmployee;

        [ObservableProperty] private string _newName = string.Empty;
        [ObservableProperty] private string _newModel = string.Empty;

        [ObservableProperty]
        private string _searchText = string.Empty;

        private List<EquipmentDisplay> _allEquipments = new();

        [ObservableProperty] private int _totalEquipmentCount;
        [ObservableProperty] private int _inRepairCount;
        [ObservableProperty] private int _inUseCount;
        [ObservableProperty] private int _writtenOffCount;

        // Свойства для кабинетов
        [ObservableProperty] private string _newCabinetName = string.Empty;
        [ObservableProperty] private int _newCabinetFloor = 1;
        [ObservableProperty] private string _newCabinetNumber = string.Empty;

        // Свойства для сотрудников
        [ObservableProperty] private string _newEmployeeName = string.Empty;
        [ObservableProperty] private Position? _selectedPosition;
        [ObservableProperty] private Department? _selectedDepartment;

        // Коллекции для ComboBox в справочниках
        [ObservableProperty] private ObservableCollection<Position> _positions;
        [ObservableProperty] private ObservableCollection<Department> _departments;

        // Серии для круговой диаграммы (Статусы)
        [ObservableProperty] private ISeries[] _statusSeries;

        // Серии для столбчатой диаграммы (Кабинеты)
        [ObservableProperty] private ISeries[] _cabinetSeries;
        [ObservableProperty] private Axis[] _cabinetXAxes;

        [ObservableProperty] private string _currentUserName;
        [ObservableProperty] private string _currentUserRole;
        [ObservableProperty] private bool _isAdmin;

        [ObservableProperty] private ObservableCollection<AuditLog> _auditLogs;

        [ObservableProperty] private string _toastMessage = string.Empty;
        [ObservableProperty] private bool _isToastVisible;

        private System.Windows.Threading.DispatcherTimer _toastTimer;

        [ObservableProperty] private EquipmentStatus? _selectedEquipmentStatus;

        [ObservableProperty] private ObservableCollection<Maintenance> _maintenances;
        [ObservableProperty] private string _newMaintenanceType = "плановое";
        [ObservableProperty] private DateTime _newMaintenanceDate = DateTime.Now;
        [ObservableProperty] private string _newMaintenanceComment = string.Empty;

        // Конструктор для XAML (создает свой контекст)
        public MainViewModel() : this(new AppDbContext())
        {
        }

        // Основной конструктор
        public MainViewModel(AppDbContext db)
        {
            _db = db;
            EquipmentsDisplay = new ObservableCollection<EquipmentDisplay>();
            Statuses = new ObservableCollection<EquipmentStatus>();
            Cabinets = new ObservableCollection<Cabinet>();
            Employees = new ObservableCollection<Employee>();
            Positions = new ObservableCollection<Position>();
            Departments = new ObservableCollection<Department>();
            Maintenances = new ObservableCollection<Maintenance>();

            StatusSeries = Array.Empty<ISeries>();
            CabinetSeries = Array.Empty<ISeries>();
            CabinetXAxes = Array.Empty<Axis>();

            AuditLogs = new ObservableCollection<AuditLog>();

            if (App.CurrentUser != null)
            {
                CurrentUserName = App.CurrentUser.Username;
                CurrentUserRole = App.CurrentUser.Role;
                IsAdmin = CurrentUserRole == "Admin";
            }

            _toastTimer = new System.Windows.Threading.DispatcherTimer();
            _toastTimer.Interval = TimeSpan.FromSeconds(3);
            _toastTimer.Tick += (s, e) =>
            {
                IsToastVisible = false;
                _toastTimer.Stop();
            };

            _ = LoadDataAsync();
        }

        // Загрузка данных из MariaDB
        public async Task LoadDataAsync()
        {
            try
            {
                // Загружаем справочники для ComboBox
                var statuses = await Task.Run(() => _db.Statuses.ToList());
                var cabinets = await Task.Run(() => _db.Cabinets.ToList());
                var employees = await Task.Run(() => _db.Employees.ToList());
                var positions = await Task.Run(() => _db.Positions.ToList());
                var departments = await Task.Run(() => _db.Departments.ToList());
                var logs = await Task.Run(() => _db.AuditLogs.OrderByDescending(l => l.Timestamp).Take(100).ToList());

                Positions.Clear(); foreach (var p in positions) Positions.Add(p);
                Departments.Clear(); foreach (var d in departments) Departments.Add(d);

                Statuses.Clear(); foreach (var s in statuses) Statuses.Add(s);
                Cabinets.Clear(); foreach (var c in cabinets) Cabinets.Add(c);
                Employees.Clear(); foreach (var e in employees) Employees.Add(e);

                AuditLogs.Clear();

                foreach (var log in logs) AuditLogs.Add(log);

                // Устанавливаем значения по умолчанию (первый элемент)
                SelectedStatus = Statuses.FirstOrDefault();
                SelectedCabinet = Cabinets.FirstOrDefault();
                SelectedEmployee = Employees.FirstOrDefault();

                // Загружаем ТО только для выбранного оборудования
                if (SelectedEquipment != null)
                {
                    var toData = await Task.Run(() =>
                        _db.Maintenances
                           .Where(m => m.EquipmentId == SelectedEquipment.Id)
                           .OrderByDescending(m => m.DateTo)
                           .ToList());

                    Maintenances.Clear();
                    foreach (var item in toData) Maintenances.Add(item);
                }

                // Загружаем само оборудование с "JOIN"
                var equipments = await Task.Run(() =>
                    _db.Equipments
                       .Select(eq => new EquipmentDisplay
                       {
                           Id = eq.Id,
                           Name = eq.Name,
                           Model = eq.Model,
                           SerialNumber = eq.SerialNumber,
                           StatusName = _db.Statuses.Where(s => s.Id == eq.StatusId).Select(s => s.Name).FirstOrDefault(),
                           CabinetName = _db.Cabinets.Where(c => c.Id == eq.CabinetId).Select(c => c.Name).FirstOrDefault(),
                           EmployeeName = _db.Employees.Where(emp => emp.Id == eq.EmployeeId).Select(emp => emp.FullName).FirstOrDefault(),
                           OriginalEquipment = eq,
                           CommissionDate = eq.CommissionDate,
                           ServiceLife = eq.ServiceLife
                       }).ToList()
                );

                _allEquipments = equipments;
                FilterEquipments();
                UpdateStatistics();
                UpdateCharts();

                EquipmentsDisplay.Clear();
                foreach (var item in equipments) EquipmentsDisplay.Add(item);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void FilterEquipments()
        {
            EquipmentsDisplay.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allEquipments
                : _allEquipments.Where(e =>
                    e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    e.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    e.SerialNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    e.EmployeeName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || // Поиск по ФИО
                    e.CabinetName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||  // Поиск по кабинету
                    e.StatusName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)      // Поиск по статусу
                    ).ToList();

            foreach (var item in filtered)
            {
                EquipmentsDisplay.Add(item);
            }

            UpdateStatistics();
            UpdateCharts();
        }

        private void UpdateStatistics()
        {
            TotalEquipmentCount = _allEquipments.Count;
            InRepairCount = _allEquipments.Count(e => e.StatusName == "На ремонте");
            InUseCount = _allEquipments.Count(e => e.StatusName == "В эксплуатации");
            WrittenOffCount = _allEquipments.Count(e => e.StatusName == "списано");
        }

        private void UpdateCharts()
        {
            // Группируем данные
            var statusGroups = _allEquipments
                .GroupBy(e => e.StatusName)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToList();

            var cabinetGroups = _allEquipments
                .GroupBy(e => e.CabinetName)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            // Обновляем UI-поток
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                // Круговая диаграмма
                StatusSeries = statusGroups.Select(g => (ISeries)new PieSeries<int>
                {
                    Values = new[] { g.Count },
                    Name = g.Name,
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    DataLabelsSize = 14,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle
                }).ToArray();

                // Столбчатая диаграмма
                CabinetSeries = new ISeries[]
                {
                new ColumnSeries<int>
                    {
                        Values = cabinetGroups.Select(g => g.Count).ToArray(),
                        Fill = new SolidColorPaint(new SKColor(52, 152, 219)), // Синий
                        Name = "Оборудование"
                    }
                };

                CabinetXAxes = new[]
                {
                new Axis
                    {
                        Labels = cabinetGroups.Select(g => g.Name).ToArray(),
                        LabelsRotation = 15,
                        TextSize = 12
                    }
                };
            });
        }

        public void ShowToast(string message)
        {
            ToastMessage = message;
            IsToastVisible = true;
            _toastTimer.Stop();
            _toastTimer.Start();
        }

        [RelayCommand]
        private async Task UpdateStatusAsync()
        {
            if (SelectedEquipment == null || SelectedEquipmentStatus == null) return;

            try
            {
                SelectedEquipment.OriginalEquipment.StatusId = SelectedEquipmentStatus.Id;
                await _db.SaveChangesAsync();

                // Обновляем отображение
                SelectedEquipment.StatusName = SelectedEquipmentStatus.Name;

                // Обновляем цвет в таблице (для триггеров)
                OnPropertyChanged(nameof(SelectedEquipment));

                ShowToast($"Статус изменен на '{SelectedEquipmentStatus.Name}'");
                UpdateStatistics();
                UpdateCharts();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddEquipmentAsync()
        {
            if (string.IsNullOrWhiteSpace(NewName) || SelectedStatus == null || SelectedCabinet == null || SelectedEmployee == null)
            {
                System.Windows.MessageBox.Show("Заполните название и выберите значения из списков!");
                return;
            }

            try
            {
                var newItem = new Equipment
                {
                    Name = NewName,
                    Model = NewModel,
                    StatusId = SelectedStatus.Id,     // Берем ID из выбранного ComboBox
                    CabinetId = SelectedCabinet.Id,
                    EmployeeId = SelectedEmployee.Id,
                    ReleaseDate = DateTime.Now,
                    CommissionDate = DateTime.Now,
                    ServiceLife = 5,
                    SerialNumber = "SN-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
                };

                _db.Equipments.Add(newItem);
                _db.AuditLogs.Add(new AuditLog
                {
                    Username = App.CurrentUser?.Username ?? "Unknown",
                    Action = "Добавление",
                    Details = $"Добавлено оборудование: {NewName} ({NewModel})"
                });
                await _db.SaveChangesAsync();
                ShowToast("Оборудование успешно добавлено!!!");

                // Очищаем поля и перезагружаем таблицу
                NewName = string.Empty;
                NewModel = string.Empty;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                string realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                System.Windows.MessageBox.Show($"Ошибка добавления: {realError}");
            }
        }

        [RelayCommand]
        private async Task DeleteEquipmentAsync()
        {
            if (!IsAdmin)
            {
                System.Windows.MessageBox.Show("У вас нет прав на удаление оборудования. Обратитесь к администратору.", "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedEquipment == null) return;

            var result = System.Windows.MessageBox.Show($"Удалить {SelectedEquipment.Name}?", "Подтверждение", System.Windows.MessageBoxButton.YesNo);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    _db.Equipments.Remove(SelectedEquipment.OriginalEquipment);
                    _db.AuditLogs.Add(new AuditLog
                    {
                        Username = App.CurrentUser?.Username ?? "Unknown",
                        Action = "Удаление",
                        Details = $"Удалено оборудование: {SelectedEquipment.Name} ({SelectedEquipment.Model})"
                    });
                    await _db.SaveChangesAsync();
                    await LoadDataAsync();
                    ShowToast("Оборудование удалено!!!");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка удаления: {ex.Message}");
                }
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            FilterEquipments();
        }

        [RelayCommand]
        private async Task SaveChangesAsync()
        {
            try
            {
                // Проходим по всем отображаемым элементам и синхронизируем с оригиналом
                foreach (var displayItem in EquipmentsDisplay)
                {
                    if (displayItem.OriginalEquipment != null)
                    {
                        if (string.IsNullOrWhiteSpace(displayItem.Name))
                        {
                            System.Windows.MessageBox.Show("Название оборудования не может быть пустым!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                            // Перезагружаем данные, чтобы откатить пустую строку в таблице
                            await LoadDataAsync();
                            return;
                        }
                        // Обновляем реальные поля в сущности БД
                        displayItem.OriginalEquipment.Name = displayItem.Name;
                        displayItem.OriginalEquipment.Model = displayItem.Model;
                        displayItem.OriginalEquipment.SerialNumber = displayItem.SerialNumber;
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ExportToExcel()
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Оборудование");

                // Заголовки
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Название";
                worksheet.Cell(1, 3).Value = "Модель";
                worksheet.Cell(1, 4).Value = "Серийный номер";
                worksheet.Cell(1, 5).Value = "Статус";
                worksheet.Cell(1, 6).Value = "Кабинет";
                worksheet.Cell(1, 7).Value = "Сотрудник";

                // Данные
                for (int i = 0; i < _allEquipments.Count; i++)
                {
                    var item = _allEquipments[i];
                    worksheet.Cell(i + 2, 1).Value = item.Id;
                    worksheet.Cell(i + 2, 2).Value = item.Name;
                    worksheet.Cell(i + 2, 3).Value = item.Model;
                    worksheet.Cell(i + 2, 4).Value = item.SerialNumber;
                    worksheet.Cell(i + 2, 5).Value = item.StatusName;
                    worksheet.Cell(i + 2, 6).Value = item.CabinetName;
                    worksheet.Cell(i + 2, 7).Value = item.EmployeeName;
                }

                worksheet.Columns().AdjustToContents();

                string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Оборудование.xlsx");
                workbook.SaveAs(filePath);
                System.Windows.MessageBox.Show($"Сохранено в {filePath}");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка экспорта: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ExportToPdf()
        {
            try
            {
                var dataToExport = EquipmentsDisplay.ToList();

                if (!dataToExport.Any())
                {
                    System.Windows.MessageBox.Show("Нет данных для экспорта.");
                    return;
                }

                QuestPDF.Settings.License = LicenseType.Community;
                                
                string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Оборудование.pdf");

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(1, Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        page.Header().Text("Список оборудования").SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);

                        page.Content().PaddingVertical(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30); // ID
                                columns.RelativeColumn();   // Название
                                columns.RelativeColumn();   // Модель
                                columns.RelativeColumn();   // SN
                                columns.RelativeColumn();   // Статус
                            });

                            // Шапка таблицы
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Название").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Модель").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("SN").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Статус").Bold();
                            });

                            // Строки
                            foreach (var item in _allEquipments)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Name);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Model);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SerialNumber);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.StatusName);
                            }
                        });
                    });
                }).GeneratePdf(filePath);

                System.Windows.MessageBox.Show($"PDF сохранен в {filePath}");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка PDF: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddCabinetAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCabinetName)) return;

            var newCab = new Cabinet
            {
                Name = NewCabinetName,
                Floor = NewCabinetFloor,
                CabinetNumber = NewCabinetNumber,
            };

            _db.Cabinets.Add(newCab);
            await _db.SaveChangesAsync();

            Cabinets.Add(newCab); // Обновляем список
            NewCabinetName = string.Empty;
            NewCabinetNumber = string.Empty;
        }

        [RelayCommand]
        private async Task AddEmployeeAsync()
        {
            if (string.IsNullOrWhiteSpace(NewEmployeeName) || SelectedPosition == null || SelectedDepartment == null)
            {
                MessageBox.Show("Заполните ФИО и выберите должность/подразделение");
                return;
            }

            var newEmp = new Employee
            {
                FullName = NewEmployeeName,
                PositionId = SelectedPosition.Id,
                DepartmentId = SelectedDepartment.Id
            };

            _db.Employees.Add(newEmp);
            await _db.SaveChangesAsync();

            Employees.Add(newEmp);
            NewEmployeeName = string.Empty;
        }

        [RelayCommand]
        private async Task MarkAsRepairAsync()
        {
            if (SelectedEquipment == null) return;
        }

        [RelayCommand]
        private void CreateBackup()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "JSON Backup (*.json)|*.json",
                FileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    BackupService.SaveBackup(dialog.FileName);
                    ShowToast("Резервная копия успешно создана!");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка бэкапа: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private async Task RestoreBackupAsync()
        {
            if (!IsAdmin)
            {
                System.Windows.MessageBox.Show("Только администратор может восстанавливать базу!");
                return;
            }

            var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "JSON Backup (*.json)|*.json" };
            if (dialog.ShowDialog() == true)
            {
                var result = System.Windows.MessageBox.Show(
                    "ВНИМАНИЕ! Все текущие данные будут заменены данными из бэкапа. Продолжить?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        BackupService.RestoreBackup(dialog.FileName);
                        await LoadDataAsync(); // Перезагружаем интерфейс
                        ShowToast("База данных успешно восстановлена!");
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Ошибка восстановления: {ex.Message}");
                    }
                }
            }
        }

        [RelayCommand]
        private async Task ImportFromExcelAsync()
        {
            if (!IsAdmin)
            {
                System.Windows.MessageBox.Show("Только администратор может импортировать данные!!!", "Доступ запрещен");
                return;
            }

            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Выберите файл для импорта"
            };

            if (dialog.ShowDialog() != true) return;

            try
            {
                int successCount = 0;
                int errorCount = 0;

                using var workbook = new XLWorkbook(dialog.FileName);
                var worksheet = workbook.Worksheet(1); // Берем первый лист
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Пропускаем первую строку (заголовки)

                foreach (var row in rows)
                {
                    try
                    {
                        // Читаем данные из ячеек
                        string name = row.Cell(1).GetString();
                        string model = row.Cell(2).GetString();
                        string serial = row.Cell(3).GetString();

                        if (string.IsNullOrWhiteSpace(name)) continue; // Пропускаем пустые строки

                        // Парсим числа и даты (с защитой от ошибок)
                        int statusId = row.Cell(4).TryGetValue<int>(out var s) ? s : 1;
                        int cabinetId = row.Cell(5).TryGetValue<int>(out var c) ? c : 1;
                        int employeeId = row.Cell(6).TryGetValue<int>(out var e) ? e : 1;

                        DateTime commissionDate = row.Cell(7).TryGetValue<DateTime>(out var d) ? d : DateTime.Now;
                        int serviceLife = row.Cell(8).TryGetValue<int>(out var l) ? l : 5;

                        // Создаем новую запись
                        var newEquipment = new Equipment
                        {
                            Name = name,
                            Model = model,
                            SerialNumber = serial,
                            StatusId = statusId,
                            CabinetId = cabinetId,
                            EmployeeId = employeeId,
                            CommissionDate = commissionDate,
                            ReleaseDate = commissionDate, // Упрощаем, ставим ту же дату
                            ServiceLife = serviceLife
                        };

                        _db.Equipments.Add(newEquipment);
                        successCount++;
                    }
                    catch
                    {
                        errorCount++; // Если строка битая, пропускаем её
                    }
                }

                await _db.SaveChangesAsync();

                // Записываем в лог
                _db.AuditLogs.Add(new AuditLog
                {
                    Username = App.CurrentUser?.Username ?? "Unknown",
                    Action = "Импорт",
                    Details = $"Импортировано из Excel: {successCount} записей. Ошибок: {errorCount}"
                });
                await _db.SaveChangesAsync();

                await LoadDataAsync(); // Обновляем таблицу
                ShowToast($"Импорт завершен! Добавлено: {successCount}. Ошибок: {errorCount}");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void GenerateWriteOffAct()
        {
            if (SelectedEquipment == null)
            {
                System.Windows.MessageBox.Show("Выберите оборудование для списания!");
                return;
            }

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF Document (*.pdf)|*.pdf",
                FileName = $"Акт_списания_{SelectedEquipment.Name}_{DateTime.Now:yyyyMMdd}.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    PdfService.GenerateWriteOffAct(SelectedEquipment, dialog.FileName);
                    ShowToast("Акт списания успешно сгенерирован!!!");

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка генерации PDF: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private async Task AddMaintenanceAsync()
        {
            if (SelectedEquipment == null) return;

            var newTo = new Maintenance
            {
                EquipmentId = SelectedEquipment.Id,
                DateTo = NewMaintenanceDate,
                DateNext = NewMaintenanceDate.AddMonths(6), // Например, следующее через 6 мес
                Type = NewMaintenanceType,
                Comment = NewMaintenanceComment
            };

            _db.Maintenances.Add(newTo);
            await _db.SaveChangesAsync();

            Maintenances.Insert(0, newTo); // Добавляем в начало списка
            NewMaintenanceComment = string.Empty;
            ShowToast("Запись о ТО добавлена!!!");
        }
    }
}
