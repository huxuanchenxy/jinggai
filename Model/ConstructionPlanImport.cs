
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Data;

// Coded by admin 2019/9/26 16:46:46
namespace SZY.Platform.WebApi.Model
{
    public class ConstructionPlanImportParm : BaseQueryParm
    {
        public int SearchCompany { get; set; }
        public int SearchYear { get; set; }
        public bool IsYear { get; set; }
    }
    public class ConstructionPlanCommonParm : BaseQueryParm
    {
        public int? Year { get; set; }
        public int? Line { get; set; }
        public int? Company { get; set; }
        public int? Department { get; set; }
    }
    public class ConstructionPlanYearPageView
    {
        public List<ConstructionPlanYear> rows { get; set; }
        public int total { get; set; }
    }

    public class ConstructionPlanImportCommon
    {
        public int ID { get; set; }
        public int Year { get; set; }
        public int Department { get; set; }
        public string DepartmentName { get; set; }
        public int Line { get; set; }
        public string LineName { get; set; }
        public int Company { get; set; }
        public string CompanyName { get; set; }
        public int IsCreatedMonth { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public DateTime ImportedTime { get; set; }
        public int ImportedBy { get; set; }
        public string ImportedName { get; set; }
    }
    public class ConstructionPlanImportCommonMap : EntityMap<ConstructionPlanImportCommon>
    {
        public ConstructionPlanImportCommonMap()
        {
            Map(o => o.DepartmentName).ToColumn("department_name");
            Map(o => o.LineName).ToColumn("line_name");
            Map(o => o.CompanyName).ToColumn("name");
            Map(o => o.IsCreatedMonth).ToColumn("is_created_month");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedName).ToColumn("user_name");
            Map(o => o.ImportedTime).ToColumn("imported_time");
            Map(o => o.ImportedBy).ToColumn("imported_by");
            Map(o => o.ImportedName).ToColumn("iname");
        }
    }

    public class ConstructionPlanYear
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int Year { get; set; }
        public int Department { get; set; }
        public string DepartmentName { get; set; }
        public int Line { get; set; }
        public string LineName { get; set; }
        public int Company { get; set; }
        public int CompanyName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int EqpType { get; set; }
        public string EqpTypeName { get; set; }
        public int Location { get; set; }
        public int LocationBy { get; set; }
        public string LocationName { get; set; }
        public int Team { get; set; }
        public string TeamName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public string Cycle { get; set; }
        public int Frequency { get; set; }
        public int Once { get; set; }
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int August { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }
        public int Query { get; set; }
    }

    public class ConstructionPlanYearMap : EntityMap<ConstructionPlanYear>
    {
        public ConstructionPlanYearMap()
        {
            Map(o => o.DepartmentName).ToColumn("department_name");
            Map(o => o.LineName).ToColumn("line_name");
            Map(o => o.EqpType).ToColumn("eqp_type");
            Map(o => o.EqpTypeName).ToColumn("eqp_type_name");
            Map(o => o.LocationBy).ToColumn("location_by");
            Map(o => o.LocationName).ToColumn("location_name");
            Map(o => o.TeamName).ToColumn("team_name");
            Map(o => o.CompanyName).ToColumn("name");
            Map(o => o.CreatedTime).ToColumn("created_time");
        }
    }

    public class ConstructionPlanMonth
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int Year { get; set; }
        public int Department { get; set; }
        public string DepartmentName { get; set; }
        public int Line { get; set; }
        public string LineName { get; set; }
        public int Company { get; set; }
        public int CompanyName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int EqpType { get; set; }
        public string EqpTypeName { get; set; }
        public int Location { get; set; }
        public int LocationBy { get; set; }
        public string LocationName { get; set; }
        public int Team { get; set; }
        public string TeamName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public string Cycle { get; set; }
        public int Frequency { get; set; }
        public int Once { get; set; }
        public int One { get; set; }
        public int Two { get; set; }
        public int Three { get; set; }
        public int Four { get; set; }
        public int Five { get; set; }
        public int Six { get; set; }
        public int Seven { get; set; }
        public int Eight { get; set; }
        public int Nine { get; set; }
        public int Ten { get; set; }
        public int Eleven { get; set; }
        public int Twelve { get; set; }
        public int Thirteen { get; set; }
        public int Fourteen { get; set; }
        public int Fifteen { get; set; }
        public int Sixteen { get; set; }
        public int Seventeen { get; set; }
        public int Eighteen { get; set; }
        public int Nineteen { get; set; }
        public int Twenty { get; set; }
        public int TwentyOne { get; set; }
        public int TwentyTwo { get; set; }
        public int TwentyThree { get; set; }
        public int TwentyFour { get; set; }
        public int TwentyFive { get; set; }
        public int TwentySix { get; set; }
        public int TwentySeven { get; set; }
        public int TwentyEight { get; set; }
        public int TwentyNine { get; set; }
        public int Thirty { get; set; }
        public int ThirtyOne { get; set; }
        public int Query { get; set; }
    }

    public class ConstructionPlanMonthMap : EntityMap<ConstructionPlanMonth>
    {
        public ConstructionPlanMonthMap()
        {
            Map(o => o.DepartmentName).ToColumn("department_name");
            Map(o => o.LineName).ToColumn("line_name");
            Map(o => o.EqpType).ToColumn("eqp_type");
            Map(o => o.EqpTypeName).ToColumn("eqp_type_name");
            Map(o => o.LocationBy).ToColumn("location_by");
            Map(o => o.LocationName).ToColumn("location_name");
            Map(o => o.TeamName).ToColumn("team_name");
            Map(o => o.CompanyName).ToColumn("name");
            Map(o => o.CreatedTime).ToColumn("created_time");

            Map(o => o.TwentyOne).ToColumn("twenty_one");
            Map(o => o.TwentyTwo).ToColumn("twenty_two");
            Map(o => o.TwentyThree).ToColumn("twenty_three");
            Map(o => o.TwentyFour).ToColumn("twenty_four");
            Map(o => o.TwentyFive).ToColumn("twenty_five");
            Map(o => o.TwentySix).ToColumn("twenty_six");
            Map(o => o.TwentySeven).ToColumn("twenty_seven");
            Map(o => o.TwentyEight).ToColumn("twenty_eight");
            Map(o => o.TwentyNine).ToColumn("twenty_nine");
            Map(o => o.ThirtyOne).ToColumn("thirty_one");
        }
    }


    public class ConstructionPlanImport
    {
        public ConstructionPlanImportCommon importCommon { get; set; }
        public List<DataTable> yearPlans { get; set; }
        public List<DataTable> monthPlans { get; set; }
    }

    public class QueryItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int LocationBy { get; set; }
    }
}