using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Zadatak_1.LogFile;
using Zadatak_1.Model;

namespace Zadatak_1.ViewModel
{
    /// <summary>
    /// Class made for displaying Main Window features of the application
    /// </summary>
    class MainWindowViewModel : INotifyPropertyChanged
    {
        //Collections made to ensure application proper functionality.
        public ObservableCollection<MainWindowModel> MainWindowViewModels { get; set; }
        public static List<Employee> employees = new List<Employee>();

        //Constructor ensures that necesery methods are invoked when class object initializes.
        public MainWindowViewModel()
        {
            FillList();
        }
        //Table row data is stored here
        private MainWindowModel row;

        public MainWindowModel Row
        {
            get { return row; }
            set
            {
                if (row != value)
                {
                    row = value;
                    OnPropertyChanged("Row");
                }
            }
        }
        /// <summary>
        /// Method for filling out previously mentioned collection
        /// </summary>
        public void FillList()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                SqlCommand query = new SqlCommand("select * from tblEmployee e " +
                            "join tblLocation l on e.LocationID = l.LocationID " +
                            "join tblSector s on e.SectorID = s.SectorID", conn);
                conn.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                if (MainWindowViewModels == null)
                    MainWindowViewModels = new ObservableCollection<MainWindowModel>();

                foreach (DataRow row in dataTable.Rows)
                {
                    MainWindowModel m = new MainWindowModel
                    {
                        Employee = new Employee
                        {
                            Id = int.Parse(row[0].ToString()),
                            FirstName = row[1].ToString(),
                            LastName = row[2].ToString(),
                            JMBG = row[3].ToString(),
                            DateOfBirth = DateTime.Parse(row[4].ToString()),
                            Gender = char.Parse(row[5].ToString()),
                            RegistrationNumber = row[6].ToString(),
                            PhoneNumber = row[7].ToString(),
                            Manager = new Employee()
                        },
                        Location = new Location
                        {
                            Id = int.Parse(row[11].ToString()),
                            Adress = row[12].ToString(),
                            Town = row[13].ToString(),
                            Country = row[14].ToString()
                        },
                        Sector = new Sector
                        {
                            Id = int.Parse(row[15].ToString()),
                            Title = row[16].ToString()
                        },
                        ManagerId = int.Parse(row[10].ToString())
                    };
                    m.Employee.Location = m.Location;
                    m.Employee.Sector = m.Sector;
                    MainWindowViewModels.Add(m);
                    employees.Add(m.Employee);
                }
            }
            //Loop added to ensure each employee gets a manager if it has one.
            foreach (var m in MainWindowViewModels)
            {
                foreach (Employee e in employees)
                {
                    if (m.ManagerId == e.Id)
                    {
                        m.Employee.Manager = e;
                    }
                }
            }
        }
        /// <summary>
        /// Method implements delete functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteRow(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
            if (row == null) { return; }
            foreach (var m in MainWindowViewModels)
            {
                if (m.Employee.Manager.Id == row.Employee.Id)
                {
                    m.Employee.Manager = null;
                }
            }
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
            con.Open();
            var cmd = new SqlCommand("delete from tblEmployee where EmployeeID = @EmployeeID;", con);
            cmd.Parameters.AddWithValue("@EmployeeID", row.Employee.Id);
            cmd.ExecuteNonQuery();
            LogActions.LogDeleteEmployee(row.Employee);
            //Object is removed from ongoing list.
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindowViewModels.Remove(row);
            });
            con.Close();
            con.Dispose();
            var messageBoxResult = System.Windows.MessageBox.Show("Delete Successfull", "Notification");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
