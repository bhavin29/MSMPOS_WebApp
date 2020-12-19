using System;
using System.Collections.Generic;
using System.Text;

namespace RocketPOS.Framework
{
    public static class DataQuery
    {
        public const string IngredientUnitAll = "SELECT Id,IngredientUnitName,Notes,IsActive FROM IngredientUnit WHERE IsDeleted = 0 " +
                                "ORDER BY IngredientUnitName";
        public const string IngredientUnitById = "";
        public const string IngredientUnitUpdate = "";
        public const string IngredientUnitInsert = "";
        public const string IngredientUnitDelete = "";


    }
}
