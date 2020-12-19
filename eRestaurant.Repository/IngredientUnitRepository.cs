﻿using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using RocketPOS.Models;
using RocketPOS.Interface;
using RocketPOS.Interface.Repository;
using RocketPOS.Framework;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RocketPOS.Repository
{
    public class IngredientUnitRepository : IIngredientUnitRepository// IDataRepository<IngredientUnitModel>
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public IngredientUnitRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public IngredientUnitModel GetById(object id)
        {
            IngredientUnitModel ingredientUnitModel = new IngredientUnitModel();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {

                var query = "SELECT Id,IngredientUnitName,Notes,IsActive FROM IngredientUnit WHERE IsDeleted = 0 AND Id = @Id;";

                ingredientUnitModel = con.Query<IngredientUnitModel>(query).ToList().FirstOrDefault();
            }

            return ingredientUnitModel;
        }

        public List<IngredientUnitModel> GetIngredientUnitList()
        {
            List<IngredientUnitModel> ingredientUnitModel = new List<IngredientUnitModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                ingredientUnitModel = con.Query<IngredientUnitModel>(DataQuery.IngredientUnitAll.ToString()).ToList();
            }

            return ingredientUnitModel;
        }

        public int InsertIngredientUnit(IngredientUnitModel ingredientUnitModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO IngredientUnit (IngredientUnitName," +
                            "Notes, " +
                            "IsActive)" +
                            "VALUES (@IngredientUnitName," +
                            "@Notes," +
                            "@IsActive); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, ingredientUnitModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public int UpdateIngredientUnit(IngredientUnitModel ingredientUnitModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE IngredientUnit SET IngredientUnitName =@IngredientUnitName," +
                            "Notes = @Notes, " +
                            "IsActive = @IsActive " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, ingredientUnitModel, sqltrans, 0, System.Data.CommandType.Text);

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

        public int DeleteIngredientUnit(int ingredientUnitId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE IngredientUnit SET isDeleted= 1 WHERE Id = {ingredientUnitId}";
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
