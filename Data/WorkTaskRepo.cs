using Dapper;
using SZY.Platform.WebApi.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZY.Platform.WebApi.Data
{

    public interface IWorkTaskRepo<T> where T : BaseEntity
    {
        Task<WorkTaskPageView> GetPageList(WorkTaskQueryParm param);
        Task<WorkTaskPageView> GetPageMyApply(WorkTaskQueryParm parm);
        Task<WorkTaskPageView> GetPageActivityInstance(WorkQueryParm parm);
        Task<Myfund> Save2(Myfund obj);
        Task<int> Update(Myfund obj);
        Task<Myfund> GetByCode(string code);
        Task<int> Update2(Myfund obj);
        Task<MyfundPageView> GetPageList(MyfundParm parm);
        Task<Myfund> GetById(int id);
        Task<Myfund> GetAllBalance();
        Task<int> UpdateExpectGrowth(Myfund obj);
        Task<JingGai> SaveJingGai(JingGai obj);
        Task<JingGaiDevice> SaveJingGaiDevice(JingGaiDevice obj);
        Task<JingGaiAlarm> SaveJingGaiAlarm(JingGaiAlarm obj);
        Task<JingGaiDevicePageView> GetPageJingGaiDevice(JingGaiDeviceParm parm);
    }

    public class WorkTaskRepo : BaseRepo, IWorkTaskRepo<TaskViewModel>
    {
        public WorkTaskRepo(DapperOptions options) : base(options) { }

        public async Task<MyfundPageView> GetPageList(MyfundParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT 
                id,
                code,
                name,
                balance,
                costavg,
                networth,
                totalworth,
                worthdate,expectgrowth,
                daygrowth,updatetime FROM myfund2
                 ");
                StringBuilder whereSql = new StringBuilder();


                sql.Append(whereSql);

                var data = await c.QueryAsync<Myfund>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<Myfund>(sql.ToString());

                MyfundPageView ret = new MyfundPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }
        public async Task<WorkTaskPageView> GetPageList(WorkTaskQueryParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@" SELECT 
                                t.ID,
                                t.AppName,
                                t.AppInstanceID,
                                ai.ProcessGUID,
                                pi.Version,
                                t.ProcessInstanceID,
                                ai.ActivityGUID,
                                t.ActivityInstanceID,
                                ai.ActivityName,
                                ai.ActivityType,
                                ai.WorkItemType,
                                ai.CreatedByUserID,
                                ai.CreatedByUserName,
                                ai.CreatedDateTime,
                                t.TaskType,
                                t.EntrustedTaskID,
                                t.AssignedToUserID,
                                t.AssignedToUserName,
                                t.CreatedDateTime,
                                t.LastUpdatedDateTime,
                                t.EndedDateTime,
                                t.EndedByUserID,
                                t.EndedByUserName,
                                t.TaskState,
                                ai.ActivityState,
                                t.RecordStatusInvalid,
                                pi.ProcessState,
                                ai.ComplexType,
                                ai.MIHostActivityInstanceID,
                                pi.AppInstanceCode,
                                pi.ProcessName,
                                pi.CreatedByUserID,
                                pi.CreatedByUserName,
                                pi.CreatedDateTime,
                                CASE WHEN ai.MIHostActivityInstanceID is null THEN ai.ActivityState ELSE ai1.ActivityState END MiHostState
                            FROM
                                WfActivityInstance ai
                                    INNER JOIN
                                WfTasks t ON ai.ID = t.ActivityInstanceID
                                    INNER JOIN
                                WfProcessInstance pi ON ai.ProcessInstanceID = pi.ID
                                    LEFT JOIN
                                WfActivityInstance ai1 ON ai.MIHostActivityInstanceID = ai1.ID ");
                StringBuilder whereSql = new StringBuilder();
                whereSql.Append(" WHERE pi.ProcessState = '2' AND ( ai.ActivityType = '4' OR ai.WorkItemType = '1' ) AND t.TaskState <> '32' ");
                if (parm.ActivityState != null)
                {
                    if (parm.ActivityState == 1)
                    {
                        whereSql.Append(" and ai.ActivityState != 4 ");
                    }
                    else
                    {
                        whereSql.Append(" and ai.ActivityState =" + parm.ActivityState);
                    }
                    
                }
                if (parm.AssignedToUserID != null)
                {
                    whereSql.Append(" and t.AssignedToUserID =" + parm.AssignedToUserID);
                }
                if (parm.AppName != null)
                {
                    whereSql.Append(" and t.AppName like '%" + parm.AppName.Trim() + "%'");
                }

                sql.Append(whereSql);


                var data = await c.QueryAsync<TaskViewModel>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<TaskViewModel>(sql.ToString());

                WorkTaskPageView ret = new WorkTaskPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        /// <summary>
        /// 我的申请
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<WorkTaskPageView> GetPageMyApply(WorkTaskQueryParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT pi.ID,
		                                ai.AppName,
		                                ai.ProcessGUID,
		                                pi.Version,
		                                ai.ActivityGUID,
		                                ai.ActivityName,
		                                ai.ActivityType,
		                                ai.WorkItemType,
		                                ai.ActivityState,
		                                pi.ProcessState,
		                                ai.ComplexType,
		                                ai.MIHostActivityInstanceID,
		                                pi.AppInstanceCode,
		                                pi.ProcessName,
		                                pi.CreatedByUserID,
		                                pi.CreatedByUserName,
		                                pi.CreatedDateTime
                                FROM
		                                WfActivityInstance ai
                                INNER JOIN
		                                WfProcessInstance pi ON ai.ProcessInstanceID = pi.ID ");
                StringBuilder whereSql = new StringBuilder();
                whereSql.Append(" WHERE ai.ActivityType = '1' AND pi.CreatedByUserID = '" + parm.AssignedToUserID + "' ");

                if (parm.AppName != null)
                {
                    whereSql.Append(" and ai.AppName like '%" + parm.AppName.Trim() + "%'");
                }

                sql.Append(whereSql);


                var data = await c.QueryAsync<TaskViewModel>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<TaskViewModel>(sql.ToString());

                WorkTaskPageView ret = new WorkTaskPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        public async Task<WorkTaskPageView> GetPageActivityInstance(WorkQueryParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT pi.ID,
		                                ai.AppName,
		                                ai.ProcessGUID,
		                                pi.Version,
		                                ai.ActivityGUID,
		                                ai.ActivityName,
		                                ai.ActivityType,
		                                ai.WorkItemType,
		                                ai.ActivityState,
		                                pi.ProcessState,
		                                ai.ComplexType,
		                                ai.MIHostActivityInstanceID,
		                                pi.AppInstanceCode,
		                                pi.ProcessName,
		                                pi.CreatedByUserID,
		                                pi.CreatedByUserName,
		                                pi.CreatedDateTime,
                                        ai.EndedByUserName,
                                        ai.LastUpdatedByUserName,
                                        ai.LastUpdatedDateTime
                                FROM
		                                WfActivityInstance ai
                                INNER JOIN
		                                WfProcessInstance pi ON ai.ProcessInstanceID = pi.ID ");
                StringBuilder whereSql = new StringBuilder();
                whereSql.Append(" WHERE ai.ProcessInstanceID = '" + parm.ProcessInstanceID + "'");

                //if (parm.AppName != null)
                //{
                //    whereSql.Append(" and ai.AppName like '%" + parm.AppName.Trim() + "%'");
                //}
                //if (parm.ProcessInstanceID != null)
                //{
                //    whereSql.Append(" and ai.ProcessInstanceID = '" + parm.ProcessInstanceID + "'");
                //}

                sql.Append(whereSql);

                //验证是否有参与到流程中
                string sqlcheck = sql.ToString();
                sqlcheck += ("AND ai.CreatedByUserID = '" + parm.UserID + "'");
                var checkdata = await c.QueryFirstOrDefaultAsync<TaskViewModel>(sqlcheck);
                if (checkdata == null)
                {
                    return null;
                }

                var data = await c.QueryAsync<TaskViewModel>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<TaskViewModel>(sql.ToString());

                WorkTaskPageView ret = new WorkTaskPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        public async Task<Myfund> Save2(Myfund obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `myfund2`(
                    
                    code,
                    name,
                    balance,
                    costavg,
                    networth,
                    totalworth,
                    worthdate,
                    daygrowth,
                    updatetime
                ) VALUES 
                (
                    @Code,
                    @Name,
                    @Balance,
                    @Costavg,
                    @Networth,
                    @Totalworth,
                    @Worthdate,
                    @Daygrowth,
                    @Updatetime
                    );
                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.Id = newid;
                return obj;
            });
        }

        public async Task<int> Update(Myfund obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" UPDATE myfund2 set 
                    networth=@Networth,
                    totalworth=@Totalworth,
                    worthdate=@Worthdate,
                    daygrowth=@Daygrowth,
                    updatetime=@Updatetime
                 where id=@Id", obj);
                return result;
            });
        }

        public async Task<int> Update2(Myfund obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" UPDATE myfund2 set 
                    balance=@Balance,
                    costavg=@Costavg,
                    updatetime=@Updatetime
                 where id=@Id", obj);
                return result;
            });
        }

        public async Task<int> UpdateExpectGrowth(Myfund obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" UPDATE myfund2 set 
                    expectgrowth=@ExpectGrowth
                 where id=@Id", obj);
                return result;
            });
        }

        public async Task<Myfund> GetByCode(string code)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<Myfund>(
                    "SELECT * FROM myfund2 WHERE code = @code", new { code = code });
                return result;
            });
        }

        public async Task<Myfund> GetById(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<Myfund>(
                    "SELECT * FROM myfund2 WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<Myfund> GetAllBalance()
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<Myfund>(
                    "SELECT SUM(balance) balance FROM myfund2");
                return result;
            });
        }


        public async Task<JingGai> SaveJingGai(JingGai obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `aisense`.`jinggai`(`device_id`, `device_type`, `fv`, `soc`, `sor`, `rtd`, `rad`, `rqd`, `sol`, `lng`, `lat`, `sensor_water_level`, `sensor_water_depth`, `sensor_temperature`, `sensor_humidity`, `sensor_smoke`, `sensor_ch4`, `sensor_toxic`, `sensor_water_alarm`, `sensor_water_warn`, `sensor_ph`, `sensor_ch4_conc`, `sensor_toxic_conc`, `date1`) VALUES 
(@device_id, @device_type,@fv,@soc,@sor,@rtd,@rad,@rqd,@sol,@lng,@lat,@sensor_water_level,@sensor_water_depth,@sensor_temperature,@sensor_humidity,@sensor_smoke,@sensor_ch4,@sensor_toxic,@sensor_water_alarm,@sensor_water_warn,@sensor_ph,@sensor_ch4_conc,@sensor_toxic_conc,@date1);

                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.id = newid;
                return obj;
            });
        }

        public async Task<JingGaiDevice> SaveJingGaiDevice(JingGaiDevice obj)
        {
            return await WithConnection(async c =>
            {
                string sqltemp = $@" SELECT * FROM jinggai_device WHERE device_id = @device_id ";
                var tempdata = await c.QueryFirstOrDefaultAsync<JingGaiDevice>(sqltemp, obj);
                if (tempdata != null)
                {
                    string sqlupdate = $@" UPDATE `aisense`.`jinggai_device` SET `device_type` = @device_type, `device_name` = @device_name, `install_addr` = @install_addr, `install_time` = @install_time, `lng` = @lng, `lat` = @lat, `date1` = @date1 WHERE `device_id` = @device_id;";
                    await c.ExecuteAsync(sqlupdate,obj);
                    return obj;
                }
                else {
                    string sql = $@" INSERT INTO `aisense`.`jinggai_device`( `device_id`, `device_type`, `device_name`, `install_addr`, `install_time`, `lng`, `lat`, `date1`) VALUES (@device_id,@device_type,@device_name,@install_addr,@install_time,@lng,@lat,@date1);
                    ";
                    sql += "SELECT LAST_INSERT_ID() ";
                    int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                    obj.id = newid;
                    return obj;
                }
                
            });
        }


        public async Task<JingGaiAlarm> SaveJingGaiAlarm(JingGaiAlarm obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `aisense`.`jinggai_alarm`(`device_id`, `device_name`, `device_addr`, `device_type`, `alarm_item`, `alarm_item_name`, `alarm_value`, `compare_type`, `threshold`, `date1`) VALUES (@device_id,@device_name,@device_addr,@device_type,@alarm_item,@alarm_item_name,@alarm_value,@compare_type,@threshold,@date1);

                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.id = newid;
                return obj;
            });
        }


        public async Task<JingGaiDevicePageView> GetPageJingGaiDevice(JingGaiDeviceParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT * FROM jinggai_device ");
                var ets = await c.QueryAsync<JingGaiDevice>(sql.ToString());

                JingGaiDevicePageView ret = new JingGaiDevicePageView();
                ret.rows = ets.ToList();
                return ret;
            });
        }
    }

}
