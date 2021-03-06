using Dapper;
using Microsoft.Extensions.Options;
using RocketPOS.Framework;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RocketPOS.Repository
{
    public class RewardSetupRepository : IRewardSetupRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public RewardSetupRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int DeleteRewardSetup(int id)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = $"UPDATE RewardSetup SET isDeleted= 1,DateDeleted=GetUtcDate(),UserIdDeleted= " + LoginInfo.Userid + " WHERE Id = " + id;
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

        public List<RewardSetupModel> GetRewardSetupList()
        {
            List<RewardSetupModel> rewardSetupModel = new List<RewardSetupModel>();

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var query = "SELECT Id,OfferName,TransactionAmount,RewardPoint,Notes,IsActive  FROM RewardSetup WHERE IsDeleted = 0 " +
               "ORDER BY OfferName ";

                rewardSetupModel = con.Query<RewardSetupModel>(query).ToList();

            }

            return rewardSetupModel;
        }

        public int InsertRewardSetup(RewardSetupModel rewardSetupModel)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("RewardSetup", "OfferName", rewardSetupModel.OfferName, rewardSetupModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                int MaxId = commonRepository.GetMaxId("RewardSetup");

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "INSERT INTO RewardSetup (Id,OfferName,TransactionAmount,RewardPoint," +
                            "Notes, " +
                            "IsActive,UserIdInserted,DateInserted,IsDeleted)" +
                            "VALUES (" + MaxId + ",@OfferName,@TransactionAmount,@RewardPoint," +
                            "@Notes," +
                            "@IsActive," + LoginInfo.Userid + ",GetUtcDate(),0); SELECT CAST(SCOPE_IDENTITY() as INT);";
                result = con.Execute(query, rewardSetupModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("RewardSetup");
                }
                else
                {
                    sqltrans.Rollback();
                }
                return result;
            }
        }

        public int UpdateRewardSetup(RewardSetupModel rewardSetupModel)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                result = commonRepository.GetValidateUnique("RewardSetup", "OfferName", rewardSetupModel.OfferName, rewardSetupModel.Id.ToString());
                if (result > 0)
                {
                    return -1;
                }

                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                var query = "UPDATE RewardSetup SET OfferName =@OfferName," +
                            "TransactionAmount = @TransactionAmount, " +
                            "RewardPoint = @RewardPoint, " +
                            "Notes = @Notes, " +
                            "IsActive = @IsActive, " +
                            "UserIdUpdated =  " + LoginInfo.Userid +
                            ",DateUpdated = GetUtcDate() " +
                            "WHERE Id = @Id;";
                result = con.Execute(query, rewardSetupModel, sqltrans, 0, System.Data.CommandType.Text);

                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("RewardSetup");
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
