﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using Microsoft.Extensions.Options;
using RocketPOS.Interface.Repository;
using RocketPOS.Models;

namespace RocketPOS.Repository
{
    public class SchedulerRepository : ISchedulerRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;
        public SchedulerRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public int SalesPOSInventorySync()
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                result = con.Query("InventoryPushPOSScheduler ", commandType: System.Data.CommandType.StoredProcedure).ToString();
            }
            return Convert.ToInt32(result);
        }
    }
}
