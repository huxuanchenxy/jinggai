using Dapper;
using MSS.API.Common;
using SZY.Platform.WebApi.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Coded By admin 2019/9/27 11:18:53
namespace SZY.Platform.WebApi.Data
{
    public interface IConstructionPlanMonthDetailRepo<T>
    {
        Task<ConstructionPlanMonthDetails> ListPage(ConstructionPlanMonthDetailParm parm);
        Task<ConstructionPlanMonthDetail> GetByID(int id);
        Task<List<ConstructionPlanMonthDetail>> GetByIDs(List<int> ids);
        Task<int> Update(ConstructionPlanMonthDetail obj);
    }

    public class ConstructionPlanMonthDetailRepo : BaseRepo, IConstructionPlanMonthDetailRepo<ConstructionPlanMonthDetail>
    {
        public ConstructionPlanMonthDetailRepo(DapperOptions options) : base(options)
        {
        }

        public async Task<ConstructionPlanMonthDetails> ListPage(ConstructionPlanMonthDetailParm parm)
        {
            return await WithConnection(async c =>
            {
                ConstructionPlanMonthDetails ret = new ConstructionPlanMonthDetails();
                string sql = "SELECT c.*,ot.name as team_name,d.name,dt.name as pm_type_name " +
                " FROM construction_plan_month_detail c " +
                " left join org_tree ot on ot.id=c.team " +
                " left join dictionary_tree d on d.id=c.work_type " +
                " left join dictionary_tree dt on dt.id=c.pm_type ";
                string sqlwhere="";
                if (parm.Query != null)
                {
                    sqlwhere = " where c.query=" + parm.Query + " and c.month=" + parm.Month;
                }
                else if (parm.Year!=null && parm.Line!=null && parm.Company!=null && parm.Department!=null)
                {
                    string tmpSql = "(select id from construction_plan_import_common where year="+parm.Year
                    + " and department="+parm.Department
                    + " and company=" + parm.Company
                    + " and line=" + parm.Line + ")";
                    sqlwhere = " where c.query=" + tmpSql + " and c.month=" + parm.Month +" and c.is_assigned=0 ";
                }
                if (parm.Team!=null)
                {
                    sqlwhere += " and c.team=" + parm.Team;
                }
                if (parm.Location!=null && parm.LocationBy!=null)
                {
                    sqlwhere += " and c.location=" + parm.Location + " and c.location_by=" + parm.LocationBy;
                }
                if (!string.IsNullOrWhiteSpace(parm.PlanDate))
                {
                    string[] tmpDate = parm.PlanDate.Split('.');
                    string lastDay = SZY.Platform.WebApi.Model.Common.GetLastDay(Convert.ToInt32(tmpDate[1]), Convert.ToInt32(tmpDate[0]));
                    string myDate = tmpDate[0] + "." + tmpDate[1] + ".01-" + tmpDate[0] + "." + tmpDate[1] + "." + lastDay;
                    sqlwhere += " and (c.plan_date='" + parm.PlanDate + "' or c.plan_date='" + myDate+"') ";
                }
                sql = sql + sqlwhere + " order by plan_date limit " + (parm.page - 1) * parm.rows + "," + parm.rows;
                var tmp = await c.QueryAsync<ConstructionPlanMonthDetail>(sql);
                if (tmp.Count()>0)
                {
                    sql= "select count(*) FROM construction_plan_month_detail c " + sqlwhere;
                    ret.total = await c.QueryFirstOrDefaultAsync<int>(sql);
                    ret.rows = tmp.ToList();
                }
                else
                {
                    ret.rows = new List<ConstructionPlanMonthDetail>();
                    ret.total = 0 ;
                }
                return ret;
            });
        }

        public async Task<ConstructionPlanMonthDetail> GetByID(int id)
        {
            return await WithConnection(async c =>
            {
                ConstructionPlanMonthDetails ret = new ConstructionPlanMonthDetails();
                string sql = "SELECT *,ot.name as team_name,d.name,dt.name as pm_type_name " +
                " FROM construction_plan_month_detail c " +
                " left join org_tree ot on ot.id=c.team " +
                " left join dictionary_tree d on d.id=c.work_type " +
                " left join dictionary_tree dt on dt.id=c.pm_type where c.id=@id";
                return await c.QueryFirstOrDefaultAsync<ConstructionPlanMonthDetail>(sql,new { id});
            });
        }

        public async Task<List<ConstructionPlanMonthDetail>> GetByIDs(List<int> ids)
        {
            return await WithConnection(async c =>
            {
                ConstructionPlanMonthDetails ret = new ConstructionPlanMonthDetails();
                string sql = "SELECT *,ot.name as team_name,d.name,dt.name as pm_type_name " +
                " FROM construction_plan_month_detail c " +
                " left join org_tree ot on ot.id=c.team " +
                " left join dictionary_tree d on d.id=c.work_type " +
                " left join dictionary_tree dt on dt.id=c.pm_type where c.id in @ids";
                return (await c.QueryAsync<ConstructionPlanMonthDetail>(sql, new { ids })).ToList();
            });
        }
        public async Task<int> Update(ConstructionPlanMonthDetail obj)
        {
            return await WithConnection(async c =>
            {
                string sql = " update construction_plan_month_detail " +
                        " set work_type =@WorkType,pm_type=@PMType,update_time=@UpdateTime," +
                        " update_by=@UpdateBy where id=@id ";
                int ret = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                return ret;
            });
        }

    }
}



