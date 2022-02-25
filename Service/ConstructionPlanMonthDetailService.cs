using Microsoft.AspNetCore.Http;
using MSS.API.Common;
using MSS.API.Common.Utility;
using SZY.Platform.WebApi.Data;
using SZY.Platform.WebApi.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;


// Coded By admin 2019/9/26 17:41:26
namespace SZY.Platform.WebApi.Service
{
    public interface IConstructionPlanMonthDetailService
    {
        Task<ApiResult> Update(ConstructionPlanMonthDetail obj);
        Task<ApiResult> GetByID(int id);
        Task<ApiResult> ListPage(ConstructionPlanMonthDetailParm parm);
        Task<ApiResult> Create(int query);
    }

    public class ConstructionPlanMonthDetailService : IConstructionPlanMonthDetailService
    {
        private readonly IConstructionPlanMonthChartRepo<ConstructionPlanMonthChart> _chartRepo;
        private readonly IConstructionPlanMonthDetailRepo<ConstructionPlanMonthDetail> _repo;
        private readonly IConstructionPlanImportRepo<ConstructionPlanYear> _importRepo;
        private readonly IMaintenanceRepo<MaintenanceItem> _mRepo;
        private readonly IAuthHelper _authhelper;
        private readonly int _userID;

        public ConstructionPlanMonthDetailService(IConstructionPlanMonthDetailRepo<ConstructionPlanMonthDetail> repo,
            IConstructionPlanImportRepo<ConstructionPlanYear> importRepo,
            IMaintenanceRepo<MaintenanceItem> mRepo,
            IConstructionPlanMonthChartRepo<ConstructionPlanMonthChart> chartRepo,IAuthHelper authhelper)
        {
            _repo = repo;
            _mRepo = mRepo;
            _chartRepo = chartRepo;
            _importRepo = importRepo;
            _authhelper = authhelper;
            _userID = _authhelper.GetUserId();
        }
        public async Task<ApiResult> Update(ConstructionPlanMonthDetail obj)
        {
            ApiResult ret = new ApiResult();
            try
            {
                obj.UpdateBy = _userID;
                obj.UpdateTime = DateTime.Now;
                ret.data = await _repo.Update(obj);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> GetByID(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data =await _repo.GetByID(id);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> ListPage(ConstructionPlanMonthDetailParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ConstructionPlanMonthDetails c = await _repo.ListPage(parm);
                var locations=await _importRepo.ListAllLocations();
                foreach (var item in c.rows)
                {
                    item.LocationName = locations.Where(a => a.LocationBy == item.LocationBy && a.ID == item.Location)
                        .FirstOrDefault().Name;
                }
                ret.data=c;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> Create(int query)
        {
            ApiResult ret = new ApiResult();
            List<ConstructionPlanMonthDetail> month = new List<ConstructionPlanMonthDetail>();
            DataTable dt = GetColumnName(false);
            DataTable dtChart = GetColumnName(true);
            DateTime dtNow = DateTime.Now;
            // 每天工作时间统计，年表中的任务分配给时间做少的一天
            List<List<int>> dayMinInMonth = new List<List<int>>();
            for (int i = 0; i < 12; i++)
            {
                List<int> tmp = new List<int>();
                for (int j = 0; j < 31; j++)
                {
                    tmp.Add(0);
                }
                dayMinInMonth.Add(tmp);
            }
            try
            {
                List<ConstructionPlanMonth> cpms =await _importRepo.ListMonthByQuery(query);
                List<ConstructionPlanYear> cpys = await _importRepo.ListYearByQuery(query);
                ConstructionPlanImportCommon cpic = await _importRepo.GetByID(query);
                AllQuery all = new AllQuery();
                //all.eqpTypes = await _importRepo.ListAllEqpTypes();
                all.locations = await _importRepo.ListAllLocations();
                all.team = await _importRepo.ListAllOrgByType(OrgType.Team);
                all.workType = await _importRepo.ListDictionarysByParent(SZY.Platform.WebApi.Model.Common.WORK_TYPE);
                all.pmType =await _importRepo.ListDictionarysByParent(SZY.Platform.WebApi.Model.Common.PM_TYPE);
                int year = cpic.Year;
                int index = 0;
                // 12月份循环
                for (int i = 1; i < 13; i++)
                {
                    int j = 0;
                    // 处理月表
                    foreach (var item in cpms)
                    {
                        string m = "." + i.ToString("D2");
                        if (item.Frequency% 31 == 0)
                        {
                            string d = "." + SZY.Platform.WebApi.Model.Common.GetLastDay(i, year);
                            string planDate= year + m + ".01-" + year + m + d;
                            ConstructionPlanMonthDetail c = GetCommonProperty(item,i, cpic,all,dtNow, planDate, ref dt,ref dtChart);
                            //dt.Rows[j+index][14] = c.PlanDate;
                            month.Add(c);
                        }
                        else
                        {
                            List<int> day = dayMinInMonth[i-1];
                            List<int> allDate = GetDay(item,ref day);
                            foreach (var date in allDate)
                            {
                                string planDate = year + m + "." + date.ToString("D2");
                                ConstructionPlanMonthDetail c = GetCommonProperty(item,i,cpic,all,dtNow, planDate,ref dt,ref dtChart);
                                //dt.Rows[j+index][14] = c.PlanDate;
                                month.Add(c);
                            }
                        }
                        j++;
                    }
                    index += j;
                }
                // 处理年表
                foreach (var item in cpys)
                {
                    int j = 0;
                    List<int> allMonth = GetMonth(item);
                    foreach (var m in allMonth)
                    {
                        int min = dayMinInMonth[m - 1].Min();
                        int day = dayMinInMonth[m - 1].IndexOf(min);
                        dayMinInMonth[m - 1][day] += item.Once;
                        string planDate = year + "." + m.ToString("D2") + "." + (day + 1).ToString("D2");
                        ConstructionPlanMonthDetail c = GetCommonProperty(item,m,cpic,all,dtNow, planDate,ref dt,ref dtChart);
                        //dt.Rows[index+j][14] = c.PlanDate;
                        month.Add(c);
                        j++;
                    }
                    index+=j;
                }
                //自动创建检修表单
                //List<MaintenanceList> mls = GetMLists(month, cpic.Year, dtNow);
                // 创建的数据存入数据,按时间排序？
                using (TransactionScope scope = new TransactionScope())
                {
                    _importRepo.BulkLoad(dt);
                    _importRepo.BulkLoad(dtChart);
                    await _importRepo.UpdateCommonStatus(query,_userID);
                    scope.Complete();
                }
                // 创建的数据按照月份拆分后，按照时间排序后，分页
                List<List<ConstructionPlanMonthDetail>> months = new List<List<ConstructionPlanMonthDetail>>();
                List<object> retList = new List<object>();
                for (int i = 0; i < 12; i++)
                {
                    months.Add(new List<ConstructionPlanMonthDetail>());
                }
                foreach (var item in month)
                {
                    months[item.Month-1].Add(item);
                }
                foreach (var item in months)
                {
                    retList.Add(new { rows = item.OrderBy(a => a.PlanDate).Take(10), total = item.Count });
                }
                ret.data = retList;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        //自动生成检修单，此逻辑未完成，暂时不做
        private List<MaintenanceList> GetMLists(List<ConstructionPlanMonthDetail> month,int year,DateTime dt)
        {
            List<MaintenanceList> ret = new List<MaintenanceList>();
            IEnumerable<IGrouping<int, ConstructionPlanMonthDetail>> groupList = month.GroupBy(a => a.Team);
            foreach (IGrouping<int, ConstructionPlanMonthDetail> group in groupList)
            {
                //需要拼凑的，目前只限于日检
                List<ConstructionPlanMonthDetail> days = group.Where(a => a.PMType == (int)PMTYPE.Day).ToList();
                MaintenanceList day = GetMList(days[0], dt);
                ret.AddRange(GetDayMLists(day, year));
                //非日检的，一个module就是一张检修表
                List<ConstructionPlanMonthDetail> notDays = group.Where(a => a.PMType != (int)PMTYPE.Day).ToList();
                foreach (var item in notDays)
                {
                    MaintenanceList notDay = GetMList(item, dt);
                    notDay.PlanDate = item.PlanDate;
                    ret.Add(notDay);
                }
            }
            return ret;
        }
        private List<MaintenanceList> GetDayMLists(MaintenanceList ml,int year)
        {
            List<MaintenanceList> ret = new List<MaintenanceList>();
            for (int i = 0; i < 12; i++)
            {
                int day = Convert.ToInt32(Model.Common.GetLastDay(i, year));
                for (int j = 0; j < day; i++)
                {
                    string tmp = year + "-" + i.ToString("D2") + "-" + j.ToString("D2");
                    ml.PlanDate = tmp;
                    ret.Add(ml);
                }
            }
            return ret;
        }
        private MaintenanceList GetMList(ConstructionPlanMonthDetail detail,DateTime dt)
        {
            MaintenanceList ret = new MaintenanceList();
            ret.CreatedBy = _userID;
            ret.CreatedTime = dt;
            ret.Status = (int)PMStatus.Init;
            ret.Team = detail.Team;
            ret.UpdatedBy = _userID;
            ret.UpdatedTime = dt;
            return ret;
        }
        private List<int> GetDay(ConstructionPlanMonth c,ref List<int> dayMin)
        {
            List<int> ret = new List<int>();
            if (c.One == 1) { ret.Add(1); dayMin[0] += c.Once; }
            if (c.Two == 1) { ret.Add(2); dayMin[1] += c.Once; }
            if (c.Three == 1) { ret.Add(3); dayMin[2] += c.Once; }
            if (c.Four == 1) { ret.Add(4); dayMin[3] += c.Once; }
            if (c.Five == 1) { ret.Add(5); dayMin[4] += c.Once; }
            if (c.Six == 1) { ret.Add(6); dayMin[5] += c.Once; }
            if (c.Seven == 1) {ret.Add(7); dayMin[6] += c.Once;}
            if (c.Eight == 1) {ret.Add(8); dayMin[7] += c.Once;}
            if (c.Nine == 1) {ret.Add(9); dayMin[8] += c.Once;}
            if (c.Ten == 1) {ret.Add(10); dayMin[9] += c.Once;}
            if (c.Eleven == 1) {ret.Add(11); dayMin[10] += c.Once;}
            if (c.Twelve == 1) {ret.Add(12); dayMin[11] += c.Once;}
            if (c.Thirteen == 1) {ret.Add(13); dayMin[12] += c.Once;}
            if (c.Fourteen == 1) {ret.Add(14); dayMin[13] += c.Once;}
            if (c.Fifteen == 1) {ret.Add(15); dayMin[14] += c.Once;}
            if (c.Sixteen == 1) {ret.Add(16); dayMin[15] += c.Once;}
            if (c.Seventeen == 1) {ret.Add(17); dayMin[16] += c.Once;}
            if (c.Eighteen == 1) {ret.Add(18); dayMin[17] += c.Once;}
            if (c.Nineteen == 1) {ret.Add(19); dayMin[18] += c.Once;}
            if (c.Twenty == 1) {ret.Add(20); dayMin[19] += c.Once;}
            if (c.TwentyOne == 1) {ret.Add(21); dayMin[20] += c.Once;}
            if (c.TwentyTwo == 1) {ret.Add(22); dayMin[21] += c.Once;}
            if (c.TwentyThree == 1) {ret.Add(23); dayMin[22] += c.Once;}
            if (c.TwentyFour == 1) {ret.Add(24); dayMin[23] += c.Once;}
            if (c.TwentyFive == 1) {ret.Add(25); dayMin[24] += c.Once;}
            if (c.TwentySix == 1) {ret.Add(26); dayMin[25] += c.Once;}
            if (c.TwentySeven == 1) {ret.Add(27); dayMin[26] += c.Once;}
            if (c.TwentyEight == 1) {ret.Add(28); dayMin[27] += c.Once;}
            if (c.TwentyNine == 1) {ret.Add(29); dayMin[28] += c.Once;}
            if (c.Thirty == 1) {ret.Add(30); dayMin[29] += c.Once;}
            if (c.ThirtyOne == 1) {ret.Add(31); dayMin[30] += c.Once;}
            return ret;
        }
        private List<int> GetMonth(ConstructionPlanYear c)
        {
            List<int> ret = new List<int>();
            if (c.January == 1) ret.Add(1);
            if (c.February == 1) ret.Add(2);
            if (c.March == 1) ret.Add(3);
            if (c.April == 1) ret.Add(4);
            if (c.May == 1) ret.Add(5);
            if (c.June == 1) ret.Add(6);
            if (c.July == 1) ret.Add(7);
            if (c.August == 1) ret.Add(8);
            if (c.September == 1) ret.Add(9);
            if (c.October == 1) ret.Add(10);
            if (c.November == 1) ret.Add(11);
            if (c.December == 1) ret.Add(12);
            return ret;
        }
        private ConstructionPlanMonthDetail GetCommonProperty (ConstructionPlanYear item,int month, 
            ConstructionPlanImportCommon cpic,AllQuery all,DateTime dtNow,string planDate,ref DataTable dt,ref DataTable dtChart)
        {
            ConstructionPlanMonthDetail c = new ConstructionPlanMonthDetail();
            #region 通用属性
            c.Line = cpic.Line;
            c.Month = month;
            c.WorkType = 112;//默认委外维护
            c.WorkTypeName = all.workType.Where(a => a.ID == c.WorkType).FirstOrDefault().Name;
            c.EqpType = item.Code;
            c.EqpTypeName= item.EqpTypeName;
            c.Location = item.Location;
            c.LocationBy = item.LocationBy;
            c.LocationName= all.locations.Where(a => a.LocationBy == c.LocationBy && a.ID==c.Location).FirstOrDefault().Name;
            c.Department = cpic.Department;
            c.Team = item.Team;
            c.TeamName= all.team.Where(a => a.ID == c.Team).FirstOrDefault().Name;
            c.PMType = (int)GetPMTypeByFrequency(item.Frequency, false);
            c.PMTypeName= all.pmType.Where(a => a.ID == c.PMType).FirstOrDefault().Name;
            c.PMCycle = item.Cycle;
            c.PMFrequency = item.Frequency;
            c.Unit = item.Unit;
            c.PlanQuantity = item.Quantity;
            c.RealQuantity = item.Quantity;
            c.Query = cpic.ID;
            c.PlanDate = planDate;
            c.UpdateTime = dtNow;
            c.UpdateBy = _userID;

            InsertRow(ref dt,c);
            InsertRow(ref dtChart, c, cpic.Year);
            #endregion
            return c;
        }
        private ConstructionPlanMonthDetail GetCommonProperty(ConstructionPlanMonth item, int month,
            ConstructionPlanImportCommon cpic, AllQuery all, DateTime dtNow, string planDate, ref DataTable dt, ref DataTable dtChart)
        {
            ConstructionPlanMonthDetail c = new ConstructionPlanMonthDetail();
            #region 通用属性
            c.Line = cpic.Line;
            c.Month = month;
            c.WorkType = 112;//默认委外维护
            c.WorkTypeName = all.workType.Where(a => a.ID == c.WorkType).FirstOrDefault().Name;
            c.EqpType = item.Code;
            c.EqpTypeName = item.EqpTypeName;
            c.Location = item.Location;
            c.LocationBy = item.LocationBy;
            c.LocationName = all.locations.Where(a => a.LocationBy == c.LocationBy && a.ID == c.Location).FirstOrDefault().Name;
            c.Department = cpic.Department;
            c.Team = item.Team;
            c.TeamName = all.team.Where(a => a.ID == c.Team).FirstOrDefault().Name;
            c.PMType = (int)GetPMTypeByFrequency(item.Frequency, true);
            c.PMTypeName = all.pmType.Where(a => a.ID == c.PMType).FirstOrDefault().Name;
            c.PMCycle= item.Cycle;
            c.PMFrequency = item.Frequency*12;
            c.Unit = item.Unit;
            c.PlanQuantity = item.Quantity;
            c.RealQuantity = item.Quantity;
            c.Query = cpic.ID;
            c.PlanDate = planDate;
            c.UpdateTime = dtNow;
            c.UpdateBy = _userID;

            InsertRow(ref dt, c);
            InsertRow(ref dtChart, c, cpic.Year);
            #endregion
            return c;
        }
        private PMTYPE GetPMTypeByFrequency(int frequency,bool isMonth)
        {
            if (isMonth)
            {
                switch (frequency)
                {
                    case 1: return PMTYPE.Month;
                    case 2: return PMTYPE.HalfMonth;
                    case 4: return PMTYPE.Week;
                    default: return PMTYPE.Day;
                }
            }
            else
            {
                switch (frequency)
                {
                    case 4: return PMTYPE.Quarter;
                    default: return PMTYPE.Year;
                }
            }
        }
        private class AllQuery
        {
            public List<QueryItem> eqpTypes { get; set; }
            public List<QueryItem> locations { get; set; }
            public List<QueryItem> team { get; set; }
            public List<QueryItem> workType { get; set; }
            public List<QueryItem> pmType { get; set; }
        }
        private DataTable GetColumnName(bool isChart)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("month"));
            dt.Columns.Add(new DataColumn("line"));
            dt.Columns.Add(new DataColumn("work_type"));
            dt.Columns.Add(new DataColumn("plan_code"));
            dt.Columns.Add(new DataColumn("plan_module_name"));
            dt.Columns.Add(new DataColumn("location"));
            dt.Columns.Add(new DataColumn("location_by"));
            dt.Columns.Add(new DataColumn("department"));
            dt.Columns.Add(new DataColumn("team"));
            dt.Columns.Add(new DataColumn("pm_type"));
            dt.Columns.Add(new DataColumn("pm_cycle"));
            dt.Columns.Add(new DataColumn("pm_frequency"));
            dt.Columns.Add(new DataColumn("unit"));
            dt.Columns.Add(new DataColumn("plan_quantity"));
            dt.Columns.Add(new DataColumn("plan_date"));
            dt.Columns.Add(new DataColumn("real_quantity"));
            dt.Columns.Add(new DataColumn("real_date"));
            dt.Columns.Add(new DataColumn("working_order"));
            dt.Columns.Add(new DataColumn("order_status"));
            dt.Columns.Add(new DataColumn("remark"));
            dt.Columns.Add(new DataColumn("query"));
            dt.Columns.Add(new DataColumn("update_time"));
            dt.Columns.Add(new DataColumn("update_by"));
            if (isChart)
            {
                dt.Columns.Add(new DataColumn("year"));
                dt.Columns.Add(new DataColumn("day"));
                dt.TableName = "construction_plan_month_chart";
            }
            else
            {
                dt.Columns.Add(new DataColumn("is_assigned"));
                dt.TableName = "construction_plan_month_detail";
            }
            return dt;
        }
        private void InsertRow(ref DataTable dt,ConstructionPlanMonthDetail c,int year=0)
        {
            DataRow dr = dt.NewRow();
            dr[0] = c.Month;
            dr[1] = c.Line;
            dr[2] = c.WorkType;
            dr[3] = c.EqpType;
            dr[4] = c.EqpTypeName;
            dr[5] = c.Location;
            dr[6] = c.LocationBy;
            dr[7] = c.Department;
            dr[8] = c.Team;
            dr[9] = c.PMType;
            dr[10] = c.PMCycle;
            dr[11] = c.PMFrequency;
            dr[12] = c.Unit;
            dr[13] = c.PlanQuantity;
            dr[14] = c.PlanDate;
            dr[15] = c.RealQuantity;
            dr[16] = c.RealDate;
            dr[17] = c.WorkingOrder;
            dr[18] = c.OrderStatus;
            dr[19] = c.Remark;
            dr[20] = c.Query;
            dr[21] = c.UpdateTime;
            dr[22] = c.UpdateBy;
            if (year!=0)
            {
                dr[23] = year;
                if (c.PlanDate.IndexOf('-') > -1)
                {
                    dr[24] = 0;
                    dr[11] = 31;
                }
                else
                {
                    
                    dr[24] = c.PlanDate.Substring(8);
                    dr[11] = 1;
                }
            }
            else
            {
                dr[23] = c.IsAssigned;
            }
            dt.Rows.Add(dr);
        }

    }
}



