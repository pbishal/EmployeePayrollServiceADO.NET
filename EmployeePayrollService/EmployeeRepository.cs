using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollService
{
    public class EmployeeRepository
    {
        ///Specifying the connection string from the sql server connection.
        public static string conectionString = @"Data Source=BISHAL\SQLBISHAL;Initial Catalog=payroll_service;Persist Security Info=True;User ID=sa;Password=9439433808";
        /// Establishing the connection using the Sql
        SqlConnection connection = new SqlConnection(conectionString);

        ///UC1 Creating a method for checking for the validity of the connection.

        public void EnsureDataBaseConnection()
        {
            /// Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DatabaseConnection dbc = new DatabaseConnection();
            connection = dbc.GetConnection();
            using (connection)
            {
                Console.WriteLine("The Connection is created");
            }
            connection.Close();
        }

        /// UC2 Ability for Employee Payroll Service to retrieve the Employee Payroll from the Database

        public void GetAllEmployeeData()
        {
            //Creating Employee model class object
            EmployeeModel employee = new EmployeeModel();
            try
            {
                using (connection)
                {
                    //Query to get all the data from table.
                    string query = @"select * from dbo.employee_payroll";
                    //Opening the connection to the statrt mapping.
                    this.connection.Open();
                    //Implementing the command on the connection fetched database table.
                    SqlCommand command = new SqlCommand(query, connection);
                    //Executing the Sql datareaeder to fetch the all records.
                    SqlDataReader dataReader = command.ExecuteReader();
                    //Checking datareader has rows or not.
                    if (dataReader.HasRows)
                    {
                        //using while loop for read multiple rows.
                        // Mapping the data to the employee model class object.
                        while (dataReader.Read())
                        {
                            employee.Id = dataReader.GetInt32(0);
                            employee.Name = dataReader.GetString(1);
                            employee.Basic_Pay = dataReader.GetDouble(2);
                            employee.startDate = dataReader.GetDateTime(3);
                            employee.Gender = dataReader.GetChar(4);
                            employee.phone_number = dataReader.GetInt64(5);
                            employee.address = dataReader.GetString(6);
                            employee.department = dataReader.GetString(7);
                            employee.Taxable_pay = dataReader.GetDouble(8);
                            employee.Deduction = dataReader.GetDouble(9);
                            employee.NetPay = dataReader.GetDouble(10);
                            employee.Incometax = dataReader.GetDouble(11);
                            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", employee.Id, employee.Name,
                                employee.Basic_Pay, employee.startDate, employee.Gender, employee.phone_number, employee.address, employee.department,
                                employee.Taxable_pay, employee.Deduction, employee.NetPay, employee.Incometax);
                            Console.WriteLine("\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("no data found ");
                    }
                    dataReader.Close();
                }
            }
            /// Catching the null record exception
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //Always ensuring the closing of the connection
            finally
            {
                this.connection.Close();
            }

        }

        /// Adding Employee To Database

        public void AddEmployee(EmployeeModel model)
        {
            try
            {
                using (this.connection)
                {
                    //Creating a stored Procedure for adding employees into database
                    SqlCommand command = new SqlCommand("dbo.employee_payroll", this.connection);
                    //Command type is set as stored procedure
                    command.CommandType = CommandType.StoredProcedure;
                    //Adding values from employeemodel to stored procedure using disconnected architecture
                    //connected architecture will only read the data
                    command.Parameters.AddWithValue("@EmpName", model.Name);
                    command.Parameters.AddWithValue("@basic_Pay", model.Basic_Pay);
                    command.Parameters.AddWithValue("@StartDate", model.startDate);
                    command.Parameters.AddWithValue("@gender", model.Gender);
                    command.Parameters.AddWithValue("@phoneNumber", model.phone_number);
                    command.Parameters.AddWithValue("@Address", model.address);
                    command.Parameters.AddWithValue("@department", model.department);
                    command.Parameters.AddWithValue("@Taxablepay", model.Taxable_pay);
                    command.Parameters.AddWithValue("@Deduction", model.Deduction);
                    command.Parameters.AddWithValue("@Netpay", model.NetPay);
                    command.Parameters.AddWithValue("@Incometax", model.Incometax);
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    connection.Close();


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }



        }
        /// UC3 Updates the given empname with given salary into database.
        /// </summary>
        /// <param name="empName"></param>
        /// <param name="basicPay"></param>
        /// <returns></returns>
        public bool UpdateSalaryIntoDatabase(string empName, double basicPay)
        {
            DatabaseConnection dbc = new DatabaseConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = @"update dbo.employee_payroll set basic_pay=@p1 where Name=@p2";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", basicPay);
                    command.Parameters.AddWithValue("@p2", empName);
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }
        /// UC 4  Reads the updated salary from database.

        public double UpdatedSalaryFromDatabase(string empName)
        {
            DatabaseConnection dbc = new DatabaseConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    string query = @"select basic_pay from dbo.employee_payroll where EmpName=@p1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@p1", empName);
                    return (double)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /// UC5 Gets the employees details for a particular date range.

        public void GetEmployeesFromForDateRange(string date)
        {
            string query = $@"select * from dbo.employee_payroll where StartDate between cast('{date}' as date) and cast(getdate() as date)";
            GetAllEmployeeData(query);
        }
        /// UC6 Getting the detail of salary ofthe employee joining grouped by gender and searched for a particular gender.

        public void FindGroupedByGenderData(string gender)
        {
            DatabaseConnection dbc = new DatabaseConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    string query = @"select Gender,count(basic_pay) as EmpCount,min(basic_pay) as MinSalary,max(basic_pay) 
                                   as MaxSalary,sum(basic_pay) as SalarySum,avg(basic_pay) as AvgSalary from dbo.employee_payroll
                                   where Gender=@parameter group by Gender";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@parameter", gender);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int empCount = reader.GetInt32(1);
                            double minSalary = reader.GetDouble(2);
                            double maxSalary = reader.GetDouble(3);
                            double sumOfSalary = reader.GetDouble(4);
                            double avgSalary = reader.GetDouble(5);
                            Console.WriteLine($"Gender:{gender}\nEmployee Count:{empCount}\nMinimum Salary:{minSalary}\nMaximum Salary:{maxSalary}\n" +
                                $"Total Salary for {gender} :{sumOfSalary}\n" + $"Average Salary:{avgSalary}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data found");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /// UC7 Inserts data into multiple tables using transactions.
        /// </summary>
        public void InsertIntoMultipleTablesWithTransactions()
        {
            DatabaseConnection dbc = new DatabaseConnection();
            connection = dbc.GetConnection();

            Console.WriteLine("Enter EmployeeID");
            int empID = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Name:");
            string empName = Console.ReadLine();

            DateTime startDate = DateTime.Now;

            Console.WriteLine("Enter Address:");
            string address = Console.ReadLine();

            Console.WriteLine("Enter Gender:");
            string gender = Console.ReadLine();

            Console.WriteLine("Enter PhoneNumber:");
            double phonenumber = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter BasicPay:");
            int basicPay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Deductions:");
            int deductions = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter TaxablePay:");
            int taxablePay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Tax:");
            int tax = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter NetPay:");
            int netPay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter CompanyId:");
            int companyId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter CompanyName:");
            string companyName = Console.ReadLine();

            Console.WriteLine("Enter DeptId:");
            int deptId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter DeptName:");
            string deptName = Console.ReadLine();

            using (connection)
            {
                connection.Open();

                // Start a local transaction.
                SqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist a command in the current transaction.
                SqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    // Execute 1st command
                    command.CommandText = "insert into company values(@company_id,@company_name)";
                    command.Parameters.AddWithValue("@company_id", companyId);
                    command.Parameters.AddWithValue("@company_name", companyName);
                    command.ExecuteScalar();

                    // Execute 2nd command
                    command.CommandText = "insert into employee values(@emp_id,@EmpName,@gender,@phone_number,@address,@startDate,@company_id)";
                    command.Parameters.AddWithValue("@emp_id", empID);
                    command.Parameters.AddWithValue("@EmpName", empName);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@phone_number", phonenumber);
                    command.Parameters.AddWithValue("@address", address);
                    command.ExecuteScalar();

                    // Execute 3rd command
                    command.CommandText = "insert into payroll values(@emp_id,@Basic_Pay,@Deductions,@Taxable_pay,@Income_tax,@Net_pay)";
                    command.Parameters.AddWithValue("@Basic_Pay", basicPay);
                    command.Parameters.AddWithValue("@Deductions", deductions);
                    command.Parameters.AddWithValue("@Taxable_pay", taxablePay);
                    command.Parameters.AddWithValue("@Income_tax", tax);
                    command.Parameters.AddWithValue("@Net_pay", netPay);
                    command.ExecuteScalar();

                    // Execute 4th command
                    command.CommandText = "insert into department values(@dept_id,@dept_name)";
                    command.Parameters.AddWithValue("@dept_id", deptId);
                    command.Parameters.AddWithValue("@dept_name", deptName);
                    command.ExecuteScalar();

                    // Execute 5th command
                    command.CommandText = "insert into employee_dept values(@emp_id,@dept_id)";
                    command.ExecuteNonQuery();

                    // Commit the transaction after all commands.
                    sqlTran.Commit();
                    Console.WriteLine("All records were added into the database.");
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Console.WriteLine(ex.Message);
                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection
                        // is closed or the transaction has already been rolled
                        // back on the server.
                        Console.WriteLine(exRollback.Message);
                    }
                }
            }
        }

        /// UC 8 : Retrieves the employee details from multiple tables after implementing E-R concept.
        public void RetrieveEmployeeDetailsFromMultipleTables()

        {
            DatabaseConnection dbc = new DatabaseConnection();
            connection = dbc.GetConnection();
            EmployeeModel employee = new EmployeeModel();
            string query = @"select emp.EmpId, emp.EmpName, dept.basic_pay, emp.StartDate, emp.phoneNumber, emp.address, 
                                    dept.department, emp.gender, pay.deductions, pay.taxable_pay, pay.income_tax, pay.net_pay
                                    from employee_payroll emp, employee_department dept, payroll pay
                                    where emp.EmpId = dept.EmpId and dept.basic_pay = pay.basic_pay;";
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employee.Id = reader.GetInt32(0);
                            employee.Name = reader.GetString(1);
                            employee.Basic_Pay = reader.GetDouble(2);
                            employee.startDate = reader.GetDateTime(3);
                            employee.Gender = reader.GetChar(4);
                            employee.phone_number = reader.GetInt64(5);
                            employee.address = reader.GetString(6);
                            employee.department = reader.GetString(7);
                            employee.Taxable_pay = reader.GetDouble(8);
                            employee.Deduction = reader.GetDouble(9);
                            employee.NetPay = reader.GetDouble(10);
                            employee.Incometax = reader.GetDouble(11);
                            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", employee.Id, employee.Name,
                                employee.Basic_Pay, employee.startDate, employee.Gender, employee.phone_number, employee.address, employee.department,
                                employee.Taxable_pay, employee.Deduction, employee.NetPay, employee.Incometax);
                            Console.WriteLine("\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State.Equals("Open"))
                    connection.Close();
            }
        }
    }
}