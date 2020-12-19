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
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _IEmployeeReportsitory;

        public EmployeeService(IEmployeeRepository iAddondRepository)
        {
            _IEmployeeReportsitory = iAddondRepository;
        }
        
        public List<EmployeeModel> GetEmployeeList()
        {

            return _IEmployeeReportsitory.GetEmployeeList();
        }

        public int InsertEmployee(EmployeeModel EmployeeModel)
        {
            return _IEmployeeReportsitory.InsertEmployee(EmployeeModel);
        }

        public int UpdateEmployee(EmployeeModel EmployeeModel)
        {
            return _IEmployeeReportsitory.UpdateEmployee(EmployeeModel);
        }

        public int DeleteEmployee(int EmployeeID)
        {
            return _IEmployeeReportsitory.DeleteEmployee(EmployeeID);
        }

        public EmployeeModel GetEmployeeById(int employeeId)
        {
            return _IEmployeeReportsitory.GetEmployeeList().Where(x => x.Id == employeeId).FirstOrDefault();
        }
    }
}
