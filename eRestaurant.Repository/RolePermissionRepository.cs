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
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly IOptions<ReadConfig> _ConnectionString;

        public RolePermissionRepository(IOptions<ReadConfig> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public List<WebRolePageModel> GetWebRolePermissionList(int webRoleId)
        {
            List<WebRolePageModel> webRolePageModel = new List<WebRolePageModel>();
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                var queryWithRecords = "Select WRP.Id ,P.Id as PagesId,P.PageName,P.Module,P.LogicalPageName, WRP.WebRolesId,ISNULL(WRP.[Add],0) As [Add],ISNULL(WRP.[Edit],0) As [Edit],ISNULL(WRP.[Delete],0) As [Delete],ISNULL(WRP.[View],0) As [View] From Pages P " +
                            "Left Join  WebRolePages WRP ON  WRP.PagesId = P.Id Where WRP.WebRolesId =  " + webRoleId + " And P.IsDeleted=0 " + 
                            " union " +
                            " Select  0 AS Id ,P.Id as PagesId,P.PageName,P.Module,P.LogicalPageName,Null AS WebRolesId,0 As [Add],0 As [Edit],0 As [Delete],0 As [View] From Pages P  " +
                            " Left Join  WebRolePages WRP ON  WRP.PagesId = P.Id where P.Id Not in (Select  P.Id From Pages P Left Join  WebRolePages WRP ON  WRP.PagesId = P.Id Where WRP.WebRolesId =" + webRoleId + ") And P.IsDeleted=0 ";
                webRolePageModel = con.Query<WebRolePageModel>(queryWithRecords).ToList();

                //if (webRolePageModel.Count <= 0)
                //{
                //    var queryWithNoRecords = " Select distinct 0 AS Id ,P.Id as PagesId,P.PageName,Null AS WebRolesId,0 As [Add],0 As [Edit],0 As [Delete],0 As [View] From Pages P  " +
                //            " Left Join  WebRolePages WRP ON  WRP.PagesId = P.Id  ";
                //    webRolePageModel = con.Query<WebRolePageModel>(queryWithNoRecords).ToList();
                //}
            }
            return webRolePageModel;
        }

        public int UpdateRolePermissionList(List<WebRolePageModel> webRolePageModels)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(_ConnectionString.Value.ConnectionString))
            {
                CommonRepository commonRepository = new CommonRepository(_ConnectionString);
                con.Open();
                SqlTransaction sqltrans = con.BeginTransaction();
                foreach (var item in webRolePageModels)
                {
                    var query = string.Empty;
                    if (item.Id != 0)
                    {
                        query = $"Update WebRolePages set WebRolesId=@WebRolesId,PagesId=@PagesId,[Add]=@Add,Edit=@Edit,[Delete]=@Delete,[View]=@View,UserIdUpdated=" + LoginInfo.Userid + ", DateUpdated=GetutcDate() where id=@Id";
                    }
                    else
                    {
                        query = " INSERT INTO WebRolePages (WebRolesId,PagesId,[Add],[Edit],[Delete],[View],[UserIdInserted],[DateInserted],[IsDeleted]) " +
                            "values(@WebRolesId,@PagesId,@Add,@Edit,@Delete,@View," + LoginInfo.Userid + ",GetUtcDate(),0)";
                    }
                    result = con.Execute(query, item, sqltrans, 0, System.Data.CommandType.Text);
                }
                if (result > 0)
                {
                    sqltrans.Commit();
                    string output = commonRepository.SyncTableStatus("WebRolePages");
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
