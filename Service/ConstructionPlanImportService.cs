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
    public interface IConstructionPlanImportService
    {
        Task<ApiResult> ListPage(ConstructionPlanImportParm parm);
        Task<ApiResult> ListPageCommon(ConstructionPlanCommonParm parm);
        Task<ApiResult> Save(ConstructionPlanImportCommon importCommon, IFormFile file);
        //Task<ApiResult> GetByID(int id);
    }

    public class ConstructionPlanImportService : IConstructionPlanImportService
    {
        private readonly IConstructionPlanImportRepo<ConstructionPlanYear> _repo;
        private readonly IAuthHelper _authhelper;
        private readonly int _userID;

        public ConstructionPlanImportService(IConstructionPlanImportRepo<ConstructionPlanYear> repo, IAuthHelper authhelper)
        {
            _repo = repo;
            _authhelper = authhelper;
            _userID = _authhelper.GetUserId();
        }

        public async Task<ApiResult> ListPage(ConstructionPlanImportParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _repo.ListPage(parm);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        public async Task<ApiResult> ListPageCommon(ConstructionPlanCommonParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _repo.ListPageCommon(parm);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> Save(ConstructionPlanImportCommon importCommon, IFormFile file)
        {
            ApiResult ret = new ApiResult();
            QueryItem qi = new QueryItem();
            //保存每个
            List<DeptAndLine> dls = new List<DeptAndLine>();
            //ConstructionPlanImport parm = new ConstructionPlanImport();
            //parm.monthPlans = new List<DataTable>();
            //parm.yearPlans = new List<DataTable>();
            importCommon.ImportedTime = DateTime.Now;
            importCommon.ImportedBy = _userID;
            //parm.importCommon = importCommon;

            ISheet sheet = null;
            IWorkbook workbook = null;
            //合并单元格时用到，有值时覆盖，无值时获取
            string code="", eqpTypeName="";

            bool isYear=true;
            int query=0;
            try
            {
                List<ConstructionPlanImportCommon> cpics = await _repo.ListByYearAndCompany(importCommon.Year, importCommon.Company);
                List<QueryItem> allLines = await _repo.ListAllLines();
                List<QueryItem> allDepartments = await _repo.ListAllOrgByType(OrgType.Department);
                List<QueryItem> allTeams = await _repo.ListAllOrgByType(OrgType.Team);
                //List<QueryItem> allEqpTypes = await _repo.ListAllEqpTypes();
                List<QueryItem> allLocations = await _repo.ListAllLocations();
                if (file.Length > 0)
                {
                    //利用IFormFile里面的OpenReadStream()方法直接读取文件流
                    Stream stream = file.OpenReadStream();
                    string fileType = Path.GetExtension(file.FileName);

                    #region 判断excel版本
                    //2007以上版本excel
                    if (fileType == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(stream);
                    }
                    //2007以下版本excel
                    else if (fileType == ".xls")
                    {
                        workbook = new HSSFWorkbook(stream);
                    }
                    else
                    {
                        ret.code = Code.ImportError;
                        ret.msg = "传入的不是Excel文件";
                        return ret;
                    }
                    #endregion
                    using (TransactionScope scope = new TransactionScope())
                    {
                        for (int sheetNo = 0; sheetNo < workbook.NumberOfSheets; sheetNo++)
                        {
                            sheet = workbook.GetSheetAt(sheetNo);
                            if (sheet.GetRow(0) != null)
                            {
                                string title = sheet.GetRow(0).GetCell(0).StringCellValue.Trim().Replace(" ", "");
                                if (title.IndexOf("年表") > -1)
                                {
                                    isYear = true;
                                }
                                else if (title.IndexOf("月表") > -1)
                                {
                                    isYear = false;
                                }
                                else continue;

                                string common = sheet.GetRow(2).GetCell(0).StringCellValue.Trim()
                                    .Replace(" ", "").Replace("(", "").Replace(")", "").Replace("（", "").Replace("）", "");
                                string[] tmp = common.Split('部');
                                importCommon.LineName = tmp[1];
                                importCommon.DepartmentName = tmp[0] + "部";

                                #region 部门和线路名称与已定义的名称匹配
                                qi = GetIDByName(allLines, tmp[1], "路线", ref ret);
                                if (ret.code != Code.Success) return ret;
                                else importCommon.Line = qi.ID;
                                qi = GetIDByName(allDepartments, importCommon.DepartmentName, "部门", ref ret);
                                if (ret.code != Code.Success) return ret;
                                else importCommon.Department = qi.ID;
                                #endregion
                                if (cpics.Count > 0)
                                {
                                    var tmpIds = cpics
                                        .Where(a=>a.LineName== importCommon.LineName && a.DepartmentName==importCommon.DepartmentName)
                                        .Select(a => a.ID);
                                    if (tmpIds.Count() > 0)
                                    {
                                        List<int> ids = tmpIds.ToList();
                                        //await _repo.Delete(ids);
                                        if (isYear) await _repo.Delete(ids, "construction_plan_year");
                                        else await _repo.Delete(ids, "construction_plan_month");
                                        query = tmpIds.FirstOrDefault();
                                    }
                                }
                                else if (dls.Where(a => a.Line == importCommon.Line && a.Department == importCommon.Department).Count() == 0)
                                {
                                    query = await _repo.Save(importCommon);
                                    dls.Add(new DeptAndLine() { Department = importCommon.Department, Line = importCommon.Line });
                                }
                                DataTable dt = GetColumnName(isYear);
                                DataRow dataRow = null;
                                //遍历行
                                for (int j = 5; j <= sheet.LastRowNum; j++)
                                {
                                    IRow row = sheet.GetRow(j);
                                    dataRow = dt.NewRow();
                                    if (row == null || row.FirstCellNum < 0)
                                    {
                                        continue;
                                    }
                                    string str = row.GetCell(0).ToString().Trim();
                                    if (str.Replace(" ", "").Contains("总计")) break;
                                    string rowNo = (j + 1).ToString();
                                    code = GetMergeCell(str, code, "代码(第" + rowNo + "行)", ref ret);
                                    if (ret.code != Code.Success) return ret;
                                    else dataRow[0] = code;
                                    #region 处所、班组匹配name
                                    //设备设施
                                    string name = row.GetCell(1).ToString().Trim();
                                    eqpTypeName = GetMergeCell(name, eqpTypeName, "设备设施(第" + rowNo + "行)", ref ret);
                                    if (ret.code != Code.Success) return ret;
                                    else dataRow[2] = eqpTypeName;
                                    dataRow[1] = 0;//此字段已弃用，为了不修改相关代码，默认写0
                                    //qi = GetIDByName(allEqpTypes, eqpTypeName, "设备设施(第" + rowNo + "行)", ref ret);
                                    //if (ret.code != Code.Success) return ret;
                                    //else dataRow[1] = qi.ID;
                                    //dataRow[1] = 0;//目前id和name没有保持一致，先把id默认0
                                    //处所
                                    name = row.GetCell(2).ToString().Trim();
                                    dataRow[5] = name;
                                    qi = GetIDByName(allLocations, name, "处所(第" + rowNo + "行)", ref ret);
                                    if (ret.code != Code.Success) return ret;
                                    else dataRow[3] = qi.ID; dataRow[4] = qi.LocationBy;
                                    //dataRow[3] = 0; dataRow[4]=0;
                                    //班组
                                    name = row.GetCell(3).ToString().Trim();
                                    dataRow[7] = name;
                                    qi = GetIDByName(allTeams, name, "班组(第" + rowNo + "行)", ref ret);
                                    if (ret.code != Code.Success) return ret;
                                    else dataRow[6] = qi.ID;
                                    //dataRow[6] = 0;
                                    #endregion
                                    //遍历列
                                    for (int i = 4; i < dt.Columns.Count - 2; i++)
                                    {
                                        if (i == 9 || i == 10) continue;
                                        ICell cellData = row.GetCell(i);
                                        //if (cellData != null)
                                        {
                                            if (i < 9)
                                            {
                                                str = cellData.ToString().Trim();
                                                if (ValidateCell(str, "第" + rowNo + "行第" + (i+1).ToString() + "列", ref ret)) return ret;
                                                dataRow[i + 4] = str;
                                            }
                                            else if (i > 10)
                                            {
                                                dataRow[i + 2] = cellData == null || string.IsNullOrEmpty(cellData.ToString().Trim()) ? 0 : 1;
                                            }
                                        }
                                    }
                                    //最后一列common表主键
                                    dataRow[dt.Columns.Count-1] = query;
                                    dt.Rows.Add(dataRow);
                                }
                                _repo.BulkLoad(dt);
                            }
                        }
                        scope.Complete();
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        private class DeptAndLine
        {
            public int Department { get; set; }
            public int Line { get; set; }
        }
        private string GetMergeCell(string val,string oldVal,string errMsg,ref ApiResult ret)
        {
            string res = string.IsNullOrWhiteSpace(val) ? oldVal : val;
            if (string.IsNullOrWhiteSpace(res))
            {
                ret.code = Code.ImportError;
                ret.msg = errMsg+"为空";
            }
            return res;
        }

        private bool ValidateCell(string val,string errMsg, ref ApiResult ret)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                ret.code = Code.ImportError;
                ret.msg = errMsg + "为空";
                return true;
            }
            return false;
        }

        private QueryItem GetIDByName(List<QueryItem> all,string name, string typeName,ref ApiResult msg)
        {
            QueryItem ret = new QueryItem();
            if (!ValidateCell(name, typeName, ref msg))
            {
                var listTmp = all.Where(a => a.Name == name);
                if (listTmp.Count() > 0)
                {
                    ret = listTmp.FirstOrDefault();
                }
                else
                {
                    msg.code = Code.ImportError;
                    msg.msg = "没有定义名称为" + name + "的" + typeName;
                }
            }
            return ret;
        }

        private object GetCellVal(ICell cellData)
        {
            object ret;
            if (cellData.CellType == CellType.Numeric)
            {
                //判断是否日期类型
                if (DateUtil.IsCellDateFormatted(cellData))
                {
                    ret = cellData.DateCellValue;
                }
                else
                {
                    ret = cellData.ToString().Trim();
                }
            }
            else
            {
                ret = cellData.ToString().Trim();
            }
            return ret;
        }

        private DataTable GetColumnName(bool isYear)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("code"));
            dt.Columns.Add(new DataColumn("eqp_type"));
            dt.Columns.Add(new DataColumn("eqp_type_name"));
            dt.Columns.Add(new DataColumn("location"));
            dt.Columns.Add(new DataColumn("location_by"));
            dt.Columns.Add(new DataColumn("location_name"));
            dt.Columns.Add(new DataColumn("team"));
            dt.Columns.Add(new DataColumn("team_name"));
            dt.Columns.Add(new DataColumn("unit"));
            dt.Columns.Add(new DataColumn("quantity"));
            dt.Columns.Add(new DataColumn("cycle"));
            dt.Columns.Add(new DataColumn("frequency"));
            dt.Columns.Add(new DataColumn("once"));
            if (isYear)
            {
                dt.Columns.Add(new DataColumn("january"));
                dt.Columns.Add(new DataColumn("february"));
                dt.Columns.Add(new DataColumn("march"));
                dt.Columns.Add(new DataColumn("april"));
                dt.Columns.Add(new DataColumn("may"));
                dt.Columns.Add(new DataColumn("june"));
                dt.Columns.Add(new DataColumn("july"));
                dt.Columns.Add(new DataColumn("august"));
                dt.Columns.Add(new DataColumn("september"));
                dt.Columns.Add(new DataColumn("october"));
                dt.Columns.Add(new DataColumn("november"));
                dt.Columns.Add(new DataColumn("december"));
                dt.TableName = "construction_plan_year";
            }
            else
            {
                dt.Columns.Add(new DataColumn("one"));
                dt.Columns.Add(new DataColumn("two"));
                dt.Columns.Add(new DataColumn("three"));
                dt.Columns.Add(new DataColumn("four"));
                dt.Columns.Add(new DataColumn("five"));
                dt.Columns.Add(new DataColumn("six"));
                dt.Columns.Add(new DataColumn("seven"));
                dt.Columns.Add(new DataColumn("eight"));
                dt.Columns.Add(new DataColumn("nine"));
                dt.Columns.Add(new DataColumn("ten"));
                dt.Columns.Add(new DataColumn("eleven"));
                dt.Columns.Add(new DataColumn("twelve"));
                dt.Columns.Add(new DataColumn("thirteen"));
                dt.Columns.Add(new DataColumn("fourteen"));
                dt.Columns.Add(new DataColumn("fifteen"));
                dt.Columns.Add(new DataColumn("sixteen"));
                dt.Columns.Add(new DataColumn("seventeen"));
                dt.Columns.Add(new DataColumn("eighteen"));
                dt.Columns.Add(new DataColumn("nineteen"));
                dt.Columns.Add(new DataColumn("twenty"));
                dt.Columns.Add(new DataColumn("twenty_one"));
                dt.Columns.Add(new DataColumn("twenty_two"));
                dt.Columns.Add(new DataColumn("twenty_three"));
                dt.Columns.Add(new DataColumn("twenty_four"));
                dt.Columns.Add(new DataColumn("twenty_five"));
                dt.Columns.Add(new DataColumn("twenty_six"));
                dt.Columns.Add(new DataColumn("twenty_seven"));
                dt.Columns.Add(new DataColumn("twenty_eight"));
                dt.Columns.Add(new DataColumn("twenty_nine"));
                dt.Columns.Add(new DataColumn("thirty"));
                dt.Columns.Add(new DataColumn("thirty_one"));
                dt.TableName = "construction_plan_month";
            }
            dt.Columns.Add(new DataColumn("query"));
            return dt;
        }
    }
}



