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
    /// Class made for displaying Add Employee Window features of the application
    /// </summary>
    class AddEmployeeViewModel : INotifyPropertyChanged
    {
        //Class specific collection is determined below.
        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public AddEmployeeViewModel()
        {
            FillList();
            Employee = new Employee();
        }

        private Employee employee;

        public Employee Employee
        {
            get { return employee; }
            set
            {
                if (employee != value)
                {
                    employee = value;
                    OnPropertyChanged("employee");
                }
            }
        }

        private string sector;

        public string Sector
        {
            get { return sector; }
            set
            {
                if (sector != value)
                {
                    sector = value;
                    OnPropertyChanged("Sector");
                }
            }
        }

        //Gender collection is made with predifined values.
        private List<string> genders;

        public List<string> Genders
        {
            get { return new List<string> { "M", "F", "X" }; }
            set { genders = value; }
        }

        /// <summary>
        /// Method for filling out previously mentioned collection
        /// </summary>
        public void FillList()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                SqlCommand query = new SqlCommand("select * from tblLocation", conn);
                conn.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                if (Locations == null)
                    Locations = new ObservableCollection<Location>();

                foreach (DataRow row in dataTable.Rows)
                {
                    Location l = new Location
                    {
                        Id = int.Parse(row[0].ToString()),
                        Adress = row[1].ToString(),
                        Town = row[2].ToString(),
                        Country = row[3].ToString()
                    };
                    Locations.Add(l);
                }
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                SqlCommand query = new SqlCommand("select * from tblEmployee e " +
                            "join tblLocation l on e.LocationID = l.LocationID " +
                            "join tblSector s on e.SectorID = s.SectorID", conn);
                conn.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                if (Employees == null)
                    Employees = new ObservableCollection<Employee>();

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
                            Gender = row[5].ToString(),
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
                    Employees.Add(m.Employee);
                }
            }
        }
        /// <summary>
        /// Method enables adding employee to the database.
        /// </summary>
        public void AddEmployee(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
                {
                    var cmd = new SqlCommand(@"insert into tblEmployee values (@FirstName, @LastName, @JMBG, @DateOfBirth, @Gender, @RegNum, @PhoneNumber, @LocId, @SectorID, @ManagerID);", conn);
                    cmd.Parameters.AddWithValue("@FirstName", Employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", Employee.LastName);
                    cmd.Parameters.AddWithValue("@JMBG", Employee.JMBG);
                    cmd.Parameters.AddWithValue("@DateOfBirth", Employee.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", Employee.Gender);
                    cmd.Parameters.AddWithValue("@RegNum", Employee.RegistrationNumber);
                    cmd.Parameters.AddWithValue("@PhoneNumber", Employee.PhoneNumber);
                    cmd.Parameters.AddWithValue("@LocId", Employee.Location.Id);
                    cmd.Parameters.AddWithValue("@SectorID", Employee.Sector.Id);
                    cmd.Parameters.AddWithValue("@ManagerID", 1);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Employee successfully created.", "Notification");
                LogActions.LogAddEmployee(Employee);
            }
            catch (Exception)
            {
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
