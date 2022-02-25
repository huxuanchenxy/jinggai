using MSS.API.Common;
using MSS.API.Common.Utility;
using SZY.Platform.WebApi.Data;
using SZY.Platform.WebApi.Model;
using System;
using System.Threading.Tasks;
using static MSS.API.Common.MyDictionary;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


// Coded By admin 2019/10/21 13:07:35
namespace SZY.Platform.WebApi.Service
{
    public interface IConstructionPlanService
    {
        Task<ApiResult> GetPageList(ConstructionPlanParm parm);
        Task<ApiResult> Save(ConstructionPlan obj);
        Task<ApiResult> Update(ConstructionPlan obj);
        Task<ApiResult> Delete(string ids);
        Task<ApiResult> GetByID(int id);
    }

    public class ConstructionPlanService : IConstructionPlanService
    {
        private readonly IConstructionPlanRepo<ConstructionPlan> _repo;
        private readonly IAuthHelper _authhelper;
        private readonly int _userID;

        public ConstructionPlanService(IConstructionPlanRepo<ConstructionPlan> repo, IAuthHelper authhelper)
        {
            _repo = repo;
            _authhelper = authhelper;
            _userID = _authhelper.GetUserId();
        }

        public async Task<ApiResult> GetPageList(ConstructionPlanParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                parm.userId = _userID;
                //parm.UserID = 40;
                parm.processGUID = "c4c03c5e-63a3-47d6-913f-b8e08a51f5f8";
                var data = await _repo.GetPageList(parm);
                ret.code = Code.Success;
                ret.data = data;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }

            return ret;
        }

        public async Task<ApiResult> Save(ConstructionPlan obj)
        {
            ApiResult ret = new ApiResult();
            try
            {
                DateTime dt = DateTime.Now;
                obj.UpdatedTime = dt;
                obj.CreatedTime = dt;
                obj.UpdatedBy = _userID;
                obj.CreatedBy = _userID;
                ret.data = await _repo.Save(obj);
                ret.code = Code.Success;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> Update(ConstructionPlan obj)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ConstructionPlan et = await _repo.GetByID(obj.Id);
                if (et != null)
                {
                    DateTime dt = DateTime.Now;
                    obj.UpdatedTime = dt;
                    obj.UpdatedBy = _userID;
                    ret.data = await _repo.Update(obj);
                    ret.code = Code.Success;
                }
                else
                {
                    ret.code = Code.DataIsnotExist;
                    ret.msg = "所要修改的数据不存在";
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

        public async Task<ApiResult> Delete(string ids)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = await _repo.Delete(ids.Split(','), _userID);
                ret.code = Code.Success;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }

        public async Task<ApiResult> GetByID(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                ret.data = 11;
                ret.code = Code.Success;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
                return ret;
            }
        }


    }
}



