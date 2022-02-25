using Microsoft.AspNetCore.Http;
using MSS.API.Common;
using MSS.API.Common.Utility;
using SZY.Platform.WebApi.Data;
using SZY.Platform.WebApi.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using static MSS.API.Common.MyDictionary;


// Coded By admin 2019/9/26 17:41:26
namespace SZY.Platform.WebApi.Service
{
    public interface IMaintenanceService
    {
        #region 已弃用
        Task<ApiResult> SaveMItem(MaintenanceItem maintenanceItem);

        Task<ApiResult> SaveMMoudleItem(List<MaintenanceModuleItem> maintenanceModuleItem);

        Task<ApiResult> SaveMMoudleItemValue(MaintenanceModuleItemValueParm parm);

        Task<ApiResult> SaveMModule(MaintenanceModule maintenanceModule);
        Task<ApiResult> SaveMList(MaintenanceList maintenanceList);

        Task<ApiResult> ListPage(MaintenanceListParm parm);
        Task<ApiResult> ListMModule(int id, bool isInit);
        #endregion

        Task<ApiResult> ImportModule(PMModule pMModule, IFormFile file);
        Task<ApiResult> ListModulePage(PMModuleParm parm);
        Task<ApiResult> GetModuleByID(int id);
        Task<ApiResult> SavePMEntity(PMEntity pmEntity);
        Task<ApiResult> UpdatePMEntity(PMEntity pmEntity);
        Task<ApiResult> UpdatePMEntityStatus(int id, int status);
        Task<ApiResult> DelPMEntity(string[] ids);
        Task<ApiResult> ListEntityPage(PMEntityParm parm);
        Task<ApiResult> GetEntityByID(int id,bool isUpdate);
    }

    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepo<MaintenanceItem> _repo;
        private readonly IConstructionPlanImportRepo<ConstructionPlanYear> _importRepo;
        private readonly IConstructionPlanMonthDetailRepo<ConstructionPlanMonthDetail> _detailRepo;
        private readonly IAuthHelper _authhelper;
        private readonly int _userID;

        public MaintenanceService(IMaintenanceRepo<MaintenanceItem> repo,
            IConstructionPlanMonthDetailRepo<ConstructionPlanMonthDetail> detailRepo,
            IConstructionPlanImportRepo<ConstructionPlanYear> importRepo,IAuthHelper authhelper)
        {
            _repo = repo;
            _importRepo = importRepo;
            _detailRepo = detailRepo;
            _authhelper = authhelper;
            _userID = _authhelper.GetUserId();
        }
        #region 已弃用
        #region MaintenanceItem
        public async Task<ApiResult> SaveMItem(MaintenanceItem maintenanceItem)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _repo.SaveMItem(maintenanceItem);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region MaintenanceModuleItem
        public async Task<ApiResult> SaveMMoudleItem(List<MaintenanceModuleItem> maintenanceModuleItem)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _repo.SaveMMoudleItem(maintenanceModuleItem);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region MaintenanceModuleItemValue
        public async Task<ApiResult> SaveMMoudleItemValue(MaintenanceModuleItemValueParm parm)
        {
            ApiResult ret = new ApiResult();
            int retData = 0;
            int listID = parm.MaintenanceModuleItemValues[0].List;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    retData = await _repo.DelMMoudleItemValue(listID);
                    retData = await _repo.SaveMMoudleItemValue(parm.MaintenanceModuleItemValues);
                    int status = (int)PMStatus.Editing;
                    if (parm.IsFinished)
                    {
                        status = (int)PMStatus.Finished;
                    }
                    retData = await _repo.UpdateMList(status,_userID, listID);
                    scope.Complete();
                }
                ret.data = retData;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region MaintenanceModule
        public async Task<ApiResult> SaveMModule(MaintenanceModule maintenanceModule)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _repo.SaveMModule(maintenanceModule);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        public async Task<ApiResult> ListMModule(int id,bool isInit)
        {
            ApiResult ret = new ApiResult();
            try
            {
                List<MaintenanceModule> mms = new List<MaintenanceModule>();
                List<MaintenanceModuleItemAll> mmiAll = new List<MaintenanceModuleItemAll>();
                if (isInit)
                {
                    mmiAll = await _repo.ListItems(id);
                    mms=ListItems(mmiAll, id);
                }
                else
                {
                    mmiAll = await _repo.ListValues(id);
                    mms=ListValues(mmiAll, id);
                }
                
                ret.data =mms;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        private List<MaintenanceModule> ListItems(List<MaintenanceModuleItemAll> mmiAll, int listID)
        {
            List<MaintenanceModule> mms = new List<MaintenanceModule>();
            IEnumerable<IGrouping<int, MaintenanceModuleItemAll>> groupList = mmiAll.GroupBy(a => a.ID);
            foreach (IGrouping<int, MaintenanceModuleItemAll> group in groupList)
            {
                int count = group.FirstOrDefault().Count;
                for (int i = 0; i < count; i++)
                {
                    MaintenanceModule mm = new MaintenanceModule();
                    mm.ID = group.Key;
                    mm.Name = group.FirstOrDefault().ModuleName;
                    if (count > 1)
                    {
                        mm.ShowName = mm.Name + "(设备设施" + (i + 1).ToString() + ")";
                        mm.IsShowEqp = true;
                    }
                    else
                    {
                        mm.ShowName = mm.Name;
                    }
                    mm.Items = new List<MaintenanceModuleItemValue>();
                    foreach (var item in group)
                    {
                        MaintenanceModuleItemValue mmiv = new MaintenanceModuleItemValue();
                        mmiv.Item = item.Item;
                        mmiv.List = listID;
                        mmiv.Module = mm.ID;
                        mmiv.ItemName = item.ItemName;
                        mmiv.ItemType = item.ItemType;
                        mm.Items.Add(mmiv);
                    }
                    mms.Add(mm);
                }
            }
            return mms;
        }
        private List<MaintenanceModule> ListValues(List<MaintenanceModuleItemAll> mmiAll, int listID)
        {
            List<MaintenanceModule> mms = new List<MaintenanceModule>();
            IEnumerable<IGrouping<int, MaintenanceModuleItemAll>> groupList = mmiAll.GroupBy(a => a.ID);
            foreach (IGrouping<int, MaintenanceModuleItemAll> group in groupList)
            {
                IEnumerable<IGrouping<string, MaintenanceModuleItemAll>> groupByEqp = group.GroupBy(a => a.Eqp);
                foreach (IGrouping<string, MaintenanceModuleItemAll> items in groupByEqp)
                {
                    MaintenanceModule mm = new MaintenanceModule();
                    mm.ID = group.Key;
                    mm.Name = group.FirstOrDefault().ModuleName;
                    if (items.Key!=null)
                    {
                        mm.ShowName = mm.Name + "(设备设施" + items.Key + ")";
                        mm.IsShowEqp = true;
                        mm.Eqp = items.Key;
                    }
                    else
                    {
                        mm.ShowName = mm.Name;
                    }
                    mm.Items = new List<MaintenanceModuleItemValue>();
                    foreach (var item in items)
                    {
                        MaintenanceModuleItemValue mmiv = new MaintenanceModuleItemValue();
                        mmiv.Item = item.Item;
                        mmiv.List = listID;
                        mmiv.Module = mm.ID;
                        mmiv.ItemName = item.ItemName;
                        mmiv.ItemType = item.ItemType;
                        mmiv.ItemValue = item.ItemValue;
                        mm.Items.Add(mmiv);
                    }
                    mms.Add(mm);
                }
            }
            return mms;
        }
        #endregion

        #region MaintenanceList
        public async Task<ApiResult> SaveMList(MaintenanceList maintenanceList)
        {
            ApiResult ret = new ApiResult();
            int retData = 0;
            DateTime dt = DateTime.Now;
            maintenanceList.CreatedBy = _userID;
            maintenanceList.CreatedTime = dt;
            maintenanceList.Status = (int)PMStatus.Init;
            maintenanceList.UpdatedBy = _userID;
            maintenanceList.UpdatedTime = dt;
            try
            {
                List<MaintenancePlanDetail> mpds = new List<MaintenancePlanDetail>();
                using (TransactionScope scope = new TransactionScope())
                {
                    retData = await _repo.SaveMList(maintenanceList);
                    foreach (var item in maintenanceList.Details)
                    {
                        item.List = retData;
                        mpds.Add(item);
                    }
                    retData = await _repo.SaveMDetail(mpds);
                    scope.Complete();
                }
                ret.data = retData;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> ListPage(MaintenanceListParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                MaintenanceListView mlv = await _repo.ListPage(parm);
                List<QueryItem> locations=await _importRepo.ListAllLocations();
                foreach (var item in mlv.rows)
                {
                    item.LocationName = locations.Where(a => a.LocationBy == item.Locationby && a.ID == item.Location).FirstOrDefault().Name;
                }
                ret.data = mlv;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion
        #endregion

        #region PMModule
        public async Task<ApiResult> ImportModule(PMModule pMModule, IFormFile file)
        {
            ApiResult ret = new ApiResult();
            pMModule.CreatedTime = DateTime.Now;
            pMModule.CreatedBy = _userID;
            List<List<string>> showData = new List<List<string>>();
            List<object> showMerge = new List<object>();

            try
            {
                //if (file.Length > 0)
                {
                    //利用IFormFile里面的OpenReadStream()方法直接读取文件流
                    Stream stream = file.OpenReadStream();
                    string fileType = Path.GetExtension(file.FileName);
                    ListShowModule(stream, fileType,ref showData,ref showMerge);
                    ret.data= new { data = showData, mergeCells = showMerge };

                    PDFHelper pdf = new PDFHelper();
                    string fileNameNew = Guid.NewGuid().ToString();
                    string[] fileName = file.FileName.Split('.');
                    string ext = "." + fileName[fileName.Length - 1];
                    pMModule.FilePath = FilePath.PMPATH.Replace('/', '\\') + "Module\\" + fileNameNew + ext;
                    pMModule.FileName = file.FileName;
                    await _repo.SavePMModule(pMModule);
                    pdf.SaveFile(file, pMModule.FilePath);
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

        private void ListShowModule(Stream stream,string fileType,
            ref List<List<string>> showData, ref List<object> showMerge)
        {
            IWorkbook workbook = null;
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
            #endregion
            ISheet sheet = workbook.GetSheetAt(0);
            //以第一行的长度为标准，每行的LastRowNum有时候会不一样
            int colLength = sheet.GetRow(0).LastCellNum;
            //遍历行
            for (int j = 0; j <= sheet.LastRowNum; j++)
            {
                IRow row = sheet.GetRow(j);
                List<string> tmp = new List<string>();
                for (int i = 0; i < colLength; i++)
                {
                    var cell = row.GetCell(i);
                    if (cell != null)
                    {
                        tmp.Add(cell.ToString().Trim());
                    }
                    else
                    {
                        tmp.Add("");
                    }
                }
                showData.Add(tmp);
            }
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                CellRangeAddress range = sheet.GetMergedRegion(i);
                showMerge.Add(new
                {
                    row = range.FirstRow,
                    col = range.FirstColumn,
                    rowspan = range.LastRow - range.FirstRow + 1,
                    colspan = range.LastColumn - range.FirstColumn + 1
                });
            }
        }

        public async Task<ApiResult> ListModulePage(PMModuleParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _repo.ListModulePage(parm);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> GetModuleByID(int id)
        {
            ApiResult ret = new ApiResult();
            List<List<string>> showData = new List<List<string>>();
            List<object> showMerge = new List<object>();
            try
            {
                PMModule pMModule = await _repo.GetModuleByID(id);
                if (pMModule.Location != null && pMModule.LocationBy != null)
                {
                    List<QueryItem> queryItems = await _importRepo.ListAllLocations(pMModule.Location, pMModule.LocationBy);
                    pMModule.LocationName = queryItems.FirstOrDefault().Name;
                }
                using (FileStream file = new FileStream(pMModule.FilePath, FileMode.Open, FileAccess.Read))
                {
                    string fileType = Path.GetExtension(pMModule.FileName);
                    ListShowModule(file, fileType, ref showData, ref showMerge);
                }
                ret.data = new {obj= pMModule, data = showData, mergeCells = showMerge };
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        private Stream GetStream(FileStream file)
        {
            byte[] bytes = new byte[file.Length];
            file.Read(bytes, 0, bytes.Length);
            file.Close();
            //把 byte[] 转换成 Stream   
            return new MemoryStream(bytes);
        }
        #endregion

        #region PMEntity
        public async Task<ApiResult> SavePMEntity(PMEntity pmEntity)
        {
            ApiResult ret = new ApiResult();
            EqpHistory eqp = new EqpHistory();
            DateTime dt= DateTime.Now;
            pmEntity.CreatedTime = dt;
            pmEntity.CreatedBy = _userID;
            pmEntity.UpdatedBy = _userID;
            pmEntity.UpdatedTime = dt;
            pmEntity.Status = (int)PMStatus.Editing;
            if (pmEntity.IsFinished)
            {
                pmEntity.Status = (int)PMStatus.Finished;
                if (pmEntity.Eqp != null)
                {
                    eqp.CreatedBy = _userID;
                    eqp.CreatedTime = dt;
                    eqp.EqpID = (int)pmEntity.Eqp;
                    eqp.Type = (int)EqpHistoryType.Maintenance;
                    eqp.ShowName = pmEntity.Title;
                }
            }
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //保存excel
                    PMModule module = await _repo.GetModuleByIDEasy(pmEntity.Module);
                    //复制模板为实例
                    string modulePath = Path.GetDirectoryName(module.FilePath);
                    string entityPath = modulePath.Substring(0, modulePath.Length - 6) + "Entity\\";
                    string fileName = pmEntity.Title + dt.ToString("yyyyMMdd");
                    string fileType = Path.GetExtension(module.FileName);
                    entityPath += fileName + fileType;
                    pmEntity.FilePath = entityPath;
                    if (File.Exists(entityPath))
                    {
                        File.Delete(entityPath);
                    }
                    File.Copy(module.FilePath, entityPath);
                    SaveExcelEntity(entityPath, fileType, pmEntity);
                    int newid = await _repo.SavePMEntity(pmEntity);
                    eqp.WorkingOrder = newid;
                    List<PMEntityMonthDetail> details = new List<PMEntityMonthDetail>();
                    foreach (int item in pmEntity.PMMonthDetails)
                    {
                        PMEntityMonthDetail detail = new PMEntityMonthDetail();
                        detail.MonthDetail = item;
                        detail.PMEntity = newid;
                        details.Add(detail);
                    }
                    ret.data = await _repo.SavePMEntityMonthDetail(details);
                    if (pmEntity.IsFinished)
                    {
                        await _repo.SaveEqpHistory(eqp);
                    }
                    scope.Complete();
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
        public async Task<ApiResult> UpdatePMEntity(PMEntity pmEntity)
        {
            ApiResult ret = new ApiResult();
            EqpHistory eqp = new EqpHistory();
            DateTime dt = DateTime.Now;
            pmEntity.UpdatedBy = _userID;
            pmEntity.UpdatedTime = dt;
            pmEntity.Status = (int)PMStatus.Editing;
            if (pmEntity.IsFinished)
            {
                pmEntity.Status = (int)PMStatus.Finished;
                if (pmEntity.Eqp != null)
                {
                    eqp.CreatedBy = _userID;
                    eqp.CreatedTime = dt;
                    eqp.EqpID = (int)pmEntity.Eqp;
                    eqp.Type = (int)EqpHistoryType.Maintenance;
                    eqp.ShowName = pmEntity.Title;
                    eqp.WorkingOrder = pmEntity.ID;
                }
            }
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //保存excel
                    PMModule module = await _repo.GetModuleByIDEasy(pmEntity.Module);
                    PMEntity oldEntity = await _repo.GetEntityByID(pmEntity.ID);
                    string modulePath, entityPath, fileName;
                    string fileType = Path.GetExtension(module.FileName);
                    entityPath = oldEntity.FilePath;
                    if (File.Exists(entityPath))
                    {
                        File.Delete(entityPath);
                    }
                    if (pmEntity.Module != oldEntity.Module)
                    {
                        //复制模板为实例
                        modulePath = Path.GetDirectoryName(module.FilePath);
                        entityPath = modulePath.Substring(0, modulePath.Length - 6) + "Entity\\";
                        fileName = pmEntity.Title + dt.ToString("yyyyMMdd");
                        fileType = Path.GetExtension(module.FileName);
                        entityPath += fileName + fileType;
                    }
                    File.Copy(module.FilePath, entityPath);
                    SaveExcelEntity(entityPath, fileType, pmEntity);
                    if (pmEntity.Module != oldEntity.Module || pmEntity.IsPlanChanged || pmEntity.Status!=oldEntity.Status)
                    {
                        pmEntity.FilePath = entityPath;
                        ret.data = await _repo.UpdatePMEntity(pmEntity);
                    }
                    if (pmEntity.IsPlanChanged)
                    {
                        await _repo.DelPMEntityMonthDetail(new string[] { pmEntity.ID.ToString() });
                        List<PMEntityMonthDetail> details = new List<PMEntityMonthDetail>();
                        foreach (int item in pmEntity.PMMonthDetails)
                        {
                            PMEntityMonthDetail detail = new PMEntityMonthDetail();
                            detail.MonthDetail = item;
                            detail.PMEntity = pmEntity.ID;
                            details.Add(detail);
                        }
                        ret.data = await _repo.SavePMEntityMonthDetail(details);
                    }
                    if (pmEntity.IsFinished)
                    {
                        await _repo.SaveEqpHistory(eqp);
                    }
                    scope.Complete();
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
        public async Task<ApiResult> UpdatePMEntityStatus(int id,int status)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _repo.UpdatePMEntityStatus(id,status,_userID);
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }
        private void SaveExcelEntity(string entityPath,string fileType,PMEntity pmEntity)
        {
            using (FileStream file = new FileStream(entityPath, FileMode.Open, FileAccess.ReadWrite))
            {
                IWorkbook workbook = null;
                #region 判断excel版本
                //2007以上版本excel
                if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook(file);
                }
                //2007以下版本excel
                else if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook(file);
                }
                #endregion
                ISheet sheet = workbook.GetSheetAt(0);
                //遍历行
                for (int j = 0; j < pmEntity.contents.Count; j++)
                {
                    IRow row = sheet.GetRow(j);
                    List<string> cols = pmEntity.contents[j];
                    for (int i = 0; i < cols.Count; i++)
                    {
                        if (cols[i] != null)
                        {
                            ICell cell = row.GetCell(i);
                            cell.SetCellValue(cols[i]);
                        }
                    }
                }
                using (FileStream fs = File.OpenWrite(entityPath))
                {
                    workbook.Write(fs);
                }
            }
        }
        public async Task<ApiResult> DelPMEntity(string[] ids)
        {
            ApiResult ret = new ApiResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    List<PMEntity> entities = await _repo.GetEntityByIDs(ids);
                    foreach (var item in entities)
                    {
                        if (File.Exists(item.FilePath))
                        {
                            File.Delete(item.FilePath);
                        }
                    }
                    ret.data = await _repo.DelPMEntity(ids);
                    ret.data = await _repo.DelPMEntityMonthDetail(ids);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> ListEntityPage(PMEntityParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                PMEntityView view = await _repo.ListEntityPage(parm);
                List<QueryItem> locations = await _importRepo.ListAllLocations();
                foreach (var item in view.rows)
                {
                    item.LocationName = locations.Where(a => a.LocationBy == item.Locationby && a.ID == item.Location)
                        .FirstOrDefault().Name;
                }
                ret.data = view;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        public async Task<ApiResult> GetEntityByID(int id,bool isUpdate)
        {
            ApiResult ret = new ApiResult();
            List<List<string>> showData = new List<List<string>>();
            List<object> showMerge = new List<object>();
            PMModule module = null;
            List<ConstructionPlanMonthDetail> cpmd = null;
            PMEntity entity = null;
            try
            {
                if (isUpdate)
                {
                    entity = await _repo.GetEntityByID(id);
                    module = await _repo.GetModuleByID(entity.Module);
                    cpmd = await _detailRepo.GetByIDs(await _repo.ListMonthDetail(id));
                    List<QueryItem> locations = await _importRepo.ListAllLocations();
                    foreach (var item in cpmd)
                    {
                        item.LocationName = locations.Where(a => a.LocationBy == item.LocationBy && a.ID == item.Location)
                            .FirstOrDefault().Name;
                    }
                }
                else
                {
                    entity = await _repo.GetEntityDetailByID(id);
                    entity.LocationName = (await _importRepo.ListAllLocations(entity.Location, entity.Locationby))
                        .FirstOrDefault().Name;
                }
                using (FileStream file = new FileStream(entity.FilePath, FileMode.Open, FileAccess.Read))
                {
                    string fileType = Path.GetExtension(entity.FilePath);
                    ListShowModule(file, fileType, ref showData, ref showMerge);
                }
                if (isUpdate) ret.data = new { entity.Eqp,module, cpmd, showData, showMerge };
                else ret.data = new { entity, showData, showMerge };
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }
            return ret;
        }

        #endregion

    }
}



