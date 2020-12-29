using RocketPOS.Interface.Services;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RocketPOS.Services
{
    public class EmployeeAttendanceService : IEmployeeAttendanceService
    {
        private readonly IEmployeeAttendanceRpository _iEmployeeAttendanceReportsitory;

        public EmployeeAttendanceService(IEmployeeAttendanceRpository iEmployeeRepository)
        {
            _iEmployeeAttendanceReportsitory = iEmployeeRepository;
        }
        public int InsertEmployeeAttendance(EmployeeAttendanceModel employeeAttendanceModel)
        {
            return _iEmployeeAttendanceReportsitory.InsertEmployeeAttendance(employeeAttendanceModel);
        }

        public int UpdateEmployeeAttendance(EmployeeAttendanceModel employeeAttendanceModel)
        {
            return _iEmployeeAttendanceReportsitory.UpdateEmployeeAttendance(employeeAttendanceModel);
        }

        public int DeleteEmployeeAttendance(int employeeAttendanceID)
        {
            return _iEmployeeAttendanceReportsitory.DeleteEmployeeAttendance(employeeAttendanceID);
        }

        public EmployeeAttendanceModel GetEmployeeAttendaceById(int employeeAttendaceId)
        {
            return _iEmployeeAttendanceReportsitory.GetEmployeeAttendanceList().Where(x => x.Id == employeeAttendaceId).FirstOrDefault();
        }

        public List<EmployeeAttendanceModel> GetEmployeeAttendaceList()
        {
            return _iEmployeeAttendanceReportsitory.GetEmployeeAttendanceList();
        }

        public int ValidationEmployeeAttendance(EmployeeAttendanceModel employeeAttendanceModel)
        {
            return _iEmployeeAttendanceReportsitory.ValidationEmployeeAttendance(employeeAttendanceModel);
        }
    }
}
