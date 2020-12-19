using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;

namespace RocketPOS.Interface.Repository
{
    public interface IExpsenseCategoryRepository
    {
        List<ExpsenseCategoryModel> GetExpsenseCategoryList();

        int InsertExpsenseCategory(ExpsenseCategoryModel expenseCategoryModel);

        int UpdateExpsenseCategory(ExpsenseCategoryModel expenseCategoryModel);

        int DeleteExpsenseCategory(int expenseCategoryId);
    }
}
