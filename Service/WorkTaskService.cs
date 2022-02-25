using Microsoft.Extensions.Caching.Distributed;
using MSS.API.Common;
using MSS.API.Common.Utility;
using SZY.Platform.WebApi.Data;
using SZY.Platform.WebApi.Model;
using System;
using System.Net;
using System.Threading.Tasks;
using MSS.API.Common.DistributedEx;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Localization;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections;

namespace SZY.Platform.WebApi.Service
{
    public class WorkTaskService : IWorkTaskService
    {
        private readonly IWorkTaskRepo<TaskViewModel> _repo;
        private readonly IAuthHelper _authhelper;
        private readonly IWfprocessRepo<Wfprocess> _wfprocessRepo;
        private readonly IDistributedCache _cache;
        public WorkTaskService(IWorkTaskRepo<TaskViewModel> repo, IAuthHelper authhelper, IWfprocessRepo<Wfprocess> wfprocessRepo, IDistributedCache cache)
        {
            _repo = repo;
            _authhelper = authhelper;
            _wfprocessRepo = wfprocessRepo;
            _cache = cache;
        }

        /// <summary>
        /// 我的待办
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetReadyTasks(WorkTaskQueryParm parm)
        {
            ApiResult ret = new ApiResult();

            try
            {
                //List<dynamic> pa = new List<dynamic>();
                //await _cache.HSetAsync("testhset", "testfield", "testvalue");
                //await _cache.SetAsync("testset1", new ApiResult() { code = Code.Success, data = 1, msg = "1" },null);
                //ApiResult pp = new ApiResult() { code = Code.Success, data = 2, msg = "2" };
                //ApiResult pp1 = new ApiResult() { code = Code.Success, data = 3, msg = "3" };
                //await _cache.HMSetAsync("testhmset",pp,pp1);

                //await _cache.HSetAsync("testobjset4", new ApiResult() { code = Code.Success, data = 4, msg = "4" });

                //var tmp = await _cache.HGetAsync<dynamic>("testobjset4","data");
                parm.ActivityState = 1;
                parm.AssignedToUserID = _authhelper.GetUserId();
                //parm.AssignedToUserID = 40;
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

        public async Task<ApiResult> GetFinishTasks(WorkTaskQueryParm parm)
        {
            ApiResult ret = new ApiResult();

            try
            {
                parm.ActivityState = 4;
                parm.AssignedToUserID = _authhelper.GetUserId();
                //parm.AssignedToUserID = 40;
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

        /// <summary>
        /// 我的申请
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetPageMyApply(WorkTaskQueryParm parm)
        {
            ApiResult ret = new ApiResult();

            try
            {
                parm.AssignedToUserID = _authhelper.GetUserId();
                //parm.AssignedToUserID = 40;
                var data = await _repo.GetPageMyApply(parm);
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

        /// <summary>
        /// 流转日志
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetPageActivityInstance(WorkQueryParm parm)
        {
            ApiResult ret = new ApiResult();

            try
            {
                parm.UserID = _authhelper.GetUserId();
                //parm.UserID = 40;
                var data = await _repo.GetPageActivityInstance(parm);
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



        public async Task<WfRet> StartProcess(WfReq parm)
        {
            WfRet ret = new WfRet();

            return ret;
        }


        public async Task<WfRet> GetNextStepRoleUserTree(WfReq parm)
        {
            WfRet ret = new WfRet();


            return ret;
        }

        public async Task<WfRet> NextProcess(WfReq parm)
        {
            WfRet ret = new WfRet();


            return ret;
        }

        public async Task<WfRet> WithdrawProcess(WfReq parm)
        {
            WfRet ret = new WfRet();


            return ret;
        }

        public async Task<WfRet> SendBackProcess(WfReq parm)
        {
            WfRet ret = new WfRet();


            return ret;
        }

        public async Task<WfRet> GetProcessListSimple()
        {
            WfRet ret = new WfRet();


            return ret;
        }

        public async Task<WfRet> ReverseProcess(WfReq parm)
        {
            WfRet ret = new WfRet();

            return ret;
        }
        public async Task<WfRet> CancelProcess(WfReq parm)
        {
            WfRet ret = new WfRet();


            return ret;
        }

        public async Task<WfRet> QueryReadyActivityInstance(WfReq parm)
        {
            WfRet ret = new WfRet();


            return ret;
        }

        public async Task<WfRet> QueryCompletedTasks(WfReq parm)
        {
            WfRet ret = new WfRet();

            return ret;
        }

        private async Task<WfReq> getWfCommonReq(WfReq parm)
        {
            parm.UserID = _authhelper.GetUserId();
            return parm;
        }

        public async Task<ApiResult> InitData()
        {
            ApiResult ret = new ApiResult();
            try
            {
                string fundcode = "001838,165513,008889,006031,008087,007574,519191,004854,519981,096001,206011,165520,040048,180003,161130,001092,118002,007824,000822,519171,164819,004317,008121,162411,161036,002938,002251,501012,003359,000995,005457,260109,160638,160630,502023,001230,006676,213008,165525,168204,168203,001628,161725,164908,161129,160141,160216,160222,050018,050024,002982,000179,003634,070001,160723,007216,160631,003194,163208,167301,539003,001552,001210,740001,005478,006105,200010,006128,006197,002345,519185,519196,161819,005033,005037,001261,006439,006438,160516,270042,450002,160218,160221,006817,001723,007164,519170,164824,164825,164402,161724,001404,161720,080002,350002,006282,006250,378546,005052,007280,160135,110025,161127,159941";

                string url = "https://api.doctorxiong.club/v1/fund?code=" + fundcode;
                FundRetComm response = HttpClientHelper.GetResponse<FundRetComm>(url);
                if (response.data != null)
                {
                    var data = response.data;
                    foreach (var d in data)
                    {
                        Myfund obj = new Myfund()
                        {
                            Code = d.code,
                            Name = d.name,
                            Daygrowth = d.dayGrowth,
                            Networth = d.netWorth,
                            Totalworth = d.totalWorth,
                            Updatetime = DateTime.Now,
                            Worthdate = d.worthDate
                        };
                        await _repo.Save2(obj);
                    }
                }
                ret.data = response;
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

        public async Task<ApiResult> InitData2()
        {
            ApiResult ret = new ApiResult();
            try
            {
                //从天天基金网站上拔取
                //string fundcode = "001838,165513,008889,006031,008087,007574,519191,004854,519981,096001,206011,165520,040048,180003,161130,001092,118002,007824,000822,519171,164819,004317,008121,162411,161036,002938,002251,501012,003359,000995,005457,260109,160638,160630,502023,001230,006676,213008,165525,168204,168203,001628,161725,164908,161129,160141,160216,160222,050018,050024,002982,000179,003634,070001,160723,007216,160631,003194,163208,167301,539003,001552,001210,740001,005478,006105,200010,006128,006197,002345,519185,519196,161819,005033,005037,001261,006439,006438,160516,270042,450002,160218,160221,006817,001723,007164,519170,164824,164825,164402,161724,001404,161720,080002,350002,006282,006250,378546,005052,007280,160135,110025,161127,159941";
                string fundcode = string.Empty;
                //string fundcode2 = string.Empty;
                var dbdata = await _repo.GetPageList(new MyfundParm() { page = 1, rows = 1000, sort = "id", order = "asc" });
                int i = 0;
                //foreach (var d1 in dbdata.rows)
                //{
                //    if (d1.Balance != 0)
                //    {
                //        if (i % 2 == 0)
                //        {
                //            fundcode += d1.Code + ",";
                //        }
                //        else
                //        {
                //            fundcode2 += d1.Code + ",";
                //        }
                //        i++;

                //    }

                //}
                //string url = "https://api.doctorxiong.club/v1/fund?code=" + fundcode;
                //FundRetComm response = HttpClientHelper.GetResponse<FundRetComm>(url);
                //if (response.data != null)
                //{
                //    var data = response.data;
                //    foreach (var d in data)
                //    {
                //        var dd = await _repo.GetByCode(d.code);
                //        Myfund obj = new Myfund()
                //        {
                //            Id = dd.Id,
                //            Code = d.code,
                //            Name = d.name,
                //            Daygrowth = d.dayGrowth,
                //            Networth = d.netWorth,
                //            Totalworth = d.totalWorth,
                //            Updatetime = DateTime.Now,
                //            Worthdate = d.worthDate
                //        };
                //        await _repo.Update(obj);
                //    }
                //}

                //string url2 = "https://api.doctorxiong.club/v1/fund?code=" + fundcode2;
                //string url2 = $@"https://api.doctorxiong.club/v1/fund?code=008889,007574,519981,206011,118002,000822,164819,008121,161036,501012,000995,260109,160630,006676,165525,168203,161725,161129,160216,050018,002982,003634,160723,160631,163208,539003,001210,005478,200010,006197,519185,161819,005037,006439,160516,450002,160221,001723,519170,164825,161724,161720,350002,006250,005052,160135,161127,";
                //string[] url2arr = fundcode2.Split(",");




                var data = dbdata.rows;
                foreach (var d1 in data)
                {
                    string url2 = "https://api.doctorxiong.club/v1/fund?code=" + d1.Code;
                    FundRetComm ret2 = HttpClientHelper.GetResponse<FundRetComm>(url2);
                    if (ret2.data != null)
                    {
                        var d = ret2.data[0];
                        Myfund obj = new Myfund()
                        {
                            Id = d1.Id,
                            Code = d.code,
                            Name = d.name,
                            Daygrowth = d.dayGrowth,
                            Networth = d.netWorth,
                            Totalworth = d.totalWorth,
                            Updatetime = DateTime.Now,
                            Worthdate = d.worthDate
                        };
                        await _repo.Update(obj);
                    }
                }



                ret.data = null;
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

        public async Task<ApiResult> InitData2V2()
        {
            ApiResult ret = new ApiResult();
            try
            {

                string fundcode = string.Empty;
                var dbdata = await _repo.GetPageList(new MyfundParm() { page = 1, rows = 1000, sort = "id", order = "asc" });

                var data = dbdata.rows;
                foreach (var d1 in data)
                {
                    try
                    {

                        string url = $@"https://fundmobapi.eastmoney.com/FundMApi/FundBaseTypeInformation.ashx?FCODE=" + d1.Code + "&deviceid=Wap&plat=Wap&product=EFund&version=2.0.0&Uid=9572315881384690&_=" + MathHelper.GetTimeStamp();
                        Root ret2 = HttpClientHelper.GetResponse<Root>(url);
                        if (ret2.ErrCode == 0)
                        {
                            var d = ret2.Datas;
                            Myfund obj = new Myfund()
                            {
                                Id = d1.Id,
                                Code = d1.Code,
                                Name = d1.Name,
                                Daygrowth = decimal.Parse(d.RZDF),
                                Networth = decimal.Parse(d.DWJZ),
                                Totalworth = decimal.Parse(d.LJJZ),
                                Updatetime = DateTime.Now,
                                Worthdate = Convert.ToDateTime(d.FSRQ)
                            };
                            await _repo.Update(obj);
                        }
                    }
                    catch (Exception ex)
                    {
                        string strex = ex.ToString();
                    }
                }



                ret.data = null;
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

        public async Task<ApiResult> GetPageList(MyfundParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                parm.page = 1;
                parm.rows = 1000;
                var data = await _repo.GetPageList(parm);
                var d2 = await _repo.GetAllBalance();
                string codes = string.Empty;
                int num = 1;
                foreach (var d in data.rows)
                {
                    codes += d.Code + ",";
                    d.Num = num++;
                    d.Percent = Math.Round(d.Balance / d2.Balance * 100, 2);
                    d.PercentGrowth = d.Costavg > 0 ? Math.Round((d.Networth - d.Costavg) / d.Costavg * 100, 2) : 0;
                }
                //string url = "https://api.doctorxiong.club/v1/fund?code=" + codes;
                //FundRetComm response = HttpClientHelper.GetResponse<FundRetComm>(url);
                //foreach (var d in data.rows)
                //{
                //    if (response.data != null)
                //    {
                //        var cur = response.data.Where(c => c.code == d.Code).FirstOrDefault();
                //        if (cur != null)
                //        {
                //            d.ExpectGrowth = cur.expectGrowth;
                //        }

                //    }
                //}

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

        public async Task<ApiResult> Update2(Myfund obj)
        {
            ApiResult ret = new ApiResult();
            try
            {
                var data = await _repo.Update2(obj);
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

        public async Task<ApiResult> GetById(int id)
        {
            ApiResult ret = new ApiResult();
            try
            {
                var data = await _repo.GetById(id);
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

        public async Task<ApiResult> UpdateNewBalance()
        {
            ApiResult ret = new ApiResult();
            try
            {
                var data = await _repo.GetPageList(new MyfundParm() { page = 1, rows = 1000, order = "asc", sort = "id" });
                foreach (var d in data.rows)
                {
                    string url = "https://api.doctorxiong.club/v1/fund?code=" + d.Code;
                    FundRetComm response = HttpClientHelper.GetResponse<FundRetComm>(url);
                    if (response.data != null)
                    {
                        var cur = response.data[0];
                        if (d.Worthdate < cur.worthDate && cur.dayGrowth != 0)//如果接口来的最新时间比db新，则更新最新balance
                        {
                            var newbalance = d.Balance * cur.dayGrowth / 100 + d.Balance;
                            await _repo.Update2(new Myfund() { Id = d.Id, Balance = newbalance, Costavg = d.Costavg });
                        }
                    }
                }
                ret.code = Code.Success;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }

            return ret;
        }

        public async Task<ApiResult> AddJingGai(JingGai parm)
        {
            ApiResult ret = new ApiResult();

            try
            {

                //parm.UserID = 40;
                var data = await _repo.SaveJingGai(parm);
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


        public async Task<ApiResult> AddJingGaiDevice(JingGaiDevice parm)
        {
            ApiResult ret = new ApiResult();

            try
            {

                //parm.UserID = 40;
                var data = await _repo.SaveJingGaiDevice(parm);
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


        public async Task<ApiResult> AddJingGaiAlarm(JingGaiAlarm parm)
        {
            ApiResult ret = new ApiResult();

            try
            {

                //parm.UserID = 40;
                var data = await _repo.SaveJingGaiAlarm(parm);
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


        public async Task<JingGaiDevicePageView> GetPageJingGaiDevice(JingGaiDeviceParm parm)
        {
            JingGaiDevicePageView ret = new JingGaiDevicePageView();

            try
            {
                ret = await _repo.GetPageJingGaiDevice(new JingGaiDeviceParm() { });
            }
            catch (Exception ex)
            {
                
            }

            return ret;
        }
        public async Task<ApiResult> UpdateNewBalanceV2()
        {
            ApiResult ret = new ApiResult();
            try
            {
                var data = await _repo.GetPageList(new MyfundParm() { page = 1, rows = 1000, order = "asc", sort = "id" });
                foreach (var d in data.rows)
                {
                    try
                    {
                        string url = $@"https://fundmobapi.eastmoney.com/FundMApi/FundBaseTypeInformation.ashx?FCODE=" + d.Code + "&deviceid=Wap&plat=Wap&product=EFund&version=2.0.0&Uid=9572315881384690&_=" + MathHelper.GetTimeStamp();
                        Root response = HttpClientHelper.GetResponse<Root>(url);
                        if (response.ErrCode == 0)
                        {
                            var cur = response.Datas;
                            if (d.Worthdate < Convert.ToDateTime(cur.FSRQ) && cur.RZDF != "0")//如果接口来的最新时间比db新，则更新最新balance
                            {
                                var newbalance = d.Balance * decimal.Parse(cur.RZDF) / 100 + d.Balance;
                                await _repo.Update2(new Myfund() { Id = d.Id, Balance = newbalance, Costavg = d.Costavg });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string exstr = ex.ToString();
                    }

                }
                ret.code = Code.Success;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }

            return ret;
        }

        public async Task<ApiResult> Algorithm(string s, string s1)
        {
            ApiResult ret = new ApiResult();
            try
            {
                string[] string1 = s1.Split(',');
                int[] parm = new int[string1.Length];
                for (int i = 0; i < string1.Length; i++)
                {
                    parm[i] = int.Parse(string1[i]);
                }
                ret.data = PivotIndex(parm);
                ret.code = Code.Success;
                //ret.data = data;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 724. 寻找数组的中心下标
        /// https://leetcode-cn.com/problems/find-pivot-index/solution/zi-ji-cuo-wu-de-whileleftrightde-si-lu-h-08o9/
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int PivotIndex(int[] nums)
        {
            int sum = 0;
            foreach (var n in nums)
            {
                sum += n;
            }
            int length = nums.Length;
            if (length == 0 || length == 2) return -1;
            if (length == 1) return 0;

            int sumLeft = 0;

            for (int i = 0; i < length; i++)
            {
                if (sumLeft == sum - nums[i]) return i;
                sumLeft += nums[i];
                sum -= nums[i];
            }

            return -1;
        }

        public int FindShortestSubArray(int[] nums)
        {
            Dictionary<int, int> left = new Dictionary<int, int>();//第一次出现的位置
            Dictionary<int, int> right = new Dictionary<int, int>();//最后一次出现的位置
            Dictionary<int, int> count = new Dictionary<int, int>();//出现的次数
            int maxDegree = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                var cur = nums[i];
                if (!count.ContainsKey(cur))
                {
                    count.Add(cur, 1);
                    maxDegree = 1;
                }
                else
                {
                    count[cur]++;
                    maxDegree = Math.Max(maxDegree, count[cur]);
                }

                if (!left.ContainsKey(cur))//没有就记录第一次出现,并赋予第一次right
                {
                    left.Add(cur, i);
                    right.Add(cur, i);
                }
                else
                {
                    right[cur] = i;//有的话则更新right
                }
            }

            int ret = nums.Length;
            foreach (KeyValuePair<int, int> kv in count)
            {
                var curDegreee = kv.Value;
                var curKey = kv.Key;
                if (curDegreee == maxDegree)
                {
                    int tmp = right[curKey] - left[curKey] + 1;
                    ret = Math.Min(ret, tmp);
                }
            }
            return ret;

        }

        public string[] FindRestaurant(string[] list1, string[] list2)
        {
            List<MyDic> mylist = new List<MyDic>();
            for (int i = 0; i < list1.Length; i++)
            {
                for (int j = 0; j < list2.Length; j++)
                {
                    if (list1[i] == list2[j])
                    {
                        mylist.Add(new MyDic() { MyKey = i + j, MyString = list1[i] });
                    }
                }
            }

            int curMin = mylist[0].MyKey;
            List<string> ret = new List<string>();
            ret.Add(mylist[0].MyString);
            for (int i = 1; i < mylist.Count; i++)
            {
                if (mylist[i].MyKey < curMin)
                {
                    ret.Clear();
                }
                else if (mylist[i].MyKey == curMin)
                {
                    ret.Add(mylist[i].MyString);
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// 496. 下一个更大元素 I   https://leetcode-cn.com/problems/next-greater-element-i/
        /// </summary>
        /// <param name="nums1"></param>
        /// <param name="nums2"></param>
        /// <returns></returns>
        public int[] NextGreaterElement(int[] nums1, int[] nums2)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            Stack<int> st = new Stack<int>();
            for (int i = 0; i < nums2.Length; i++)
            {
                while (st.Count != 0 && nums2[i] > st.Peek())
                {
                    dic.Add(st.Pop(),nums2[i]);
                }
                st.Push(nums2[i]);
            }
            while (st.Count != 0)
            {
                dic.Add(st.Pop(), -1);
            }
            for (int i = 0; i < nums1.Length; i++)
            {
                int cur = dic[nums1[i]];
                nums1[i] = cur;
            }
            return nums1;
        }
         

        public bool RepeatedSubstringPattern(string s)
        {
            string ss = s + s;
            return ss.IndexOf(s, 1) != s.Length;
        }

        /// <summary>
        /// 349. 两个数组的交集
        /// </summary>
        /// <param name="nums1"></param>
        /// <param name="nums2"></param>
        /// <returns></returns>
        private int[] Intersection(int[] nums1, int[] nums2)
        {
            HashSet<int> hash = new HashSet<int>();
            Array.Sort<int>(nums1);
            Array.Sort<int>(nums2);
            int i = 0;
            int j = 0;
            while (i < nums1.Length && j < nums2.Length)
            {
                if (nums1[i] == nums2[j])
                {
                    if (!hash.Contains(nums1[i]))
                    {
                        hash.Add(nums1[i]);
                    }
                    i++;
                    j++;
                }
                else if (nums1[i] < nums2[j])
                {
                    i++;
                }
                else if (nums1[i] > nums2[j])
                {
                    j++;
                }
            }
            int[] ret = new int[hash.Count];
            int index = 0;
            foreach(var tmp in hash)
            {
                ret[index++] = tmp;
            }
            return ret;
        }

        private bool WordPattern(string pattern, string s)
        {
            string[] sArr = s.Split(' ');
            StringBuilder s1 = new StringBuilder();
            StringBuilder s2 = new StringBuilder();
            for (int i = 0; i < pattern.Length; i++)
            {
                s1.Append(pattern.IndexOf(pattern[i]));
                s2.Append(sArr.IndexOf(sArr[i]));
            }
            return s1.ToString() == s2.ToString();
        }

        /// <summary>
        /// 234. 回文链表
        /// https://leetcode-cn.com/problems/palindrome-linked-list/solution/di-gui-zhan-deng-3chong-jie-jue-fang-shi-zui-hao-d/
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        private bool IsPalindrome(ListNode head)
        {
            ListNode tmp = head;
            Stack<int> st = new Stack<int>();
            while(tmp != null)
            {
                st.Push(tmp.val);
                tmp = tmp.next;
            }
            while (head != null)
            {
                if (head.val != st.Pop())
                {
                    return false;
                }
                head = head.next;
            }
            return true;
        }
        private bool ContainsNearbyDuplicate(int[] nums, int k)
        {
            Queue<int> q = new Queue<int>();
            for (int i = 0; i < nums.Length; i++)
            {
                if (q.Contains(nums[i]))
                {
                    return true;
                }
                q.Enqueue(nums[i]);
                if (q.Count > k)
                {
                    q.Dequeue();
                }
            }
            return false;
        }

        /// <summary>
        /// 205. 同构字符串 https://leetcode-cn.com/problems/isomorphic-strings/
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool IsIsomorphic(string s, string t)
        {
            StringBuilder s1 = new StringBuilder();
            StringBuilder t1 = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                s1.Append(s.IndexOf(s[i]));
                t1.Append(t.IndexOf(t[i]));
            }
            return s1.ToString() == t1.ToString();
        }

        private int LengthOfLastWord(string s)
        {
            int ret = 0;
            int end = -1;
            int start = -1;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] == ' ')
                {
                    continue;
                }
                else
                {
                    if (end == -1)
                    {
                        end = i;
                        start = end;
                        ret = end - start + 1;
                        if (start -1  == -1)
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        
                        if (s[start - 1] == ' ')
                        {
                            ret = end - start + 1;
                            break;
                        }
                        else
                        {
                            start--;
                        }
                        ret = end - start + 1;
                    }
                }

            }
            return ret;
        }

        /// <summary>
        /// 二进制求和
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private string AddBinary(string a, string b)
        {
            string ret = string.Empty;
            if (a.Length > b.Length)
            {
                b = b.PadLeft(a.Length, '0');
            }
            if (a.Length < b.Length)
            {
                a = a.PadLeft(b.Length, '0');
            }
            int carry = 0;
            for (int i = a.Length - 1; i >= 0; i--)
            {
                int curold = (int.Parse(a[i].ToString()) + int.Parse(b[i].ToString()));//当前无脑加起来的和
                int curnew = (curold + carry);//当前加上上一位的进位新的数字
                carry = curnew >= 2 ? 1 : 0;//下一位是否进位
                curnew = curnew % 2;//计算出当前应该是几
                ret = curnew + ret;//拼接结果
            }
            ret = carry == 1 ? 1 + ret : ret;

            return ret;
        }

        /// <summary>
        /// https://leetcode-cn.com/problems/maximum-subarray/solution/hua-jie-suan-fa-53-zui-da-zi-xu-he-by-guanpengchn/
        /// 最大子序和
        /// 神奇的正数增益
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private int MaxSubArray(int[] nums)
        {
            int ret = nums[0];
            int sum = 0;
            foreach (var num in nums)
            {
                if (sum <= 0)
                {
                    sum = num;
                }
                else
                {
                    sum += num;
                }
                ret = Math.Max(ret, sum);
            }
            return ret;
        }

        private int MaxDepth(TreeNode nodes)
        {
            if (nodes == null)
            {
                return 0;
            }
            int deep = 0;
            Queue<TreeNode> que = new Queue<TreeNode>();
            que.Enqueue(nodes);
            while (que.Count != 0)
            {
                deep++;
                int size = que.Count;
                if (size > 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        TreeNode cur = que.Dequeue();
                        if (cur.left != null)
                        {
                            que.Enqueue(cur.left);
                        }
                        if (cur.right != null)
                        {
                            que.Enqueue(cur.right);
                        }
                    }
                }
            }
            return deep;
        }

        /// <summary>
        /// 前序打印二叉树
        /// </summary>
        /// <param name="tree"></param>
        private string PreOrder(TreeNode tree)
        {
            string ret = string.Empty;
            Stack<TreeNode> st = new Stack<TreeNode>();
            st.Push(tree);
            while (st.Count != 0)
            {
                TreeNode t = st.Pop();
                ret += t.val + ",";
                if (t.right != null)
                {
                    st.Push(t.right);
                }
                if (t.left != null)
                {
                    st.Push(t.left);
                }
            }
            return ret;
        }

        private void FirstPrint(TreeNode node)
        {
            if (node == null)
            {
                return;
            }
            Console.Write(node.val);
            FirstPrint(node.left);
            FirstPrint(node.right);
        }

        private TreeNode CreateTree()
        {
            TreeNode nodeA = new TreeNode();
            nodeA.val = 1;
            TreeNode nodeB = new TreeNode();
            nodeB.val = 2;
            TreeNode nodeC = new TreeNode();
            nodeC.val = 3;
            TreeNode nodeD = new TreeNode();
            nodeD.val = 4;
            TreeNode nodeE = new TreeNode();
            nodeE.val = 5;
            TreeNode nodeF = new TreeNode();
            nodeF.val = 6;

            nodeB.left = nodeD;
            nodeB.right = nodeE;
            nodeC.left = nodeF;
            nodeA.left = nodeB;
            nodeA.right = nodeC;
            return nodeA;

        }

        /// <summary>
        /// 加一
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        public int[] PlusOne(int[] digits)
        {
            int length = digits.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                if (digits[i] != 9)
                {
                    digits[i]++;//当前的位置不是9的加1即可
                    return digits;
                }
                else
                {
                    digits[i] = 0;//当前是9的肯定变成0，进位不用管
                }
            }
            digits = new int[length + 1];//重新分配一个本来长度+1的int数组，里面全是0，也就这一种情况int数组会加长
            digits[0] = 1;

            return digits;
        }
        /// <summary>
        /// 删除排序链表中的重复元素
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        private ListNode DeleteDuplicates(ListNode head)
        {
            ListNode p = head;
            while (p != null)
            {
                if (p.next == null)
                {
                    break;
                }
                int nextval = p.next.val;
                if (p.val == nextval)
                {
                    p.next = p.next.next;
                }
                else
                {
                    p = p.next;
                }
            }
            return head;
        }

        public ListNode deleteDuplicates2(ListNode head)
        {
            if (head == null || head.next == null)
            {
                return head;
            }

            head.next = deleteDuplicates2(head.next);

            return head.val == head.next.val ? head.next : head;
        }

        /// <summary>
        /// 异或方法找不同
        /// 1，位运算解决
        ///这题说的是字符串t只比s多了一个字符，其他字符他们的数量都是一样的，如果我们把字符串s和t合并就会发现
        ///，除了那个多出的字符出现奇数次，其他的所有字符都是出现偶数次。
        /// 一个数和0做XOR运算等于本身：a⊕0 = a
        /// 一个数和其本身做XOR运算等于 0：a⊕a = 0
        /// XOR 运算满足交换律和结合律：a⊕b⊕a = (a⊕a)⊕b = 0⊕b = b
        /// char[][] board = new char[9][];
        //board[0] = new char[9] { '.', '.', '.', '.', '5', '.', '.', '1', '.' };
        //board[1] = new char[9] { '.', '4', '.', '3', '.', '.', '.', '.', '.' };
        //board[2] = new char[9] { '.', '.', '.', '.', '.', '3', '.', '.', '1' };
        //board[3] = new char[9] { '8', '.', '.', '.', '.', '.', '.', '2', '.' };
        //board[4] = new char[9] { '.', '.', '2', '.', '7', '.', '.', '.', '.' };
        //board[5] = new char[9] { '.', '1', '5', '.', '.', '.', '.', '.', '.' };
        //board[6] = new char[9] { '.', '.', '.', '.', '.', '2', '.', '.', '.' };
        //board[7] = new char[9] { '.', '2', '.', '9', '.', '.', '.', '.', '.' };
        //board[8] = new char[9] { '.', '.', '4', '.', '.', '.', '.', '.', '.' };
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private char FindTheDifference(string s, string t)
        {
            char ret = char.MinValue;
            char[] arr = (s + t).ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                ret ^= arr[i];
            }
            return ret;
        }


        private bool IsValidSudoku(char[][] board)
        {
            bool ret = true;
            //横扫字典
            Dictionary<int, int> dic1 = new Dictionary<int, int>() { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 }, { 9, 0 } };
            //竖扫字典
            Dictionary<int, int> dic2 = new Dictionary<int, int>() { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 }, { 9, 0 } };
            //宫扫字典
            Dictionary<int, int> dic3 = new Dictionary<int, int>() { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 }, { 9, 0 } };
            for (int i = 0; i < 9; i++)
            {
                //按宫扫先求下标，每个宫的第一个格的下标
                // 每个宫的第一个格子是 (036)(036)的9种组合，这个是按宫的外部循环
                //(0,0)(0,3)(0,6)
                //(3,0)(3,3)(3,6)
                //(6,0)(6,3)(6,6)
                int i1 = i - (i % 3);// 012 345 678 要转化成000 333 666，每个都是差0,1,2
                int j1 = (i % 3) * 3;// 012 345 678 要转化成036 036 036
                for (int j = 0; j < 9; j++)
                {
                    if (board[i][j] != '.')//横扫描
                    {
                        int cur = int.Parse(board[i][j].ToString());
                        if (dic1[cur] > 0)
                        {
                            ret = false;
                            break;
                        }
                        else
                        {
                            dic1[cur]++;
                        }
                    }
                    if (board[j][i] != '.')//竖扫描
                    {
                        int cur = int.Parse(board[j][i].ToString());
                        if (dic2[cur] > 0)
                        {
                            ret = false;
                            break;
                        }
                        else
                        {
                            dic2[cur]++;
                        }
                    }

                    int gapi = j / 3;//012 345 678转化成加000 111 222 如(0,3)(0,4)(0,5)(1,3)(1,4)(1,5)(2,3)(2,4)(2,5)
                    int gapj = j % 3;//012 345 678转化成加012 012 012
                    int curi = i1 + gapi;
                    int curj = j1 + gapj;
                    if (board[curi][curj] != '.')//宫扫描
                    {
                        int cur = int.Parse(board[curi][curj].ToString());
                        if (dic3[cur] > 0)
                        {
                            ret = false;
                            break;
                        }
                        else
                        {
                            dic3[cur]++;
                        }
                    }
                }
                //扫完一遍清空字典
                GetNewDic(dic1);
                GetNewDic(dic2);
                GetNewDic(dic3);
            }
            return ret;
        }

        private void GetNewDic(Dictionary<int, int> dic)
        {
            for (int i = 1; i <= 9; i++)
            {
                dic[i] = 0;
            }
        }

        public int StrStr(string haystack, string needle)
        {
            int ret = 0;
            if (string.IsNullOrEmpty(needle))
            {
                return ret;
            }
            int haylength = haystack.Length;
            int needlelength = needle.Length;
            for (int i = 0; i <= haylength - needlelength; i++)
            {
                string curchararr = haystack.Substring(i, needlelength);
                if (curchararr == needle)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 最长公共字符前缀
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string LongestCommonPrefix(string[] strs)
        {
            string ret = string.Empty;
            if (strs.Length == 0)
            {
                return ret;
            }

            int min = strs[0].Length;
            //先找出最短字符的长度
            for (int i = 1; i < strs.Length; i++)
            {
                string t = strs[i];
                if (t.Length < min)
                {
                    min = t.Length;
                }
            }

            for (int i = 0; i < min; i++)
            {
                char curchar = char.MinValue;
                bool flag = true;
                for (int j = 0; j < strs.Length; j++)
                {
                    string temp = strs[j];
                    if (j == 0)
                    {
                        curchar = temp[i];
                    }
                    else
                    {
                        if (temp[i] != curchar)
                        {
                            flag = false;
                            break;//该字符位置的字符有不一样的
                        }
                    }
                }
                if (!flag)
                {
                    break;//一旦有端的就全部退出
                }
                if (flag)
                {
                    ret += curchar;
                }
            }
            return ret;
        }
        /// <summary>
        /// 数组转链表
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private ListNode ConvertToNode(string[] arr)
        {
            ListNode nodes = new ListNode() { val = int.Parse(arr[0]) };
            ListNode p = nodes;
            for (int i = 1; i < arr.Length; i++)
            {
                p.next = new ListNode() { val = int.Parse(arr[i]) };
                p = p.next;
            }
            return nodes;
        }

        private string ConvertToString(ListNode nodes)
        {
            string ret = string.Empty;
            if (nodes == null)
            {
                return ret;
            }
            ListNode p = nodes;
            while (p != null)
            {
                int cur = p.val;
                ret = cur + ret;
                p = p.next;
            }
            return ret;
        }

        private string ReverseToString(string n)
        {
            string ret = string.Empty;
            for (int i = 0; i < n.Length; i++)
            {
                ret = n[i] + "," + ret;
            }
            ret = ret.TrimEnd(',');
            return ret;
        }

        private int RemoveElement(int[] nums, int val)
        {
            if (nums.Length == 0)
            {
                return 0;
            }
            int length = nums.Length;
            //int i = 0;
            int j = 0;
            while (j < length)
            {
                //把后面不是val的提上来
                if (nums[j] == val)
                {
                    int havenotval = 0;//记录当前j后面的数字是否有和val不一样的，不一样说明有交换位置的操作
                    for (int k = j; k < length - 1; k++)
                    {
                        if (nums[k + 1] != val)
                        {
                            havenotval++;
                        }
                        int c = nums[k];
                        nums[k] = nums[k + 1];
                        nums[k + 1] = c;
                    }
                    j++;
                    if (nums[j - 1] == val && havenotval != 0)//如果当前交换后的前一个数字仍然是val并且确实交换过，则j回退，说明当前交换后第一个数字还是val，得再次逐个移动后面的数字向前。如果havenotval是0，说明后面的数字全是和val一样的，j不需要做回退操作，任然沿用上面的j++往后移。
                    {
                        j--;
                    }
                    //if (havenotval != 0)
                    //{
                    //    i++;
                    //}
                }
                else
                {
                    j++;//当前快指针如果不等于val则向后移
                }
            }
            int ret = length;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == val)
                {
                    ret--;
                }
            }
            return ret;
        }



        private ListNode AddTwoNumsV2(ListNode l1, ListNode l2)
        {
            ListNode ret = new ListNode();
            ListNode p = ret;
            int carry = 0;//进位
            while (l1 != null || l2 != null)
            {
                int cur1 = l1 != null ? l1.val : 0;
                int cur2 = l2 != null ? l2.val : 0;
                int curtmp = cur1 + cur2;//当前应该加起来等于几
                int cur = (curtmp + p.val) % 10;//当前个位数
                if ((curtmp + p.val) / 10 > 0)
                {
                    carry = 1;
                }
                else
                {
                    carry = 0;
                }
                p.val = cur;
                if ((l1 != null && l1.next != null) || (l2 != null && l2.next != null))
                {
                    p.next = new ListNode() { val = carry };
                    p = p.next;
                }

                if (l1 != null)
                {
                    l1 = l1.next;
                }
                if (l2 != null)
                {
                    l2 = l2.next;
                }
                if (l1 == null && l2 == null && carry == 1)
                {
                    p.next = new ListNode() { val = 1 };
                }

                //if (l1.next == null && l2.next == null)
                //{
                //    break;
                //}
            }
            return ret;
        }

        private bool IsValid(string s)
        {
            Stack<char> st = new Stack<char>();
            Dictionary<char, char> dict = new Dictionary<char, char>() { { ')', '(' }, { '}', '{' }, { ']', '[' } };
            for (int i = 0; i < s.Length; i++)
            {
                if (dict.ContainsValue(s[i]))
                {
                    st.Push(s[i]);//凡是左括号的就直接入栈
                }
                else if (st.Count() == 0)
                {
                    return false;//如果第一个入的是右括号，则直接返回false,说明右括号不能在左括号之前出现
                }
                else if (dict[s[i]] != st.Pop())
                {
                    return false;//如果当前准备入栈的右括号先去通过dic匹配他的左括号和已经在栈里的第一个比较，如果不相等说明没有匹配成功
                }
            }
            return st.Count == 0;//如果栈为空，括号全部配对完，返回true
        }

        public void ReverseString(char[] s)
        {
            int n = s.Length;
            int middle = n / 2;
            for (int i = 0; i < middle;)
            {
                char m = s[i];
                s[i] = s[n - 1];
                s[n - 1] = m;
                i++;
                n--;
            }
        }

        public void reverseStringHelper(char[] s, int left, int right)
        {
            if (left >= right)
                return;
            swap(s, left, right);
            reverseStringHelper(s, ++left, --right);
        }

        private void swap(char[] array, int i, int j)
        {
            char temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        public bool LemonadeChange(int[] bills)
        {
            int dic5 = 0;
            int dic10 = 0;
            for (int i = 0; i < bills.Length; i++)
            {
                if (bills[i] == 5)
                {
                    dic5 += 1;
                }
                else if (bills[i] == 10)
                {
                    if (dic5 > 0)
                    {
                        dic10 += 1;
                        dic5 -= 1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (bills[i] == 20)
                {
                    if (dic10 > 0 && dic5 > 0)
                    {
                        dic5 -= 1;
                        dic10 -= 1;
                    }
                    else if (dic5 > 2)
                    {
                        dic5 = dic5 - 3;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            return true;
        }


        private int getNum(ListNode node)
        {
            int number = 0;
            int deep = 0;
            int cur = node.val;
            int curnum = deep * 10;
            if (curnum != 0)
            {
                number += curnum;
            }
            else
            {
                number = cur;
            }
            deep++;
            getNum(node.next);
            return number;
        }

    }



    public class KthLargest
    {
        int[] Nums;
        int K;
        public KthLargest(int k, int[] nums)
        {
            this.Nums = new int[nums.Length];
            this.K = k;
            for (int i = 0; i < nums.Length; i++)
            {
                this.Nums[i] = nums[i];
            }
            //Array.Sort(this.Nums);
        }

        public int Add(int val)
        {
            int[] arr = new int[this.Nums.Length + 1];
            for (int i = 0; i < this.Nums.Length; i++)
            {
                arr[i] = this.Nums[i];
            }
            arr[arr.Length - 1] = val;
            this.Nums = new int[arr.Length];
            for (int i = 0; i < this.Nums.Length; i++)
            {
                this.Nums[i] = arr[i];
            }
            Array.Sort(this.Nums);
            return this.Nums[this.Nums.Length - this.K];
        }

        
    }
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }

    public class TreeNode
    {
        public int val;
        public TreeNode left { get; set; }
        public TreeNode right { get; set; }

    }

    public class MyDic
    {
        public int MyKey { get; set; }
        public string MyString { get; set; }
    }

    public interface IWorkTaskService
    {
        Task<ApiResult> Algorithm(string s, string s1);
        Task<ApiResult> GetReadyTasks(WorkTaskQueryParm parm);
        Task<ApiResult> GetFinishTasks(WorkTaskQueryParm parm);
        Task<ApiResult> GetPageMyApply(WorkTaskQueryParm parm);
        Task<ApiResult> GetPageActivityInstance(WorkQueryParm parm);
        Task<WfRet> StartProcess(WfReq parm);
        Task<WfRet> GetNextStepRoleUserTree(WfReq parm);
        Task<WfRet> NextProcess(WfReq parm);
        Task<WfRet> WithdrawProcess(WfReq parm);
        Task<WfRet> SendBackProcess(WfReq parm);
        Task<WfRet> GetProcessListSimple();
        Task<WfRet> ReverseProcess(WfReq parm);
        Task<WfRet> CancelProcess(WfReq parm);
        Task<WfRet> QueryReadyActivityInstance(WfReq parm);
        Task<WfRet> QueryCompletedTasks(WfReq parm);
        Task<ApiResult> InitData();
        Task<ApiResult> InitData2();
        Task<ApiResult> GetPageList(MyfundParm parm);
        Task<ApiResult> Update2(Myfund obj);
        Task<ApiResult> GetById(int id);
        Task<ApiResult> UpdateNewBalance();
        Task<ApiResult> UpdateNewBalanceV2();
        Task<ApiResult> InitData2V2();

        Task<ApiResult> AddJingGai(JingGai parm);
        Task<ApiResult> AddJingGaiDevice(JingGaiDevice parm);
        Task<ApiResult> AddJingGaiAlarm(JingGaiAlarm parm);
        Task<JingGaiDevicePageView> GetPageJingGaiDevice(JingGaiDeviceParm parm);
    }


}
