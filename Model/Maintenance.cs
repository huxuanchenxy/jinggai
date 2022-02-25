
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Data;

// Coded by admin 2019/9/26 16:46:46
namespace SZY.Platform.WebApi.Model
{
    #region 已弃用，目前直接导入excel
    #region MaintenanceItem
    public class MaintenanceItem
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public int ItemType { get; set; }
    }
    public class MaintenanceItemMap : EntityMap<MaintenanceItem>
    {
        public MaintenanceItemMap()
        {
            Map(o => o.ItemName).ToColumn("item_name");
            Map(o => o.ItemType).ToColumn("item_type");
        }

    }
    #endregion

    #region MaintenanceModuleItem
    public class MaintenanceModuleItem
    {
        public int ID { get; set; }
        public int Module { get; set; }
        public int Item { get; set; }
    }
    public class MaintenanceModuleItemMap : EntityMap<MaintenanceModuleItem>
    {
        public MaintenanceModuleItemMap()
        {
            Map(o => o.Module).ToColumn("module");
        }
    }
    #endregion

    #region MaintenanceModuleItemValue
    public class MaintenanceModuleItemValueParm
    {
        public List<MaintenanceModuleItemValue> MaintenanceModuleItemValues { get; set; }
        public bool IsFinished { get; set; }

    }
    public class MaintenanceModuleItemValue
    {
        public int ID { get; set; }
        public int List { get; set; }
        public int Module { get; set; }
        public int Item { get; set; }
        public string Eqp { get; set; }
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public int ItemType { get; set; }
    }
    public class MaintenanceModuleItemValueMap : EntityMap<MaintenanceModuleItemValue>
    {
        public MaintenanceModuleItemValueMap()
        {
            Map(o => o.ItemValue).ToColumn("item_value");
            Map(o => o.ItemType).ToColumn("item_type");
            Map(o => o.ItemName).ToColumn("item_name");
        }
    }
    #endregion

    #region MaintenanceModule
    public class MaintenanceModule
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShowName { get; set; }
        public int Type { get; set; }
        public string PlanCode { get; set; }
        public bool IsShowEqp { get; set; }
        /// <summary>
        /// 当检查设施数量大于1的时候，ModuleItem和Eqp才能确定唯一子项的值
        /// </summary>
        public string Eqp { get; set; }

        public List<MaintenanceModuleItemValue> Items { get; set; }
    }
    public class MaintenanceModuleMap : EntityMap<MaintenanceModule>
    {
        public MaintenanceModuleMap()
        {
            Map(o => o.PlanCode).ToColumn("plan_code");
        }
    }
    #endregion

    #region MaintenanceList
    public class MaintenanceListParm: BaseQueryParm
    {
        public int? Status { get; set; }
        public string Title { get; set; }
    }
    public class MaintenanceListView
    {
        public List<MaintenanceList> rows{ get; set; }
        public int total { get; set; }
    }
    public class MaintenanceList
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Team { get; set; }
        public string TeamName { get; set; }
        public string PlanDate { get; set; }
        public int Location { get; set; }
        public string LocationName { get; set; }
        public int Locationby { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Remark { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime UpdatedTime { get; set; }
        public List<MaintenancePlanDetail> Details { get; set; }
    }
    public class MaintenanceListMap : EntityMap<MaintenanceList>
    {
        public MaintenanceListMap()
        {
            Map(o => o.TeamName).ToColumn("team_name");
            Map(o => o.StatusName).ToColumn("name");
            Map(o => o.PlanDate).ToColumn("plan_date");
            Map(o => o.Locationby).ToColumn("location_by");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedByName).ToColumn("cname");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedByName).ToColumn("user_name");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
        }
    }
    #endregion

    #region MaintenancePlanDetail
    public class MaintenancePlanDetail
    {
        public int ID { get; set; }
        public int List { get; set; }
        public int Detail { get; set; }
        public string PlanCode { get; set; }
        public int Count { get; set; }
        public int PMType { get; set; }
    }
    public class MaintenancePlanDetailMap : EntityMap<MaintenancePlanDetail>
    {
        public MaintenancePlanDetailMap()
        {
            Map(o => o.PMType).ToColumn("pm_type");
            Map(o => o.PlanCode).ToColumn("plan_code");
        }
    }
    #endregion

    #region MaintenanceModuleItemAll,根据检修单获取所有父项及其子项
    public class MaintenanceModuleItemAll
    {
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public int Item { get; set; }
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public string Eqp { get; set; }
        public int ItemType { get; set; }
        public int Count { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
    public class MaintenanceModuleItemAllMap : EntityMap<MaintenanceModuleItemAll>
    {
        public MaintenanceModuleItemAllMap()
        {
            Map(o => o.ItemName).ToColumn("item_name");
            Map(o => o.ItemType).ToColumn("item_type");
            Map(o => o.ItemValue).ToColumn("item_value");
            Map(o => o.ModuleName).ToColumn("name");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedByName).ToColumn("user_name");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
        }
    }
    #endregion
    #endregion

    #region 检修单模板 PMModule
    public class PMModuleParm : BaseQueryParm
    {
        public string ModuleName { get; set; }
        public string FileName { get; set; }
        public string DeviceName { get; set; }
        public string KeyWord { get; set; }
    }
    public class PMModule
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int? Major { get; set; }
        public string MajorName { get; set; }
        public int? Line { get; set; }
        public string LineName { get; set; }
        public int? Location { get; set; }
        public string LocationName { get; set; }
        public int? LocationBy { get; set; }
        public string LocationPath { get; set; }
        public string DeviceName { get; set; }
        public string KeyWord { get; set; }
        public string Department { get; set; }
        public int? DeathYear { get; set; }
        public int? Level { get; set; }
        public string LevelName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedTime { get; set; }
    }
    public class PMModuleMap : EntityMap<PMModule>
    {
        public PMModuleMap()
        {
            Map(o => o.FileName).ToColumn("file_name");
            Map(o => o.FilePath).ToColumn("file_path");
            Map(o => o.MajorName).ToColumn("mname");
            Map(o => o.LineName).ToColumn("line_name");
            Map(o => o.LocationBy).ToColumn("location_by");
            Map(o => o.LocationName).ToColumn("lname");
            Map(o => o.DeviceName).ToColumn("device_name");
            Map(o => o.KeyWord).ToColumn("key_word");
            Map(o => o.DeathYear).ToColumn("death_year");
            Map(o => o.LevelName).ToColumn("levelname");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedByName).ToColumn("user_name");
            Map(o => o.CreatedTime).ToColumn("created_time");
        }

    }
    #endregion

    #region 检修单实例 PMEntity
    public class PMEntityParm : BaseQueryParm
    {
        public int? Status { get; set; }
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
    public class PMEntityView
    {
        public List<PMEntity> rows { get; set; }
        public int total { get; set; }
    }
    public class PMEntity
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Team { get; set; }
        public string TeamName { get; set; }
        public string PlanDate { get; set; }
        public int Location { get; set; }
        public string LocationName { get; set; }
        public int Locationby { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int? Eqp { get; set; }
        public string EqpName { get; set; }
        public string Remark { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int Module { get; set; }
        public string FilePath { get; set; }
        public List<int> PMMonthDetails { get; set; }
        public bool IsFinished { get; set; }
        public bool IsPlanChanged { get; set; }
        public List<List<string>> contents { get; set; }
    }
    public class PMEntityMap : EntityMap<PMEntity>
    {
        public PMEntityMap()
        {
            Map(o => o.TeamName).ToColumn("tname");
            Map(o => o.StatusName).ToColumn("name");
            Map(o => o.EqpName).ToColumn("eqp_name");
            Map(o => o.PlanDate).ToColumn("plan_date");
            Map(o => o.Locationby).ToColumn("location_by");
            Map(o => o.FilePath).ToColumn("file_path");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedByName).ToColumn("cname");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedByName).ToColumn("user_name");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
        }
    }
    #endregion

    #region 检修单和月计划的关联关系 PMEntityMonthDetail
    public class PMEntityMonthDetail
    {
        public int ID { get; set; }
        public int PMEntity { get; set; }
        public int MonthDetail { get; set; }
    }
    public class PMEntityMonthDetailMap : EntityMap<PMEntityMonthDetail>
    {
        public PMEntityMonthDetailMap()
        {
            Map(o => o.PMEntity).ToColumn("pm_entity");
            Map(o => o.MonthDetail).ToColumn("month_detail");
        }
    }
    #endregion

    #region 设备历史记录EqpHistory
    public class EqpHistory
    {
        public int ID { get; set; }
        public int EqpID { get; set; }
        public int Type { get; set; }
        public int WorkingOrder { get; set; }
        public string ShowName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CreatedBy { get; set; }
    }

    /// <summary>
    /// model
    /// </summary>
    public class EqpHistoryMap : EntityMap<EqpHistory>
    {
        public EqpHistoryMap()
        {
            Map(o => o.ID).ToColumn("id");
            Map(o => o.EqpID).ToColumn("eqp");
            Map(o => o.Type).ToColumn("type");
            Map(o => o.WorkingOrder).ToColumn("working_order");
            Map(o => o.ShowName).ToColumn("show_name");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedTime).ToColumn("created_time");
        }
    }
    #endregion
}