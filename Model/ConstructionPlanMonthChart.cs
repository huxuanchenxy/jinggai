
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System.Collections.Generic;

// Coded by admin 2019/11/29 10:39:28
namespace SZY.Platform.WebApi.Model
{
    public class ConstructionPlanMonthChartParm : BaseQueryParm
    {
        public int year { get; set; }
        public int month { get; set; }
        public int xAxisType { get; set; }//x轴 按日1,月2，年3统计
        public string startTime { get; set; }
        public string endTime { get; set; }
        public int startMonth { get; set; }
        public int endMonth { get; set; }
        public int team { get; set; }
        public int startYear { get; set; }
        public int endYear { get; set; }
    }
    public class ConstructionPlanMonthChartPageView
    {
        public List<ConstructionPlanMonthChart> rows { get; set; }
        public int total { get; set; }
    }

    public class ConstructionPlanMonthChartRet
    {
        public List<string> Dimension { get; set; }
        public List<ConstructionPlanMonthChartSeries> Series { get; set; }
        public List<string> Legend { get; set; }
    }

    public class ConstructionPlanMonthChartSeries
    {
        public string Name { get; set; }
        public List<int> Data { get; set; }
        public string Type { get; set; }
    }

    

    public class ConstructionPlanMonthChart : BaseEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Line { get; set; }
        public int WorkType { get; set; }
        public int EqpType { get; set; }
        public int Location { get; set; }
        public int LocationBy { get; set; }
        public int Department { get; set; }
        public int Team { get; set; }
        public int PmType { get; set; }
        public int PmFrequency { get; set; }
        public string Unit { get; set; }
        public int PlanQuantity { get; set; }
        public string PlanDate { get; set; }
        public int RealQuantity { get; set; }
        public string RealDate { get; set; }
        public string WorkingOrder { get; set; }
        public int OrderStatus { get; set; }
        public string Remark { get; set; }
        public int Query { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public int UpdateBy { get; set; }
        public int status { get; set; }
        public string donetime { get; set; }
    }

    public class ConstructionPlanMonthChartMap : EntityMap<ConstructionPlanMonthChart>
    {
        public ConstructionPlanMonthChartMap()
        {
            Map(o => o.Id).ToColumn("id");
            Map(o => o.Year).ToColumn("year");
            Map(o => o.Month).ToColumn("month");
            Map(o => o.Day).ToColumn("day");
            Map(o => o.Line).ToColumn("line");
            Map(o => o.WorkType).ToColumn("work_type");
            Map(o => o.EqpType).ToColumn("eqp_type");
            Map(o => o.Location).ToColumn("location");
            Map(o => o.LocationBy).ToColumn("location_by");
            Map(o => o.Department).ToColumn("department");
            Map(o => o.Team).ToColumn("team");
            Map(o => o.PmType).ToColumn("pm_type");
            Map(o => o.PmFrequency).ToColumn("pm_frequency");
            Map(o => o.Unit).ToColumn("unit");
            Map(o => o.PlanQuantity).ToColumn("plan_quantity");
            Map(o => o.PlanDate).ToColumn("plan_date");
            Map(o => o.RealQuantity).ToColumn("real_quantity");
            Map(o => o.RealDate).ToColumn("real_date");
            Map(o => o.WorkingOrder).ToColumn("working_order");
            Map(o => o.OrderStatus).ToColumn("order_status");
            Map(o => o.Remark).ToColumn("remark");
            Map(o => o.Query).ToColumn("query");
            Map(o => o.UpdateTime).ToColumn("update_time");
            Map(o => o.UpdateBy).ToColumn("update_by");
        }
    }

}