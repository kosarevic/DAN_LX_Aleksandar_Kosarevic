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

namespace Zadatak_1.View
{
    /// <summary>
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        EditEmployeeViewModel evm = new EditEmployeeViewModel();
        public static bool JMBGchanged = false;
        public static bool RegNumChanged = false;
        public string InitialJMBG;
        public string InitialRegNum;

        public EditEmployeeWindow(Employee e)
        {
            JMBGchanged = false;
            RegNumChanged = false;
            InitializeComponent();
            evm.Employee = e;
            DataContext = evm;
            evm.RemoveSelectedEmployee(e);
            evm.Employee.FirstName = evm.Employee.FirstName;
            evm.Employee.LastName = evm.Employee.LastName;
            evm.Employee.JMBG = evm.Employee.JMBG;
            InitialJMBG = evm.Employee.JMBG;
            evm.Employee.Gender = evm.Employee.Gender;
            evm.Employee.RegistrationNumber = evm.Employee.RegistrationNumber;
            InitialRegNum = evm.Employee.RegistrationNumber;
            evm.Employee.PhoneNumber = evm.Employee.PhoneNumber;
            evm.Employee.Location = e.Location;
            evm.Employee.Sector = e.Sector;
            evm.Sector = e.Sector.Title;
        }

        private void Btn_Ok(object sender, RoutedEventArgs e)
        {
            evm.Employee.Sector.Title = evm.Sector;

            if (JMBG.Text != InitialJMBG)
                JMBGchanged = true;
            if (RegNum.Text != InitialRegNum)
                RegNumChanged = true;

            if (EmployeValidation.Validate(evm.Employee))
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += evm.EditEmpoye;
                worker.RunWorkerAsync();
                Thread.Sleep(2000);
                MainWindow window = new MainWindow();
                window.Show();
                Close();
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            MainWindow MainPage = new MainWindow();
            MainPage.Show();
            this.Close();
        }
    }
}
