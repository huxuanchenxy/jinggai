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
using Quartz;

namespace SZY.Platform.WebApi.Service
{
    public class FundJob : IJob//创建IJob的实现类，并实现Excute方法。
    {
        private readonly IWorkTaskRepo<TaskViewModel> _repo;
        public FundJob(IWorkTaskRepo<TaskViewModel> repo)
        {
            _repo = repo;
        }
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(DateTime.Now);
                string codes = string.Empty;
                MyfundParm parm = new MyfundParm() { page = 1, rows = 1000, sort = "id", order = "asc" };
                var data = _repo.GetPageList(parm).Result;
                //foreach (var d in data.rows)
                //{
                //    codes += d.Code + ",";
                //}
                //string url = "https://api.doctorxiong.club/v1/fund?code=" + codes;
                //FundRetComm response = HttpClientHelper.GetResponse<FundRetComm>(url);
                foreach (var d in data.rows)
                {
                    try
                    {
                        //string url = "https://api.doctorxiong.club/v1/fund?code=" + d.Code;
                        //FundRetComm response = HttpClientHelper.GetResponse<FundRetComm>(url);
                        string url = $@"https://fundmobapi.eastmoney.com/FundMApi/FundValuationDetail.ashx?FCODE=" + d.Code + "&deviceid=Wap&plat=Wap&product=EFund&version=2.0.0&Uid=9572315881384690&_=" + MathHelper.GetTimeStamp();
                        Root2 response = HttpClientHelper.GetResponse<Root2>(url);
                        if (response.ErrCode == 0)
                        {
                            var cur = response.Datas[0];
                            if (cur != null)
                            {
                                if (!string.IsNullOrEmpty(cur.gszzl))
                                {
                                    _repo.UpdateExpectGrowth(new Myfund() { Id = d.Id, ExpectGrowth = decimal.Parse(cur.gszzl) });
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }


                }
            });
        }
    }


}
