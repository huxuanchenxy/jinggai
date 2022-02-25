using Dapper;
using SZY.Platform.WebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Coded By admin 2019/11/29 10:42:22
namespace SZY.Platform.WebApi.Data
{
    public interface IConstructionPlanMonthChartRepo<T> where T : BaseEntity
    {
        Task<ConstructionPlanMonthChartPageView> GetPageList(ConstructionPlanMonthChartParm param);
        Task<ConstructionPlanMonthChart> Save(ConstructionPlanMonthChart obj);
        Task<ConstructionPlanMonthChart> GetByID(long id);
        Task<int> Update(ConstructionPlanMonthChart obj);
        Task<int> Delete(string[] ids, int userID);
        Task<List<ConstructionPlanMonthChart>> GetByParm(ConstructionPlanMonthChartParm parm);
    }

    public class ConstructionPlanMonthChartRepo : BaseRepo, IConstructionPlanMonthChartRepo<ConstructionPlanMonthChart>
    {
        public ConstructionPlanMonthChartRepo(DapperOptions options) : base(options) { }

        public async Task<ConstructionPlanMonthChartPageView> GetPageList(ConstructionPlanMonthChartParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT 
                id,
                year,
                month,
                day,
                line,
                work_type,
                eqp_type,
                location,
                location_by,
                department,
                team,
                pm_type,
                pm_frequency,
                unit,
                plan_quantity,
                plan_date,
                real_quantity,
                real_date,
                working_order,
                order_status,
                remark,
                query,
                update_time,update_by FROM construction_plan_month_chart
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

                var data = await c.QueryAsync<ConstructionPlanMonthChart>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<ConstructionPlanMonthChart>(sql.ToString());

                ConstructionPlanMonthChartPageView ret = new ConstructionPlanMonthChartPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        public async Task<ConstructionPlanMonthChart> Save(ConstructionPlanMonthChart obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `construction_plan_month_chart`(
                    
                    year,
                    month,
                    day,
                    line,
                    work_type,
                    eqp_type,
                    location,
                    location_by,
                    department,
                    team,
                    pm_type,
                    pm_frequency,
                    unit,
                    plan_quantity,
                    plan_date,
                    real_quantity,
                    real_date,
                    working_order,
                    order_status,
                    remark,
                    query,
                    update_time,
                    update_by
                ) VALUES 
                (
                    @Year,
                    @Month,
                    @Day,
                    @Line,
                    @WorkType,
                    @EqpType,
                    @Location,
                    @LocationBy,
                    @Department,
                    @Team,
                    @PmType,
                    @PmFrequency,
                    @Unit,
                    @PlanQuantity,
                    @PlanDate,
                    @RealQuantity,
                    @RealDate,
                    @WorkingOrder,
                    @OrderStatus,
                    @Remark,
                    @Query,
                    @UpdateTime,
                    @UpdateBy
                    );
                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.Id = newid;
                return obj;
            });
        }

        public async Task<ConstructionPlanMonthChart> GetByID(long id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<ConstructionPlanMonthChart>(
                    "SELECT * FROM construction_plan_month_chart WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<List<ConstructionPlanMonthChart>> GetByParm(ConstructionPlanMonthChartParm parm)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                sql = sql.Append("  SELECT a.*,c.status,c.updated_time donetime" +
                    " FROM construction_plan_month_chart a LEFT JOIN pm_entity_month_detail b" +
                    " ON a.id = b.month_detail LEFT JOIN pm_entity c ON c.id = b.pm_entity WHERE 1 = 1 ");

                StringBuilder whereSql = new StringBuilder();
                //whereSql.Append(" WHERE ai.ProcessInstanceID = '" + parm.ProcessInstanceID + "'");

                if (parm.xAxisType == 1)
                {
                    
                    if (parm.month != 0)
                    {
                        whereSql.Append(" and a.month = '" + parm.month + "'");
                    }
                    if (parm.year != 0)
                    {
                        whereSql.Append(" and a.year = '" + parm.year + "'");
                    }
                }
                if (parm.xAxisType == 2)
                {
                    if (parm.year != 0)
                    {
                        whereSql.Append(" and a.year = '" + parm.year + "'");
                    }
                    if (parm.startMonth != 0 && parm.endMonth != 0)
                    {
                        whereSql.Append(" and ( a.month >= '" + parm.startMonth + "'AND a.month <= '" + parm.endMonth + "' ) ");
                    }
                }
                if (parm.xAxisType == 3)
                {
                    if (parm.startYear != 0 && parm.endYear != 0)
                    {
                        whereSql.Append(" and ( a.year >= '" + parm.startYear + "'AND a.year <= '" + parm.endYear + "' ) ");
                    }
                }
                if (parm.team != 0)
                {
                    whereSql.Append(" and a.team = '" + parm.team + "'");
                }
                sql.Append(whereSql);

                List<ConstructionPlanMonthChart> result = new List<ConstructionPlanMonthChart>();
                var data = await c.QueryAsync<ConstructionPlanMonthChart>(sql.ToString());
                result = data.ToList();
                return result;
            });
        }

        public async Task<int> Update(ConstructionPlanMonthChart obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" UPDATE construction_plan_month_chart set 
                    
                    year=@Year,
                    month=@Month,
                    day=@Day,
                    line=@Line,
                    work_type=@WorkType,
                    eqp_type=@EqpType,
                    location=@Location,
                    location_by=@LocationBy,
                    department=@Department,
                    team=@Team,
                    pm_type=@PmType,
                    pm_frequency=@PmFrequency,
                    unit=@Unit,
                    plan_quantity=@PlanQuantity,
                    plan_date=@PlanDate,
                    real_quantity=@RealQuantity,
                    real_date=@RealDate,
                    working_order=@WorkingOrder,
                    order_status=@OrderStatus,
                    remark=@Remark,
                    query=@Query,
                    update_time=@UpdateTime,
                    update_by=@UpdateBy
                 where id=@Id", obj);
                return result;
            });
        }

        public async Task<int> Delete(string[] ids, int userID)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Update construction_plan_month_chart set is_del=1" +
                ",updated_time=@updated_time,updated_by=@updated_by" +
                " WHERE id in @ids ", new { ids = ids, updated_time = DateTime.Now, updated_by = userID });
                return result;
            });
        }
    }
}



