using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zadatak_1.Model;

namespace Zadatak_1.ViewModel
{
    class EditEmployeeViewModel : INotifyPropertyChanged
    {
        //Class specific collection is determined below.
        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public EditEmployeeViewModel()
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

        public void EditEmpoye(object sender, DoWorkEventArgs e)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
            {
                var cmd = new SqlCommand(@"update tblEmployee set FirstName=@FirstName, LastName=@LastName, JMBG=@JMBG, DateOfBirth=@DateOfBirth, Gender=@Gender, RegistrationNumber=@RegistrationNumber, PhoneNumber=@PhoneNumber, LocationID=@LocationID, SectorID=@SectorID, ManagerID=@ManagerID where EmployeeID=@EmployeID", conn);
                cmd.Parameters.AddWithValue("@EmployeID", employee.Id);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@JMBG", employee.JMBG);
                cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@RegistrationNumber", employee.RegistrationNumber);
                cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber); 
                cmd.Parameters.AddWithValue("@LocationID", employee.Location.Id);
                cmd.Parameters.AddWithValue("@SectorID", employee.Sector.Id);
                cmd.Parameters.AddWithValue("@ManagerID", employee.Manager.Id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Employe successfully updated.", "Notification");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
