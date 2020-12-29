using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Interface.Repository;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Repository
{
    public class EmployeeAttendanceRepository : IEmployeeAttendanceRpository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public EmployeeAttendanceRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<EmployeeAttendanceModel> GetEmployeeAttendanceList()
        {
            List<EmployeeAttendanceModel> addonsModel = new List<EmployeeAttendanceModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT EA.Id,(E.LastName + ' ' + E.FirstName) as EmployeeName, EmployeeId,LogDate,InTime,OutTime,TotalTimeCount,UpdateStatus,EA.Notes " +
                            "FROM EmployeeAttendance EA INNER JOIN Employee E ON  EA.EmployeeId = E.Id WHERE EA.IsDeleted = 0 " +
                            "ORDER BY LogDate, EmployeeId";
                addonsModel = con.Query<EmployeeAttendanceModel>(query).ToList();
            }

            return addonsModel;
        }

        public int InsertEmployeeAttendance(EmployeeAttendanceModel employeeModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                employeeModel.UpdateStatus = (Framework.AttendanceStatus?)1; //Manullly by defauly

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                var query = "INSERT INTO EmployeeAttendance " +
                            "(EmployeeId,LogDate,InTime,OutTime,TotalTimeCount,UpdateStatus,Notes) " +
                            " VALUES " +
                             "(@EmployeeId,@LogDate,@InTime,@OutTime,@TotalTimeCount,@UpdateStatus,@Notes); " +
                              " SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, employeeModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }

        public int UpdateEmployeeAttendance(EmployeeAttendanceModel employeeModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE EmployeeAttendance SET " +
                            " EmployeeId=@EmployeeId, LogDate=@LogDate, InTime=@InTime, OutTime=@OutTime, " +
                            "TotalTimeCount=@TotalTimeCount, UpdateStatus=@UpdateStatus, Notes=@Notes " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, employeeModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }

        public int DeleteEmployeeAttendance(int employeeID)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE EmployeeAttendance SET IsDeleted = 1 WHERE Id = {employeeID};";
                result = con.Execute(query, null, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
            }
            return result;
        }

        public int ValidationEmployeeAttendance(EmployeeAttendanceModel employeeAttendanceModel)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                List<EmployeeAttendanceModel> employeeAttendanceModels = new List<EmployeeAttendanceModel>();

                var query = "SELECT * FROM EMPLOYEEATTENDANCE WHERE EMPLOYEEID= " + employeeAttendanceModel.EmployeeId.ToString() +
                            "AND convert(varchar,LOGDATE,5) = '" + employeeAttendanceModel.LogDate.ToString("dd-MM-yy") + "';";
                employeeAttendanceModels = con.Query<EmployeeAttendanceModel>(query).ToList();

                if (employeeAttendanceModels.Count > 0)
                {
                    result = 0;
                }

                return result;
            }
        }
    }
}
