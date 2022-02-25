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
    public interface IMaintenanceRepo<T>
    {
        #region 已弃用
        Task<int> SaveMItem(MaintenanceItem maintenanceItem);

        Task<int> SaveMMoudleItem(List<MaintenanceModuleItem> maintenanceModuleItem);

        Task<int> SaveMMoudleItemValue(List<MaintenanceModuleItemValue> maintenanceModuleItemValues);
        Task<int> DelMMoudleItemValue(int list);

        Task<int> SaveMModule(MaintenanceModule maintenanceModule);

        Task<int> SaveMList(MaintenanceList maintenanceList);
        Task<int> UpdateMList(int status, int user,int id);
        Task<int> SaveMDetail(List<MaintenancePlanDetail> maintenancePlanDetail);
        Task<MaintenanceListView> ListPage(MaintenanceListParm parm);

        Task<List<MaintenanceModuleItemAll>> ListItems(int id);
        Task<List<MaintenanceModuleItemAll>> ListValues(int id);
        #endregion

        Task<int> SavePMModule(PMModule pmModule);
        Task<object> ListModulePage(PMModuleParm parm);
        Task<PMModule> GetModuleByID(int id);
        Task<PMModule> GetModuleByIDEasy(int id);
        Task<int> SavePMEntity(PMEntity pmEntity);
        Task<PMEntityView> ListEntityPage(PMEntityParm parm);
        Task<int> DelPMEntity(string[] ids);
        Task<PMEntity> GetEntityByID(int id);
        Task<PMEntity> GetEntityDetailByID(int id);
        Task<List<PMEntity>> GetEntityByIDs(string[] ids);

        Task<int> SavePMEntityMonthDetail(List<PMEntityMonthDetail> pmEntityMonthDetails);
        Task<int> UpdatePMEntity(PMEntity pmEntity);
        Task<int> UpdatePMEntityStatus(int id, int status, int userID);
        Task<int> DelPMEntityMonthDetail(string[] ids);
        Task<List<int>> ListMonthDetail(int id);

        Task<int> SaveEqpHistory(EqpHistory eqp);
    }

    public class MaintenanceRepo : BaseRepo, IMaintenanceRepo<MaintenanceItem>
    {
        public MaintenanceRepo(DapperOptions options) : base(options)
        {
        }
        #region 已弃用
        #region MaintenanceItem
        public async Task<int> SaveMItem(MaintenanceItem maintenanceItem)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into maintenance_item " +
                        " values (0,@ItemName,@ItemType); ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, maintenanceItem);
                return newid;
            });
        }
        #endregion

        #region MaintenanceModuleItem
        public async Task<int> SaveMMoudleItem(List<MaintenanceModuleItem> maintenanceModuleItem)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into maintenance_module_item " +
                        " values (0,@Module,@Item); ";
                sql += "SELECT LAST_INSERT_ID() ";
                return await c.ExecuteAsync(sql, maintenanceModuleItem);
            });
        }
        #endregion

        #region MaintenanceModuleItemValue
        public async Task<int> SaveMMoudleItemValue(List<MaintenanceModuleItemValue> maintenanceModuleItemValues)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into maintenance_module_item_value " +
                        " values (0,@List,@Module,@Eqp,@Item,@ItemValue); ";
                sql += "SELECT LAST_INSERT_ID() ";
                int ret = await c.ExecuteAsync(sql, maintenanceModuleItemValues);
                return ret;
            });
        }
        /// <summary>
        /// 根据list删除数据，和批量插入一起使用，由于还要更新状态，所以直接在sevice事务，而不是dal
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> DelMMoudleItemValue(int list)
        {
            return await WithConnection(async c =>
            {
                string sql = " delete FROM maintenance_module_item_value where list=@list ";
                int ret = await c.ExecuteAsync(sql, new { list});
                return ret;
            });
        }
        #endregion

        #region MaintenanceModule
        public async Task<int> SaveMModule(MaintenanceModule maintenanceModule)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into maintenance_module " +
                        " values (0,@Name,@Type,@PlanCode); ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, maintenanceModule);
                return newid;
            });
        }
        #endregion

        #region MaintenanceList
        public async Task<int> SaveMList(MaintenanceList maintenanceList)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into maintenance_list " +
                        " values (0,@Title,@Team,@PlanDate,@Location,@LocationBy,@Status,@Remark," +
                        " @CreatedBy,@CreatedTime,@UpdatedBy,@UpdatedTime); ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, maintenanceList);
                return newid;
            });
        }

        public async Task<int> UpdateMList(int status,int user,int id)
        {
            return await WithConnection(async c =>
            {
                string sql = " update maintenance_list " +
                        " set status=@status,updated_by=@user,updated_time=@time where id=@id ";
                int newid = await c.ExecuteAsync(sql, new {status,user,time=DateTime.Now,id });
                return newid;
            });
        }

        public async Task<MaintenanceListView> ListPage(MaintenanceListParm parm)
        {
            return await WithConnection(async c =>
            {
                MaintenanceListView ret = new MaintenanceListView();
                string sql = "SELECT ml.*,ot.name as team_name,d.name,u2.user_name,u1.user_name as cname " +
                " FROM maintenance_list ml " +
                " left join org_tree ot on ot.id=ml.team " +
                " left join dictionary_tree d on d.id=ml.status " +
                " left join user u1 on u1.id=ml.created_by " +
                " left join user u2 on u2.id=ml.updated_by " +
                " where 1=1 ";
                string sqlwhere="";
                if (!string.IsNullOrWhiteSpace(parm.Title))
                {
                    sqlwhere += " and ml.title like '%" + parm.Title+"%' ";
                }
                if (parm.Status !=null)
                {
                    sqlwhere += " and ml.status="+ parm.Status;
                }
                sql = sql + sqlwhere + " order by "+ parm.sort + " "+parm.order
                +" limit " + (parm.page - 1) * parm.rows + "," + parm.rows;
                var tmp = await c.QueryAsync<MaintenanceList>(sql);
                if (tmp.Count() > 0)
                {
                    sql = "select count(*) FROM maintenance_list ml " + sqlwhere;
                    ret.total = await c.QueryFirstOrDefaultAsync<int>(sql);
                    ret.rows = tmp.ToList();
                }
                else
                {
                    ret.rows = new List<MaintenanceList>();
                    ret.total = 0;
                }
                return ret;
            });
        }

        #endregion

        #region MaintenancePlanDetail
        public async Task<int> SaveMDetail(List<MaintenancePlanDetail> maintenancePlanDetail)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into maintenance_plan_detail " +
                        " values (0,@List,@Detail,@PlanCode,@Count,@PMType); ";
                return await c.ExecuteAsync(sql, maintenancePlanDetail);
            });
        }
        #endregion

        #region MaintenanceModuleItemAll,根据检修单获取所有父项、子项、值
        /// <summary>
        /// 新建时查询主表为item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MaintenanceModuleItemAll>> ListItems(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT DISTINCT mm.id,mm.name,mmi.item,mi.item_name,mi.item_type, " +
                " mpd.count from maintenance_module_item mmi " +
                " left join maintenance_module mm on mm.id=mmi.module " +
                " LEFT JOIN maintenance_item mi on mi.id=mmi.item " +
                " right JOIN maintenance_plan_detail mpd on mpd.plan_code=mm.plan_code " +
                " where mpd.list=@id ";
                var tmp = await c.QueryAsync<MaintenanceModuleItemAll>(sql,new { id});
                if (tmp.Count() > 0)
                {
                    return tmp.ToList();
                }
                else
                {
                    return new List<MaintenanceModuleItemAll>();
                }
            });
        }

        /// <summary>
        /// 更新时查询主表为value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MaintenanceModuleItemAll>> ListValues(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT DISTINCT mm.id,mm.name,mmiv.item,mi.item_name,mi.item_type, " +
                " mpd.count,mmiv.eqp,mmiv.item_value,ml.updated_time,u.user_name " +
                " from maintenance_module_item_value mmiv " +
                " left join maintenance_module mm on mm.id=mmiv.module " +
                " LEFT JOIN maintenance_item mi on mi.id=mmiv.item " +
                " right JOIN maintenance_plan_detail mpd on mpd.plan_code=mm.plan_code " +
                " left join maintenance_list ml on ml.id=mmiv.list " +
                " left join user u on u.id=ml.updated_by " +
                " where mmiv.list=@id ";
                var tmp = await c.QueryAsync<MaintenanceModuleItemAll>(sql, new { id });
                if (tmp.Count() > 0)
                {
                    return tmp.ToList();
                }
                else
                {
                    return new List<MaintenanceModuleItemAll>();
                }
            });
        }

        #endregion
        #endregion

        #region PMModule
        public async Task<int> SavePMModule(PMModule pmModule)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into pm_module " +
                        " values (0,@Code,@Name,@FileName,@FilePath,@Major,@Line,@Location,@LocationBy," +
                        " @LocationPath,@DeviceName,@KeyWord,@Department,@DeathYear,@Level,@CreatedBy,@CreatedTime); ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, pmModule);
                return newid;
            });
        }

        public async Task<object> ListModulePage(PMModuleParm parm)
        {
            return await WithConnection(async c =>
            {
                object ret= new { total = 0, row = new List<PMModule>() };
                string sql = "SELECT m.*,d.name as levelname,u1.user_name " +
                " FROM pm_module m " +
                " left join dictionary_tree d on d.id=m.level " +
                " left join user u1 on u1.id=m.created_by " +
                " where 1=1 ";
                string sqlwhere = "";
                if (!string.IsNullOrWhiteSpace(parm.ModuleName))
                {
                    sqlwhere += " and m.name like '%" + parm.ModuleName + "%' ";
                }
                if (!string.IsNullOrWhiteSpace(parm.FileName))
                {
                    sqlwhere += " and m.file_name like '%" + parm.FileName + "%' ";
                }
                if (!string.IsNullOrWhiteSpace(parm.DeviceName))
                {
                    sqlwhere += " and m.device_name like '%" + parm.DeviceName + "%' ";
                }
                if (!string.IsNullOrWhiteSpace(parm.KeyWord))
                {
                    sqlwhere += " and m.key_word like '%" + parm.KeyWord + "%' ";
                }
                sql = sql + sqlwhere + " order by " + parm.sort + " " + parm.order
                + " limit " + (parm.page - 1) * parm.rows + "," + parm.rows;
                var tmp = await c.QueryAsync<PMModule>(sql);
                if (tmp.Count() > 0)
                {
                    sql = "select count(*) FROM pm_module m " + sqlwhere;
                    ret= new { total = await c.QueryFirstOrDefaultAsync<int>(sql), rows = tmp.ToList() };
                }
                return ret;
            });
        }

        public async Task<PMModule> GetModuleByID(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT m.*,d.name as levelname,u1.user_name,ml.line_name, " +
                " dt.name as mname FROM pm_module m " +
                " left join metro_line ml on ml.id=m.line " +
                " left join dictionary_tree d on d.id=m.level " +
                " left join dictionary_tree dt on dt.id=m.major " +
                " left join user u1 on u1.id=m.created_by " +
                " where m.id=@id ";
                return await c.QueryFirstOrDefaultAsync<PMModule>(sql,new { id});
            });
        }
        public async Task<PMModule> GetModuleByIDEasy(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM pm_module m where m.id=@id ";
                return await c.QueryFirstOrDefaultAsync<PMModule>(sql, new { id });
            });
        }

        #endregion

        #region PMEntity
        public async Task<int> SavePMEntity(PMEntity pmEntity)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into pm_entity " +
                        " values (0,@Title,@Team,@PlanDate,@Location,@LocationBy,@Eqp," +
                        " @Status,@Remark,@Module,@FilePath,@CreatedBy,@CreatedTime,@UpdatedBy,@UpdatedTime); ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, pmEntity);
                return newid;
            });
        }
        public async Task<int> UpdatePMEntity(PMEntity pmEntity)
        {
            return await WithConnection(async c =>
            {
                string sql = " update pm_entity " +
                        " set title=@Title,team=@Team,plan_date=@PlanDate,location=@Location,location_by=@LocationBy," +
                        " eqp=@Eqp,status=@Status,remark=@Remark,module=@Module,file_path=@FilePath," +
                        " updated_by=@UpdatedBy,updated_time=@UpdatedTime where id=@id ";
                return await c.ExecuteAsync(sql, pmEntity);
            });
        }
        public async Task<int> UpdatePMEntityStatus(int id,int status,int userID)
        {
            return await WithConnection(async c =>
            {
                string sql = " update pm_entity " +
                        " set status=@status,updated_by=@userID,updated_time=@UpdatedTime where id=@id ";
                return await c.ExecuteAsync(sql, new { id, status, userID, UpdatedTime=DateTime.Now });
            });
        }

        public async Task<PMEntityView> ListEntityPage(PMEntityParm parm)
        {
            return await WithConnection(async c =>
            {
                PMEntityView ret = new PMEntityView();
                ret.total = 0;
                ret.rows = new List<PMEntity>();
                string sql = "SELECT m.*,d.name,u1.user_name,o.name as tname,e.eqp_name " +
                " FROM pm_entity m " +
                " left join dictionary_tree d on d.id=m.status " +
                " left join equipment e on e.id=m.eqp " +
                " left join org_tree o on o.id=m.team " +
                " left join user u1 on u1.id=m.updated_by " +
                " where 1=1 ";
                string sqlwhere = "";
                if (!string.IsNullOrWhiteSpace(parm.Title))
                {
                    sqlwhere += " and m.title like '%" + parm.Title + "%' ";
                }
                if (parm.Status!=null)
                {
                    sqlwhere += " and m.status = " + parm.Status;
                }
                if (parm.Start != null)
                {
                    sqlwhere += " and m.updated_time >= '" + parm.Start+"' ";
                }
                if (parm.End != null)
                {
                    sqlwhere += " and m.updated_time <= '" + parm.End + "' ";
                }
                sql = sql + sqlwhere + " order by " + parm.sort + " " + parm.order
                + " limit " + (parm.page - 1) * parm.rows + "," + parm.rows;
                var tmp = await c.QueryAsync<PMEntity>(sql);
                if (tmp.Count() > 0)
                {
                    sql = "select count(*) FROM pm_Entity m where 1=1 " + sqlwhere;
                    ret.total = await c.QueryFirstOrDefaultAsync<int>(sql);
                    ret.rows = tmp.ToList();
                }
                return ret;
            });
        }
        public async Task<int> DelPMEntity(string[] ids)
        {
            return await WithConnection(async c =>
            {
                string sql = " delete from pm_entity " +
                        " where id in @ids ";
                return await c.ExecuteAsync(sql, new { ids });
            });
        }

        public async Task<PMEntity> GetEntityByID(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM pm_entity where id=@id ";
                return await c.QueryFirstOrDefaultAsync<PMEntity>(sql, new { id });
            });
        }
        public async Task<List<PMEntity>> GetEntityByIDs(string[] ids)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM pm_entity where id in @ids ";
                return (await c.QueryAsync<PMEntity>(sql, new { ids })).ToList();
            });
        }
        public async Task<PMEntity> GetEntityDetailByID(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT m.*,d.name,u1.user_name,o.name as tname " +
                " FROM pm_entity m " +
                " left join dictionary_tree d on d.id=m.status " +
                " left join org_tree o on o.id=m.team " +
                " left join user u1 on u1.id=m.updated_by " +
                " where m.id=@id ";
                return await c.QueryFirstOrDefaultAsync<PMEntity>(sql, new { id });
            });
        }

        #endregion

        #region PMEntityMonthDetail
        public async Task<int> SavePMEntityMonthDetail(List<PMEntityMonthDetail> pmEntityMonthDetails)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into pm_entity_month_detail " +
                        " values (0,@PMEntity,@MonthDetail); ";
                return await c.ExecuteAsync(sql, pmEntityMonthDetails);
            });
        }
        public async Task<int> DelPMEntityMonthDetail(string[] ids)
        {
            return await WithConnection(async c =>
            {
                string sql = " delete from pm_entity_month_detail " +
                        " where pm_entity in @ids ";
                return await c.ExecuteAsync(sql,new { ids });
            });
        }
        public async Task<List<int>> ListMonthDetail(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = " select month_detail from pm_entity_month_detail " +
                        " where pm_entity=@id ";
                var tmp = await c.QueryAsync<int>(sql, new { id });
                return tmp.ToList();
            });
        }
        #endregion

        #region EqpHistory
        public async Task<int> SaveEqpHistory(EqpHistory eqp)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into equipment_history " +
                        " values (0,@EqpID,@Type,@WorkingOrder,@ShowName,@CreatedTime,@CreatedBy); ";
                return await c.ExecuteAsync(sql, eqp);
            });
        }
        #endregion

    }
}



