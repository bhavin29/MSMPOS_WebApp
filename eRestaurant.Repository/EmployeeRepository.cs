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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public EmployeeRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<EmployeeModel> GetEmployeeList()
        {
            List<EmployeeModel> addonsModel = new List<EmployeeModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT iD,FirstName,MiddleName,LastName,Designation,Email,Phone,AlterPhone,PresentAdress,PermanentAdress,picture," +
                            "DegreeName,UniversityName,CGP,PassingYear,CompanyName,WorkingPeriod,Duties,Suoervisor,Signature,State,City,Zip," +
                            "CitizenShip,HireDate,OriginalHireDate,TerminationDate,TerminationReason,VolunteryTermination,RehireDate, " +
                            "RateType,Rate,PayFrequency,PayFrequencyTxt,HourlyRate2,HourlyRate3,DOB,Gender,Country,MaritalStatus," +
                            "EthnicGroup,SSN,WorkInState,LiveInState,HomeEmail,BusinessEmail,HomePhone,BUsinessPhone,CellPhone," +
                            "EmergConct,EmergHPhone,EmergWPhone,EmergContctRelation,AltEmContct,AltEmgHPhone,AltEmgWPhone,IsActive " +
                            "FROM Employee WHERE IsDeleted = 0 " +
                            "ORDER BY Lastname + ' ' + FirstName ";
                addonsModel = con.Query<EmployeeModel>(query).ToList();
            }

            return addonsModel;
        }

        public int InsertEmployee(EmployeeModel employeeModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                int MaxId = commonRepository.GetMaxId("Employee");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();

                //HireDate,OriginalHireDate,TerminationDate,RehireDate,DOB,
                //@HireDate,@OriginalHireDate,@TerminationDate,@RehireDate,@DOB,
                var query = "INSERT INTO Employee " +
                            "(Id,FirstName,MiddleName,LastName,Designation,Email,Phone,AlterPhone,PresentAdress,PermanentAdress,picture,DegreeName,UniversityName,CGP,PassingYear,CompanyName,WorkingPeriod,Duties,Suoervisor,Signature,State,City,Zip,CitizenShip,TerminationReason,VolunteryTermination,RateType,Rate,PayFrequency,PayFrequencyTxt,HourlyRate2,HourlyRate3,Gender,Country,MaritalStatus,EthnicGroup,SSN,WorkInState,LiveInState,HomeEmail,BusinessEmail,HomePhone,BUsinessPhone,CellPhone,EmergConct,EmergHPhone,EmergWPhone,EmergContctRelation,AltEmContct,AltEmgHPhone,AltEmgWPhone,IsActive) " +
                             " VALUES " +
                             "(" + MaxId + ", @FirstName,@MiddleName,@LastName,@Designation,@Email,@Phone,@AlterPhone,@PresentAdress,@PermanentAdress,@picture,@DegreeName,@UniversityName,@CGP,@PassingYear,@CompanyName,@WorkingPeriod,@Duties,@Suoervisor,@Signature,@State,@City,@Zip,@CitizenShip,@TerminationReason,@VolunteryTermination,@RateType,@Rate,@PayFrequency,@PayFrequencyTxt,@HourlyRate2,@HourlyRate3,@Gender,@Country,@MaritalStatus,@EthnicGroup,@SSN,@WorkInState,@LiveInState,@HomeEmail,@BusinessEmail,@HomePhone,@BUsinessPhone,@CellPhone,@EmergConct,@EmergHPhone,@EmergWPhone,@EmergContctRelation,@AltEmContct,@AltEmgHPhone,@AltEmgWPhone,@IsActive);" +
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

        public int UpdateEmployee(EmployeeModel employeeModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                //"HireDate=@HireDate," +
                //"OriginalHireDate=@OriginalHireDate," +
                //"TerminationDate=@TerminationDate," +
                //"RehireDate=@RehireDate," +
                //  "DOB=@DOB," +

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE Employee SET " +
                              "FirstName=@FirstName," +
                            "MiddleName=@MiddleName," +
                            "LastName=@LastName," +
                            "Designation=@Designation," +
                            "Email=@Email," +
                            "Phone=@Phone," +
                            "AlterPhone=@AlterPhone," +
                            "PresentAdress=@PresentAdress," +
                            "PermanentAdress=@PermanentAdress," +
                            "picture=@picture," +
                            "DegreeName=@DegreeName," +
                            "UniversityName=@UniversityName," +
                            "CGP=@CGP," +
                            "PassingYear=@PassingYear," +
                            "CompanyName=@CompanyName," +
                            "WorkingPeriod=@WorkingPeriod," +
                            "Duties=@Duties," +
                            "Suoervisor=@Suoervisor," +
                            "Signature=@Signature," +
                            "State=@State," +
                            "City=@City," +
                            "Zip=@Zip," +
                            "CitizenShip=@CitizenShip," +
                            "TerminationReason=@TerminationReason," +
                            "VolunteryTermination=@VolunteryTermination," +
                            "RateType=@RateType," +
                            "Rate=@Rate," +
                            "PayFrequency=@PayFrequency," +
                            "PayFrequencyTxt=@PayFrequencyTxt," +
                            "HourlyRate2=@HourlyRate2," +
                            "HourlyRate3=@HourlyRate3," +
                            "Gender=@Gender," +
                            " Country=@ Country," +
                            "MaritalStatus=@MaritalStatus," +
                            "EthnicGroup=@EthnicGroup," +
                            "SSN=@SSN," +
                            "WorkInState=@WorkInState," +
                            "LiveInState=@LiveInState," +
                            "HomeEmail=@HomeEmail," +
                            "BusinessEmail=@BusinessEmail," +
                            "HomePhone=@HomePhone," +
                            "BUsinessPhone=@BUsinessPhone," +
                            "CellPhone=@CellPhone," +
                            "EmergConct=@EmergConct," +
                            "EmergHPhone=@EmergHPhone," +
                            "EmergWPhone=@EmergWPhone," +
                            "EmergContctRelation=@EmergContctRelation," +
                            "AltEmContct=@AltEmContct," +
                            "AltEmgHPhone=@AltEmgHPhone," +
                            "AltEmgWPhone=@AltEmgWPhone," +
                            "IsActive=@IsActive " +
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

        public int DeleteEmployee(int employeeID)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE Employee SET IsDeleted = 1 WHERE Id = {employeeID};";
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
    }
}
