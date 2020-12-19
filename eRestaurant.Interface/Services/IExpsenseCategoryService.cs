using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Services
{
    public interface IExpsenseCategoryService
    {
        ExpsenseCategoryModel GetExpsenseCategoryById(int expenseCategoryId);
        List<ExpsenseCategoryModel> GetExpsenseCategoryList();

        int InsertExpsenseCategory(ExpsenseCategoryModel expenseCategoryModel);

        int UpdateExpsenseCategory(ExpsenseCategoryModel expenseCategoryModel);

        int DeleteExpsenseCategory(int expenseCategoryId);
    }
}
