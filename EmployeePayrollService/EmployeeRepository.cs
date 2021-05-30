using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace EmployeePayrollService
{
    class EmployeeRepository
    {
        ///Specifying the connection string from the sql server connection.
        public static string conectionString = @"Data Source=BISHAL\SQLBISHAL;Initial Catalog=payroll_service;Persist Security Info=True;User ID=sa;Password=9439433808";
        /// Establishing the connection using the Sql
        SqlConnection connection = new SqlConnection(conectionString);

        /// <summary>
        ///UC1 Creating a method for checking for the validity of the connection.
        /// </summary>
        public void EnsureDataBaseConnection()
        {
            this.connection.Open();
            using (connection)
            {
                Console.WriteLine("The Connection is created");
            }
            this.connection.Close();
        }

    }
}
