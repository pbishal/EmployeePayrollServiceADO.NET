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
            //  repository.EnsureDataBaseConnection();
            repository.GetAllEmployeeData();
            //repository.AddEmployee();

            Inputdata();


            Console.ReadLine();
        }
        public static void Inputdata()
        {
            EmployeeRepository repository = new EmployeeRepository();
            EmployeeModel model = new EmployeeModel();

            model.Name = "Bishal";
            model.Basic_Pay = 80000;
            model.startDate = DateTime.Now;
            model.Gender = 'M';
            model.phone_number = 9439433808;
            model.address = "Bhubaneswar";
            model.department = "Hr";
            model.Taxable_pay = 900;
            model.Deduction = 6000;
            model.NetPay = 75000;
            model.Incometax = 1000;

            repository.AddEmployee(model);

            //Console.WriteLine(repository.UpdateSalaryIntoDatabase("Bishal", 70000) ? "Update done successfully " : "Update Failed");
            //repository.GetEmployeesFromForDateRange("2018 - 05 - 03");

            repository.FindGroupedByGenderData("M");
            Console.ReadKey();
        }
    }
}

