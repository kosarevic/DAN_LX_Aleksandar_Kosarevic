using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zadatak_1.Model;
using Zadatak_1.Validation;
using Zadatak_1.ViewModel;

namespace Zadatak_1
{
    /// <summary>
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        AddEmployeeViewModel evm = new AddEmployeeViewModel();

        public AddEmployeeWindow()
        {
            InitializeComponent();
            DataContext = evm;
            evm.Employee.FirstName = "";
            evm.Employee.LastName = "";
            evm.Employee.JMBG = "";
            evm.Employee.Gender = "";
            evm.Employee.RegistrationNumber = "";
            evm.Employee.PhoneNumber = "";
            evm.Employee.Location = new Location();
            evm.Employee.Sector = new Sector();
        }

        private void Btn_Ok(object sender, RoutedEventArgs e)
        {
            evm.Employee.Sector.Title = evm.Sector;
            if (EmployeValidation.Validate(evm.Employee))
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += evm.AddEmployee;
                worker.RunWorkerAsync();
            }
            Thread.Sleep(2000);
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            MainWindow MainPage = new MainWindow();
            MainPage.Show();
            this.Close();
        }
    }
}
