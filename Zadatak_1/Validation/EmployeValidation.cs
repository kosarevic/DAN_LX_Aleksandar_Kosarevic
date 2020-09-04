using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zadatak_1.Model;

namespace Zadatak_1.Validation
{
    /// <summary>
    /// Class validates Employe creation window data, and returns bool value.
    /// </summary>
    static class EmployeValidation
    {
        //Static variables made to store usefull data after validation.
        public static string dateOfBirth = "";
        public static string expirationDate = "";
        public static string registrationNumber = "";
        public static string dateOfIssue = "";

        public static bool Validate(Employee e)
        {
            bool cancel = false;
            while (true)
            {
                //name validation is realied below.
                while (true)
                {
                    if (e.FirstName.Length > 0 && e.FirstName.All(Char.IsLetter) && e.FirstName != null && e.FirstName != "")
                    {
                        break;
                    }
                    else
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("incorrect first name, try again.", "Notification");
                        cancel = true;
                        break;
                    }
                }
                if (cancel) return false;
                //last name validation is realised here.
                while (true)
                {
                    if (e.LastName.Length > 0 && e.LastName.All(Char.IsLetter) && e.LastName != null && e.LastName != "")
                    {
                        break;
                    }
                    else
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("incorrect last name, try again.", "Notification");
                        cancel = true;
                        break;
                    }
                }
                if (cancel) return false;
                //JMBG validation starts here.
                string day = "";
                string month = "";
                string year = "";

                while (true)
                {
                    DateTime correct = new DateTime();

                    if (e.JMBG.Length == 13 && e.JMBG.All(Char.IsDigit) && e.JMBG != null && e.JMBG != "")
                    {
                        //Validation for checking duplicate JMBG in database.
                        using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
                        {
                            var cmd = new SqlCommand(@"select JMBG from tblEmployee where JMBG = @JMBG", conn);
                            cmd.Parameters.AddWithValue("@JMBG", e.JMBG);
                            conn.Open();
                            SqlDataReader reader1 = cmd.ExecuteReader();
                            while (reader1.Read())
                            {
                                if (reader1[0].ToString() == e.JMBG)
                                {
                                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("JMBG already exists in database, try again.", "Notification");
                                    cancel = true;
                                    break;
                                }
                            }
                            reader1.Close();
                            conn.Close();
                        }
                        //if there is a duplicate, method stops further execution.
                        if (cancel) { break; }
                        //creating date of birth of user is realised below.
                        day = e.JMBG[0].ToString() + e.JMBG[1].ToString();
                        month = e.JMBG[2].ToString() + e.JMBG[3].ToString();
                        year = e.JMBG[4].ToString() + e.JMBG[5].ToString() + e.JMBG[6].ToString();

                        if (int.Parse(year) <= 99)
                        {
                            year = "2" + year;
                        }
                        else
                        {
                            year = "1" + year;
                        }

                        dateOfBirth = year + "-" + month + "-" + day;
                        dateOfIssue = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd"));
                        //validation if date of birth is in correct format and value.
                        if (!DateTime.TryParse(dateOfBirth, out correct))
                        {
                            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("JMBG is not correct, due to incorrect date of birth, please try again.", "Notification");
                            cancel = true;
                            break;
                        }
                        else
                        {
                            e.DateOfBirth = DateTime.Parse(dateOfBirth);
                        }
                        if (cancel) break;
                    }
                    else
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("JMBG input is not correct, try again.", "Notification");
                        cancel = true;
                        break;
                    }
                    if (cancel) { break; }

                    //Validations consernig date of birth are realised below.
                    if (int.Parse(year) > int.Parse(DateTime.Now.ToString("yyyy")))
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Date of birth suggests that you are born in the future, please try again.", "Notification");
                        cancel = true;
                        break;
                    }
                    else if (int.Parse(DateTime.Now.ToString("yyyy")) - int.Parse(year) > 100)
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Your date of birth suggests that you lived longer than 100 years, please try again.", "Notification");
                        cancel = true;
                        break;
                    }
                    else if (int.Parse(DateTime.Now.ToString("yyyy")) - int.Parse(year) < 18)
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Your date of birth suggests are under aged (less than 18 y/o), please try again.", "Notification");
                        cancel = true;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                if (cancel) { break; }

                if (e.RegistrationNumber.Length != 9 || !e.RegistrationNumber.All(char.IsDigit))
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("incorrect registration number (9 digits required), try again.", "Notification");
                    return false;
                }
                else
                {
                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
                    {
                        var cmd = new SqlCommand(@"select RegistrationNumber from tblEmployee where RegistrationNumber = @RegistrationNumber", conn);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", e.RegistrationNumber);
                        conn.Open();
                        SqlDataReader reader1 = cmd.ExecuteReader();
                        while (reader1.Read())
                        {
                            if (reader1[0].ToString() == e.RegistrationNumber)
                            {
                                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Registration number already exists in database, try again.", "Notification");
                                return false;
                            }
                        }
                        reader1.Close();
                        conn.Close();
                    }
                }

                if (e.Gender == "" || e.Gender == null)
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("incorrect gender, try again.", "Notification");
                    return false;
                }
                if (e.Location.Adress == null || e.Location.Adress == "")
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Adress is incorrect, please try again.", "Notification");
                    cancel = true;
                    break;
                }
                if (e.Sector.Title == null || e.Sector.Title == "")
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sector is incorrect, please try again.", "Notification");
                    cancel = true;
                    break;
                }
                else
                {
                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
                    {
                        bool exist = false;
                        var cmd = new SqlCommand(@"select * from tblSector where Title = @Title", conn);
                        cmd.Parameters.AddWithValue("@Title", e.Sector.Title);
                        conn.Open();
                        SqlDataReader reader1 = cmd.ExecuteReader();
                        while (reader1.Read())
                        {
                            e.Sector.Id = int.Parse(reader1[0].ToString());
                            e.Sector.Title = reader1[1].ToString();
                            exist = true;
                        }

                        if (!exist)
                        {
                            using (var conn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString()))
                            {
                                var cmd1 = new SqlCommand(@"insert into tblSector values (@Title); SELECT SCOPE_IDENTITY();", conn1);
                                cmd1.Parameters.AddWithValue("@Title", e.Sector.Title);
                                conn1.Open();
                                e.Sector.Id = int.Parse(cmd1.ExecuteScalar().ToString());
                                conn1.Close();
                            } 
                        }

                        reader1.Close();
                        conn.Close();
                    }
                }
                if (e.PhoneNumber.Length < 10 || !e.PhoneNumber.All(Char.IsDigit))
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Phone number must have at least 10 digits, please try again.", "Notification");
                    cancel = true;
                    break;
                }
                break;
            }
            if (!cancel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
