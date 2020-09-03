using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Zadatak_1.ViewModel;

namespace Zadatak_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mwm = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = mwm;
        }

        //Method responsible for validation of confirmation for delete operation. Executes on button click. 
        private void HyperlinkButton_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += mwm.DeleteRow;
                worker.RunWorkerAsync();
            }
        }
        /// <summary>
        /// Method responsible for canceling deletation of employee on cancel button click.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow AddCardPage = new AddEmployeeWindow();
            AddCardPage.Show();
            this.Close();
        }
    }
}
