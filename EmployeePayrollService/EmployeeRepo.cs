using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EmployeePayrollService
{
    class EmployeeRepo
    {
        public static string conectionString = @"Data Source=BISHAL\SQLBISHAL;Initial Catalog=payroll_service;Persist Security Info=True;User ID=sa;Password=***********";
        SqlConnection connection = new SqlConnection(conectionString);

    }
}
