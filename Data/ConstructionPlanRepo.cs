using Dapper;
using SZY.Platform.WebApi.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSS.API.Common.MyDictionary;


// Coded By admin 2019/10/21 13:06:57
namespace SZY.Platform.WebApi.Data
{
    public interface IConstructionPlanRepo<T> where T : BaseEntity
    {
        Task<ConstructionPlanPageView> GetPageList(ConstructionPlanParm param);
        Task<ConstructionPlan> Save(ConstructionPlan obj);
        Task<ConstructionPlan> GetByID(long id);
        Task<int> Update(ConstructionPlan obj);
        Task<int> Delete(string[] ids, int userID);
    }

    public class ConstructionPlanRepo : BaseRepo, IConstructionPlanRepo<ConstructionPlan>
    {
        public ConstructionPlanRepo(DapperOptions options) : base(options) { }

        public async Task<ConstructionPlanPageView> GetPageList(ConstructionPlanParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT 
                a.id,
                a.line_id,
                a.area_id,
                a.area_typename,
                a.plan_name,
                a.plan_type,
                a.plan_number,
                a.important_level,
                a.plan_level,
                a.apply_company_org_id,
                a.construction_company_org_id,
                a.start_time,
                a.end_time,
                a.register_station_id,
                a.device_num,
                a.trouble_num,
                a.operation_address,
                a.construction_content,
                a.construction_detail,
                a.coordination_request,
                a.coordination_audit,
                a.stop_electric,
                a.traction_power,
                a.traction_train_num,
                a.traction_start_time,
                a.traction_time,
                a.use_laddercar,
                a.electric_range,
                a.traction_car_route,
                a.effect_area,
                a.effect_explain,
                a.safe_measure,
                a.force_unlock_key,
                a.other_request,
                a.memo,
                a.created_time,
                a.created_by,
                a.updated_time,
                a.updated_by,a.is_del,b.processstate,a.con_plan_type FROM construction_plan a
                LEFT JOIN wfprocessinstance b ON a.id = b.AppInstanceID AND b.ProcessGUID = '" + parm.processGUID + "'");
                StringBuilder whereSql = new StringBuilder();
                whereSql.Append(" WHERE a.is_del = 0 ");

                if (parm.planName != null)
                {
                    whereSql.Append(" and a.plan_name like '%" + parm.planName.Trim() + "%'");
                }
                if (parm.userId != 0)
                {
                    whereSql.Append(" and a.created_by = '" + parm.userId + "' ");
                }
                if (parm.conplantype != 0)
                {
                    whereSql.Append(" and a.con_plan_type = '" + parm.conplantype + "' ");
                }

                sql.Append(whereSql);
                //验证是否有参与到流程中
                //string sqlcheck = sql.ToString();
                //sqlcheck += ("AND ai.CreatedByUserID = '" + parm.UserID + "'");
                //var checkdata = await c.QueryFirstOrDefaultAsync<TaskViewModel>(sqlcheck);
                //if (checkdata == null)
                //{
                //    return null;
                //}

                var data = await c.QueryAsync<ConstructionPlan>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<ConstructionPlan>(sql.ToString());

                ConstructionPlanPageView ret = new ConstructionPlanPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        public async Task<ConstructionPlan> Save(ConstructionPlan obj)
        {
            return await WithConnection(async c =>
            {
                IDbTransaction trans = c.BeginTransaction();
                try
                {

                    string sql = $@" INSERT INTO `construction_plan`(
                    
                    line_id,
                    area_id,
                    area_typename,
                    plan_name,
                    plan_type,
                    plan_number,
                    important_level,
                    plan_level,
                    apply_company_org_id,
                    construction_company_org_id,
                    start_time,
                    end_time,
                    register_station_id,
                    device_num,
                    trouble_num,
                    operation_address,
                    construction_content,
                    construction_detail,
                    coordination_request,
                    coordination_audit,
                    stop_electric,
                    traction_power,
                    traction_train_num,
                    traction_start_time,
                    traction_time,
                    use_laddercar,
                    electric_range,
                    traction_car_route,
                    effect_area,
                    effect_explain,
                    safe_measure,
                    force_unlock_key,
                    other_request,
                    memo,
                    created_time,
                    created_by,
                    updated_time,
                    updated_by,
                    is_del,
                    con_plan_type
                ) VALUES 
                (
                    @LineId,
                    @AreaId,
                    @AreaTypename,
                    @PlanName,
                    @PlanType,
                    @PlanNumber,
                    @ImportantLevel,
                    @PlanLevel,
                    @ApplyCompanyOrgId,
                    @ConstructionCompanyOrgId,
                    @StartTime,
                    @EndTime,
                    @RegisterStationId,
                    @DeviceNum,
                    @TroubleNum,
                    @OperationAddress,
                    @ConstructionContent,
                    @ConstructionDetail,
                    @CoordinationRequest,
                    @CoordinationAudit,
                    @StopElectric,
                    @TractionPower,
                    @TractionTrainNum,
                    @TractionStartTime,
                    @TractionTime,
                    @UseLaddercar,
                    @ElectricRange,
                    @TractionCarRoute,
                    @EffectArea,
                    @EffectExplain,
                    @SafeMeasure,
                    @ForceUnlockKey,
                    @OtherRequest,
                    @Memo,
                    @CreatedTime,
                    @CreatedBy,
                    @UpdatedTime,
                    @UpdatedBy,
                    @IsDel,
                    @ConPlanType
                    );
                    ";
                    sql += "SELECT LAST_INSERT_ID() ";
                    int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj, trans);
                    obj.Id = newid;
                    if (!string.IsNullOrWhiteSpace(obj.FileIDs))
                    {
                        List<object> objs = new List<object>();
                        JArray jobj = JsonConvert.DeserializeObject<JArray>(obj.FileIDs);
                        foreach (var o in jobj)
                        {
                            foreach (var item in o["ids"].ToString().Split(','))
                            {
                                objs.Add(new
                                {
                                    entityID = newid,
                                    fileID = Convert.ToInt32(item),
                                    type = Convert.ToInt32(o["type"]),
                                    systemResource = (int)SystemResource.ConstructionPlan
                                });
                            }
                        }
                        sql = "insert into upload_file_relation values (0,@entityID,@fileID,@type,@systemResource)";
                        int ret = await c.ExecuteAsync(sql, objs, trans);
                    }
                    trans.Commit();
                    return obj;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.ToString());
                }
            });
        }

        public async Task<ConstructionPlan> GetByID(long id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<ConstructionPlan>(
                    "SELECT * FROM construction_plan WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<int> Update(ConstructionPlan obj)
        {
            return await WithConnection(async c =>
            {
                IDbTransaction trans = c.BeginTransaction();
                try
                {
                    var result = await c.ExecuteAsync($@" UPDATE construction_plan set 
                    
                    line_id=@LineId,
                    area_id=@AreaId,
                    area_typename=@AreaTypename,
                    plan_name=@PlanName,
                    plan_type=@PlanType,
                    plan_number=@PlanNumber,
                    important_level=@ImportantLevel,
                    plan_level=@PlanLevel,
                    apply_company_org_id=@ApplyCompanyOrgId,
                    construction_company_org_id=@ConstructionCompanyOrgId,
                    start_time=@StartTime,
                    end_time=@EndTime,
                    register_station_id=@RegisterStationId,
                    device_num=@DeviceNum,
                    trouble_num=@TroubleNum,
                    operation_address=@OperationAddress,
                    construction_content=@ConstructionContent,
                    construction_detail=@ConstructionDetail,
                    coordination_request=@CoordinationRequest,
                    coordination_audit=@CoordinationAudit,
                    stop_electric=@StopElectric,
                    traction_power=@TractionPower,
                    traction_train_num=@TractionTrainNum,
                    traction_start_time=@TractionStartTime,
                    traction_time=@TractionTime,
                    use_laddercar=@UseLaddercar,
                    electric_range=@ElectricRange,
                    traction_car_route=@TractionCarRoute,
                    effect_area=@EffectArea,
                    effect_explain=@EffectExplain,
                    safe_measure=@SafeMeasure,
                    force_unlock_key=@ForceUnlockKey,
                    other_request=@OtherRequest,
                    memo=@Memo,
                    updated_time=@UpdatedTime,
                    updated_by=@UpdatedBy,
                    is_del=@IsDel,
                    con_plan_type=@ConPlanType
                 where id=@Id", obj, trans);
                    if (!string.IsNullOrWhiteSpace(obj.FileIDs))
                    {
                        string delsql = $@" DELETE FROM upload_file_relation WHERE entity_id = '{obj.Id}' AND system_resource = '{(int)SystemResource.ConstructionPlan}' ";
                        await c.ExecuteAsync(delsql, trans);
                        List<object> objs = new List<object>();
                        JArray jobj = JsonConvert.DeserializeObject<JArray>(obj.FileIDs);
                        foreach (var o in jobj)
                        {
                            foreach (var item in o["ids"].ToString().Split(','))
                            {
                                objs.Add(new
                                {
                                    entityID = obj.Id,
                                    fileID = Convert.ToInt32(item),
                                    type = Convert.ToInt32(o["type"]),
                                    systemResource = (int)SystemResource.ConstructionPlan
                                });
                            }
                        }
                        string sql = "insert into upload_file_relation values (0,@entityID,@fileID,@type,@systemResource)";
                        int ret = await c.ExecuteAsync(sql, objs, trans);
                    }
                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.ToString());
                }
            });
        }

        public async Task<int> Delete(string[] ids, int userID)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Update construction_plan set is_del=1" +
                ",updated_time=@updated_time,updated_by=@updated_by" +
                " WHERE id in @ids ", new { ids = ids, updated_time = DateTime.Now, updated_by = userID });
                return result;
            });
        }
    }
}



