using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.Input;
using InventoryOfEquipmentInAPolyclinic.ViewModels;

namespace InventoryOfEquipmentInAPolyclinic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Обработка Ctrl+N
            this.InputBindings.Add(new KeyBinding(
                new RelayCommand(() =>
                {
                    // Находим EquipmentView и фокусируем TextBox
                    // Это упрощенный вариант, лучше использовать Attached Properties
                }),
                Key.N, ModifierKeys.Control));
        }

        private async void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            // Получаем ViewModel и вызываем загрузку
            if (DataContext is MainViewModel vm)
            {
                await vm.LoadDataAsync();
            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                // Небольшая задержка, чтобы DataGrid успел применить значение
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    vm.SaveChangesCommand.Execute(null);
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            this.Close(); // Закроется главное окно, приложение завершится или покажет Login
        }
    }
}