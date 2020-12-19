using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;


namespace RocketPOS.Interface.Repository
{
    public interface IEmployeeRepository
    {
        List<EmployeeModel> GetEmployeeList();

        int InsertEmployee(EmployeeModel employeeModel);

        int UpdateEmployee(EmployeeModel employeeModel);

        int DeleteEmployee(int EmployeeID);

    }
}
