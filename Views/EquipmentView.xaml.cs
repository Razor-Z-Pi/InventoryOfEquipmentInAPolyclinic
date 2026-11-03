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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using InventoryOfEquipmentInAPolyclinic.ViewModels;

namespace InventoryOfEquipmentInAPolyclinic.Views
{
    /// <summary>
    /// Логика взаимодействия для EquipmentView.xaml
    /// </summary>
    public partial class EquipmentView : UserControl
    {
        public EquipmentView()
        {
            InitializeComponent();
        }

        private async void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                await vm.LoadDataAsync();
            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                // Небольшая задержка, чтобы DataGrid успел обновить Binding
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    vm.SaveChangesCommand.Execute(null);
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}
