using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IEmployeeAttendanceRpository
    {
        List<EmployeeAttendanceModel> GetEmployeeAttendanceList();

        int InsertEmployeeAttendance(EmployeeAttendanceModel employeeAttendaceModel);

        int UpdateEmployeeAttendance(EmployeeAttendanceModel employeeAttendaceModel);

        int DeleteEmployeeAttendance(int employeeAttendaceID);

        int ValidationEmployeeAttendance(EmployeeAttendanceModel employeeAttendanceModel);
    }
}
