using MSS.API.Common;
using MSS.API.Common.Utility;
using SZY.Platform.WebApi.Data;
using SZY.Platform.WebApi.Model;
using System;
using System.Net;
using System.Threading.Tasks;


// Coded By admin 2019/11/9 13:46:57
namespace SZY.Platform.WebApi.Service
{
    public interface IWfprocessService
    {
        Task<ApiResult> GetPageList(WfprocessParm parm);
        Task<ApiResult> Save(Wfprocess obj);
        Task<ApiResult> Update(Wfprocess obj);
        Task<ApiResult> Delete(string ids);
        Task<ApiResult> GetByID(int id);
    }

    public class WfprocessService : IWfprocessService
    {
        private readonly IWfprocessRepo<Wfprocess> _repo;
        private readonly IAuthHelper _authhelper;
        private readonly int _userID;

        public WfprocessService(IWfprocessRepo<Wfprocess> repo, IAuthHelper authhelper)
        {
            _repo = repo;
            _authhelper = authhelper;
            _userID = _authhelper.GetUserId();
        }

        public async Task<ApiResult> GetPageList(WfprocessParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                //parm.UserID = _userID;
                //parm.UserID = 40;
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

        public async Task<ApiResult> Save(Wfprocess obj)
        {
            ApiResult ret = new ApiResult();
            try
            {
                //DateTime dt = DateTime.Now;
                //obj.UpdatedTime = dt;
                //obj.CreatedTime = dt;
                //obj.UpdatedBy = _userID;
                //obj.CreatedBy = _userID;
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

        public async Task<ApiResult> Update(Wfprocess obj)
        {
            ApiResult ret = new ApiResult();
            try
            {
                Wfprocess et = await _repo.GetByID(obj.ID);
                if (et != null)
                {
                    DateTime dt = DateTime.Now;
                    //obj.UpdatedTime = dt;
                    //obj.UpdatedBy = _userID;
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
                Wfprocess obj = await _repo.GetByID(id);
                ret.data = obj;
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



