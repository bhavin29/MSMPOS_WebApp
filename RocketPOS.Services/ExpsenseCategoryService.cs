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
    public class ExpsenseCategoryService : IExpsenseCategoryService
    {
        private readonly IExpsenseCategoryRepository _iExpenseCategoryReportsitory;

        public ExpsenseCategoryService(IExpsenseCategoryRepository iexpsenseCategoryRepository)
        {
            _iExpenseCategoryReportsitory = iexpsenseCategoryRepository;
        }
        public int DeleteExpsenseCategory(int expenseCategoryId)
        {
           return _iExpenseCategoryReportsitory.DeleteExpsenseCategory(expenseCategoryId);
        }

        public ExpsenseCategoryModel GetExpsenseCategoryById(int expenseCategoryId)
        {
           return _iExpenseCategoryReportsitory.GetExpsenseCategoryList().Where(x => x.Id == expenseCategoryId).FirstOrDefault();
        }

        public List<ExpsenseCategoryModel> GetExpsenseCategoryList()
        {
            return _iExpenseCategoryReportsitory.GetExpsenseCategoryList();
        }

        public int InsertExpsenseCategory(ExpsenseCategoryModel expenseCategoryModel)
        {
            return _iExpenseCategoryReportsitory.InsertExpsenseCategory(expenseCategoryModel);
        }

        public int UpdateExpsenseCategory(ExpsenseCategoryModel expenseCategoryModel)
        {
            return _iExpenseCategoryReportsitory.UpdateExpsenseCategory(expenseCategoryModel);
        }
    }
}
