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

        //SqlConnection connection1 = new SqlConnection(connectionString);
        //ReInitiallizing the connection using the sql for update employee method.
        //SqlConnection connection2 = new SqlConnection(connectionString);



        ///UC1 Creating a method for checking for the validity of the connection.

        public void EnsureDataBaseConnection()
        {
            this.connection.Open();
            using (connection)
            {
                Console.WriteLine("The Connection is created");
            }
            this.connection.Close();
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
                    SqlCommand command = new SqlCommand("dbo.Employee_Daata", this.connection);
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

    }
}