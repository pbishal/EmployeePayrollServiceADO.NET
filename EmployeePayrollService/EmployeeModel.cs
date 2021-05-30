using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollService
{
    class EmployeeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Basic_Pay { get; set; }
        public DateTime startDate { get; set; }
        public char Gender { get; set; }
        public long phone_number { get; set; }
        public string address { get; set; }
        public string department { get; set; }
        public double Taxable_pay { get; set; }
        public double Deduction { get; set; }
        public double NetPay { get; set; }
        public double Incometax { get; set; }
        
    }
}
