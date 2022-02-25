
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System.Collections.Generic;

// Coded by admin 2019/10/21 11:18:41
namespace SZY.Platform.WebApi.Model
{
    public class ConstructionPlanParm : BaseQueryParm
    {
        public string planName { get; set; }
        public int userId { get; set; }
        public string processGUID { get; set; }
        public int conplantype { get; set; }
    }
    public class ConstructionPlanPageView
    {
        public List<ConstructionPlan> rows { get; set; }
        public int total { get; set; }
    }

    public class ConstructionPlan : BaseEntity
    {
        public long Id { get; set; }
        public int LineId { get; set; }
        public int AreaId { get; set; }
        public string AreaTypename { get; set; }
        public string PlanName { get; set; }
        public sbyte PlanType { get; set; }
        public string PlanNumber { get; set; }
        public sbyte ImportantLevel { get; set; }
        public sbyte PlanLevel { get; set; }
        public int ApplyCompanyOrgId { get; set; }
        public int ConstructionCompanyOrgId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string RegisterStationId { get; set; }
        public string DeviceNum { get; set; }
        public string TroubleNum { get; set; }
        public string OperationAddress { get; set; }
        public string ConstructionContent { get; set; }
        public string ConstructionDetail { get; set; }
        public string CoordinationRequest { get; set; }
        public string CoordinationAudit { get; set; }
        public sbyte StopElectric { get; set; }
        public sbyte TractionPower { get; set; }
        public int TractionTrainNum { get; set; }
        public System.DateTime TractionStartTime { get; set; }
        public int TractionTime { get; set; }
        public sbyte UseLaddercar { get; set; }
        public string ElectricRange { get; set; }
        public string TractionCarRoute { get; set; }
        public string EffectArea { get; set; }
        public string EffectExplain { get; set; }
        public string SafeMeasure { get; set; }
        public string ForceUnlockKey { get; set; }
        public string OtherRequest { get; set; }
        public string Memo { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime UpdatedTime { get; set; }
        public int UpdatedBy { get; set; }
        public sbyte IsDel { get; set; }
        public string FileIDs { get; set; }
        public int ProcessState { get; set; }
        public sbyte ConPlanType { get; set; }
    }

    public class ConstructionPlanMap : EntityMap<ConstructionPlan>
    {
        public ConstructionPlanMap()
        {
            Map(o => o.Id).ToColumn("id");
            Map(o => o.LineId).ToColumn("line_id");
            Map(o => o.AreaId).ToColumn("area_id");
            Map(o => o.AreaTypename).ToColumn("area_typename");
            Map(o => o.PlanName).ToColumn("plan_name");
            Map(o => o.PlanType).ToColumn("plan_type");
            Map(o => o.PlanNumber).ToColumn("plan_number");
            Map(o => o.ImportantLevel).ToColumn("important_level");
            Map(o => o.PlanLevel).ToColumn("plan_level");
            Map(o => o.ApplyCompanyOrgId).ToColumn("apply_company_org_id");
            Map(o => o.ConstructionCompanyOrgId).ToColumn("construction_company_org_id");
            Map(o => o.StartTime).ToColumn("start_time");
            Map(o => o.EndTime).ToColumn("end_time");
            Map(o => o.RegisterStationId).ToColumn("register_station_id");
            Map(o => o.DeviceNum).ToColumn("device_num");
            Map(o => o.TroubleNum).ToColumn("trouble_num");
            Map(o => o.OperationAddress).ToColumn("operation_address");
            Map(o => o.ConstructionContent).ToColumn("construction_content");
            Map(o => o.ConstructionDetail).ToColumn("construction_detail");
            Map(o => o.CoordinationRequest).ToColumn("coordination_request");
            Map(o => o.CoordinationAudit).ToColumn("coordination_audit");
            Map(o => o.StopElectric).ToColumn("stop_electric");
            Map(o => o.TractionPower).ToColumn("traction_power");
            Map(o => o.TractionTrainNum).ToColumn("traction_train_num");
            Map(o => o.TractionStartTime).ToColumn("traction_start_time");
            Map(o => o.TractionTime).ToColumn("traction_time");
            Map(o => o.UseLaddercar).ToColumn("use_laddercar");
            Map(o => o.ElectricRange).ToColumn("electric_range");
            Map(o => o.TractionCarRoute).ToColumn("traction_car_route");
            Map(o => o.EffectArea).ToColumn("effect_area");
            Map(o => o.EffectExplain).ToColumn("effect_explain");
            Map(o => o.SafeMeasure).ToColumn("safe_measure");
            Map(o => o.ForceUnlockKey).ToColumn("force_unlock_key");
            Map(o => o.OtherRequest).ToColumn("other_request");
            Map(o => o.Memo).ToColumn("memo");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.IsDel).ToColumn("is_del");
            Map(o => o.ProcessState).ToColumn("processstate");
            Map(o => o.ConPlanType).ToColumn("con_plan_type");
        }
    }

}