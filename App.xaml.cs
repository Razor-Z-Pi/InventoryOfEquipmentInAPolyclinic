using System.Configuration;
using System.Data;
using System.Windows;
using InventoryOfEquipmentInAPolyclinic.Models;
using InventoryOfEquipmentInAPolyclinic.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryOfEquipmentInAPolyclinic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Глобальный доступ к текущему пользователю
        public static User? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                using (var db = new AppDbContext())
                {
                    // Проверяем, есть ли неприменённые миграции
                    var pendingMigrations = db.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                        // Применяем все миграции (создаст таблицы, если их нет)
                        db.Database.Migrate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}\n\nПроверьте строку подключения в appsettings.json.",
                                "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            LoginWindow loginWindow = new LoginWindow();

            // ShowDialog блокирует выполнение, пока окно не закроется
            if (loginWindow.ShowDialog() == true)
            {
                MainWindow mainWindow = new MainWindow();

                // Меняем режим обратно: теперь приложение закроется, когда закроется MainWindow
                this.ShutdownMode = ShutdownMode.OnMainWindowClose;
                this.MainWindow = mainWindow;

                mainWindow.Show();
            }
            else
            {
                Shutdown(); // Закрываем приложение, если вход отменен
            }
        }
    }

}
