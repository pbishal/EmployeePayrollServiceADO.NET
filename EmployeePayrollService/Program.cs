using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Employee Payroll Services Using ADO.NET");
            //Creating a instance object of EmployeeRepository class.
            EmployeeRepository repository = new EmployeeRepository();
            // UC1 Ensuring the database connection using the sql connection string
            repository.EnsureDataBaseConnection();
            Console.ReadLine();
        }
    }
}
