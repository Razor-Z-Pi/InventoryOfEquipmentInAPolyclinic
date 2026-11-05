using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InventoryOfEquipmentInAPolyclinic.Services;

namespace InventoryOfEquipmentInAPolyclinic
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            ConnectionStringTextBox.Text = ConfigService.GetConnectionString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigService.UpdateConnectionString(ConnectionStringTextBox.Text);
                MessageBox.Show("Настройки сохранены!!! Приложение будет перезапущено:)");

                // Перезапуск приложения
                System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}
