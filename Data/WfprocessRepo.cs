using Dapper;
using SZY.Platform.WebApi.Model;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Coded By admin 2019/11/9 13:45:19
namespace SZY.Platform.WebApi.Data
{
    public interface IWfprocessRepo<T> where T : BaseEntity
    {
        Task<WfprocessPageView> GetPageList(WfprocessParm param);
        Task<Wfprocess> Save(Wfprocess obj);
        Task<Wfprocess> GetByID(long id);
        Task<int> Update(Wfprocess obj);
        Task<int> Delete(string[] ids, int userID);
    }

    public class WfprocessRepo : BaseRepo, IWfprocessRepo<Wfprocess>
    {
        public WfprocessRepo(DapperOptions options) : base(options) { }

        public async Task<WfprocessPageView> GetPageList(WfprocessParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT 
                ID,
                ProcessGUID,
                ProcessName,
                Version,
                IsUsing,
                AppType,
                PageUrl,
                XmlFileName,
                XmlFilePath,
                XmlContent,
                StartType,
                StartExpression,
                EndType,
                EndExpression,
                Description,
                CreatedDateTime,LastUpdatedDateTime FROM wfprocess
                 ");
                StringBuilder whereSql = new StringBuilder();
                //whereSql.Append(" WHERE ai.ProcessInstanceID = '" + parm.ProcessInstanceID + "'");

                //if (parm.AppName != null)
                //{
                //    whereSql.Append(" and ai.AppName like '%" + parm.AppName.Trim() + "%'");
                //}

                sql.Append(whereSql);
                //验证是否有参与到流程中
                //string sqlcheck = sql.ToString();
                //sqlcheck += ("AND ai.CreatedByUserID = '" + parm.UserID + "'");
                //var checkdata = await c.QueryFirstOrDefaultAsync<TaskViewModel>(sqlcheck);
                //if (checkdata == null)
                //{
                //    return null;
                //}

                var data = await c.QueryAsync<Wfprocess>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<Wfprocess>(sql.ToString());

                WfprocessPageView ret = new WfprocessPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        public async Task<Wfprocess> Save(Wfprocess obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `wfprocess`(
                    ID,
                    ProcessGUID,
                    ProcessName,
                    Version,
                    IsUsing,
                    AppType,
                    PageUrl,
                    XmlFileName,
                    XmlFilePath,
                    XmlContent,
                    StartType,
                    StartExpression,
                    EndType,
                    EndExpression,
                    Description,
                    CreatedDateTime,
                    LastUpdatedDateTime
                ) VALUES 
                (@ID,
                    @ProcessGUID,
                    @ProcessName,
                    @Version,
                    @IsUsing,
                    @AppType,
                    @PageUrl,
                    @XmlFileName,
                    @XmlFilePath,
                    @XmlContent,
                    @StartType,
                    @StartExpression,
                    @EndType,
                    @EndExpression,
                    @Description,
                    @CreatedDateTime,
                    @LastUpdatedDateTime
                    );
                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.ID = newid;
                return obj;
            });
        }

        public async Task<Wfprocess> GetByID(long id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<Wfprocess>(
                    "SELECT * FROM wfprocess WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<int> Update(Wfprocess obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" UPDATE wfprocess set 
                    ID=@ID,
                    ProcessGUID=@ProcessGUID,
                    ProcessName=@ProcessName,
                    Version=@Version,
                    IsUsing=@IsUsing,
                    AppType=@AppType,
                    PageUrl=@PageUrl,
                    XmlFileName=@XmlFileName,
                    XmlFilePath=@XmlFilePath,
                    XmlContent=@XmlContent,
                    StartType=@StartType,
                    StartExpression=@StartExpression,
                    EndType=@EndType,
                    EndExpression=@EndExpression,
                    Description=@Description,
                    CreatedDateTime=@CreatedDateTime,
                    LastUpdatedDateTime=@LastUpdatedDateTime
                 where id=@Id", obj);
                return result;
            });
        }

        public async Task<int> Delete(string[] ids, int userID)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Update wfprocess set is_del=1" +
                ",updated_time=@updated_time,updated_by=@updated_by" +
                " WHERE id in @ids ", new { ids = ids, updated_time = DateTime.Now, updated_by = userID });
                return result;
            });
        }

        
    }
}



