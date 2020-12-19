using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IEmployeeService
    {
        EmployeeModel GetEmployeeById(int employeeId);
        List<EmployeeModel> GetEmployeeList();

        int InsertEmployee(EmployeeModel employeeModel);

        int UpdateEmployee(EmployeeModel employeeModel);

        int DeleteEmployee(int employeeID);
    }
}
