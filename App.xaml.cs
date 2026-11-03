using System.Configuration;
using System.Data;
using System.Windows;
using InventoryOfEquipmentInAPolyclinic.Models;

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
