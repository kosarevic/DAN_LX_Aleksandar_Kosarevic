using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak_1.Model;

namespace Zadatak_1.LogFile
{
    /// <summary>
    /// Class made to preform loging of activities into text file LogFile.txt
    /// </summary>
    static class LogActions
    {
        //method logs deletation of the Employee.
        public static void LogDeleteEmployee(Employee e)
        {
            File.AppendAllText(@"..\\..\Files\LogFile.txt", "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + "]" + " Employee: " + e.FirstName + " " + e.LastName + ", has been deleted" + Environment.NewLine);
        }
        //method logs creation of the Employee.
        public static void LogAddEmployee(Employee e)
        {
            File.AppendAllText(@"..\\..\Files\LogFile.txt", "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + "]" + " Employee: " + e.FirstName + " " + e.LastName + ", has been created" + Environment.NewLine);
        }
    }
}
