
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Data;

// Coded by admin 2019/9/26 16:46:46
namespace SZY.Platform.WebApi.Model
{
    public class ConstructionPlanMonthDetailParm : BaseQueryParm
    {
        public int? Query { get; set; }
        public int Month { get; set; }
        public int? Team { get; set; }
        public int? Location { get; set; }
        public int? LocationBy { get; set; }
        public string PlanDate { get; set; }
        public int? Year { get; set; }
        public int? Line { get; set; }
        public int? Company { get; set; }
        public int? Department { get; set; }
        public int? IsAssigned { get; set; }
    }
    public class ConstructionPlanMonthDetail
    {
        public int ID { get; set; }
        public int Month { get; set; }
        public int WorkType { get; set; }
        public string WorkTypeName { get; set; }
        public int Department { get; set; }
        public string DepartmentName { get; set; }
        public int Line { get; set; }
        public string LineName { get; set; }
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// plan_code
        /// </summary>
        public string EqpType { get; set; }
        public string EqpTypeName { get; set; }
        public int Location { get; set; }
        public int LocationBy { get; set; }
        public string LocationName { get; set; }
        public int Team { get; set; }
        public string TeamName { get; set; }
        public int PMType { get; set; }
        public string PMTypeName { get; set; }
        public string PMCycle { get; set; }
        public int PMFrequency { get; set; }
        public string Unit { get; set; }
        public int PlanQuantity { get; set; }
        public string PlanDate { get; set; }
        public int? RealQuantity { get; set; }
        public string RealDate { get; set; }
        public string WorkingOrder { get; set; }
        public int OrderStatus { get; set; }
        public string Remark { get; set; }
        public int Query { get; set; }
        public DateTime UpdateTime { get; set; }
        public int UpdateBy { get; set; }
        public string UpdateName { get; set; }
        public int IsAssigned { get; set; }
    }
    public class ConstructionPlanMonthDetailMap : EntityMap<ConstructionPlanMonthDetail>
    {
        public ConstructionPlanMonthDetailMap()
        {
            Map(o => o.WorkType).ToColumn("work_type");
            Map(o => o.WorkTypeName).ToColumn("name");
            Map(o => o.DepartmentName).ToColumn("department_name");
            Map(o => o.LineName).ToColumn("line_name");
            Map(o => o.EqpType).ToColumn("plan_code");
            Map(o => o.EqpTypeName).ToColumn("plan_module_name");
            Map(o => o.LocationBy).ToColumn("location_by");
            Map(o => o.LocationName).ToColumn("location_name");
            Map(o => o.TeamName).ToColumn("team_name");
            Map(o => o.PMType).ToColumn("pm_type");
            Map(o => o.PMTypeName).ToColumn("pm_type_name");
            Map(o => o.PMFrequency).ToColumn("pm_frequency");
            Map(o => o.PMCycle).ToColumn("pm_cycle");
            Map(o => o.CreatedTime).ToColumn("created_time");

            Map(o => o.PlanQuantity).ToColumn("plan_quantity");
            Map(o => o.PlanDate).ToColumn("plan_date");
            Map(o => o.RealQuantity).ToColumn("real_quantity");
            Map(o => o.RealDate).ToColumn("real_date");
            Map(o => o.WorkingOrder).ToColumn("working_order");
            Map(o => o.OrderStatus).ToColumn("order_status");

            Map(o => o.UpdateTime).ToColumn("update_time");
            Map(o => o.UpdateBy).ToColumn("update_by");
            Map(o => o.UpdateName).ToColumn("user_name");
            Map(o => o.IsAssigned).ToColumn("is_assigned");
        }
    }

    public class ConstructionPlanMonthDetails
    {
        public List<ConstructionPlanMonthDetail> rows { get; set; }
        public int total { get; set; }
    }

}