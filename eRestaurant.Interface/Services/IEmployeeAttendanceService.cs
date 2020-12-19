using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IEmployeeAttendanceService
    {
        EmployeeAttendanceModel GetEmployeeAttendaceById(int employeeAttendaceId);
        List<EmployeeAttendanceModel> GetEmployeeAttendaceList();

        int InsertEmployeeAttendance(EmployeeAttendanceModel employeeAttendaceModel);

        int UpdateEmployeeAttendance(EmployeeAttendanceModel employeeAttendaceModel);

        int DeleteEmployeeAttendance(int employeeAttendaceID);
    }
}
